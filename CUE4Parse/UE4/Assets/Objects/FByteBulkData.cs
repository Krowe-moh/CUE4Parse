using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using CUE4Parse.Compression;
using CUE4Parse.FileProvider.Vfs;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Assets.Utils;
using CUE4Parse.UE4.Exceptions;
using CUE4Parse.UE4.Readers;
using Newtonsoft.Json;
using Serilog;
using static CUE4Parse.UE4.Assets.Objects.EBulkDataFlags;

public class FCompressedChunkBlock
{
    public int CompressedSize;
    public int UncompressedSize;

    public FCompressedChunkBlock(FArchive Ar)
    {
        CompressedSize = Ar.Read<int>();
        UncompressedSize = Ar.Read<int>();
    }
}

public class FCompressedChunkHeader
{
    public int Tag;
    public int ChunkSize;
    public FCompressedChunkBlock Sum;
    public FCompressedChunkBlock[] Blocks;
    public FCompressedChunkHeader(FArchive Ar)
    {
        Tag = Ar.Read<int>();
        ChunkSize = Ar.Read<int>();
        // ignore, i'm lazy too finish rn
        ChunkSize = 0x20000;

        Sum = new FCompressedChunkBlock(Ar);
        int blockCount = (Sum.UncompressedSize + ChunkSize - 1) / ChunkSize;
        Blocks = new FCompressedChunkBlock[blockCount];
        int compSize = 0, uncompSize = 0, i = 0;

        while (compSize < Sum.CompressedSize && uncompSize < Sum.UncompressedSize)
        {
            var block = new FCompressedChunkBlock(Ar);
            Blocks[i++] = block;

            compSize += block.CompressedSize;
            uncompSize += block.UncompressedSize;
        }

        if (Blocks.Length > 1)
            ChunkSize = Blocks[0].UncompressedSize;

        if (uncompSize != Sum.UncompressedSize)
            throw new Exception("Header validation failed: uncompressed size mismatch");
    }
}

namespace CUE4Parse.UE4.Assets.Objects
{
    [JsonConverter(typeof(FByteBulkDataConverter))]
    public class FByteBulkData
    {
        public static bool LazyLoad = true;

        public readonly FByteBulkDataHeader Header;
        public EBulkDataFlags BulkDataFlags => Header.BulkDataFlags;

        public byte[]? Data => _data?.Value;
        private readonly Lazy<byte[]?>? _data;

        private readonly FAssetArchive _savedAr;
        private readonly long _dataPosition;
        
        public static void ReadCompressedChunk(FArchive Ar, byte[] buffer, CompressionMethod Compression)
        {
            var header = new FCompressedChunkHeader(Ar);

            var readBuffer = new byte[header.ChunkSize * 16]; // ignore everything here 
            int bufferOffset = 0;

            foreach (var block in header.Blocks)
            {
                if (block.CompressedSize > readBuffer.Length)
                    throw new Exception("Block compressed size exceeds read buffer");
                if (block.UncompressedSize + bufferOffset > buffer.Length)
                    throw new Exception("Block uncompressed size exceeds output buffer");

                Ar.Read(readBuffer, 0, block.CompressedSize);

                if (block.CompressedSize <= 36 && block.UncompressedSize <= 32)
                {
                    Buffer.BlockCopy(readBuffer, 0, buffer, bufferOffset, block.UncompressedSize);
                }
                else
                {
                    CUE4Parse.Compression.Compression.Decompress(
                        readBuffer, 0, block.CompressedSize,
                        buffer, bufferOffset, block.UncompressedSize,
                        Compression, Ar
                    );
                }

                bufferOffset += block.UncompressedSize;
            }

            if (bufferOffset != buffer.Length)
                throw new Exception("Buffer not fully filled by decompressed data");
        }

        public FByteBulkData(byte[] data)
        {
            _data = new Lazy<byte[]>(data);
        }

        public FByteBulkData(Lazy<byte[]?> data)
        {
            _data = data;
        }

        public FByteBulkData(FAssetArchive Ar)
        {
            Header = new FByteBulkDataHeader(Ar);
            if (Header.ElementCount == 0 || BulkDataFlags.HasFlag(BULKDATA_Unused))
            {
                // Log.Warning("Bulk with no data");
                return;
            }

            _dataPosition = Ar.Position;
            _savedAr = Ar;

            if (BulkDataFlags.HasFlag(BULKDATA_ForceInlinePayload))
            {
                Ar.Position += Header.ElementCount;
            }
            else if (BulkDataFlags.HasFlag(BULKDATA_SerializeCompressedZLIB))
            {
                var data = new byte[Header.ElementCount];
                ReadCompressedChunk(Ar, data, CompressionMethod.Zlib);
                _data = new Lazy<byte[]>(() => data);
                return;
            }
            else if (BulkDataFlags.HasFlag(BULKDATA_CompressedLZO))
            {
                var data = new byte[Header.ElementCount];
                ReadCompressedChunk(Ar, data, CompressionMethod.LZO);
                _data = new Lazy<byte[]>(() => data);
                return;
            }


            if (LazyLoad)
            {
                _data = new Lazy<byte[]?>(() =>
               {
                    var data = new byte[Header.ElementCount];
                    return ReadBulkDataInto(data) ? data : null;
                });
            }
            else
            {
                var data = new byte[Header.ElementCount];
                if (ReadBulkDataInto(data)) _data = new Lazy<byte[]?>(() => data);
            }
            Ar.Position += Header.ElementCount;
        }
        protected FByteBulkData(FAssetArchive Ar, bool skip = false)
        {
            Header = new FByteBulkDataHeader(Ar);

            if (BulkDataFlags.HasFlag(BULKDATA_Unused | BULKDATA_PayloadInSeperateFile | BULKDATA_PayloadAtEndOfFile))
            {
                return;
            }

            if (BulkDataFlags.HasFlag(BULKDATA_ForceInlinePayload) || Header.OffsetInFile == Ar.Position)
            {
                Ar.Position += Header.SizeOnDisk;
            }
        }

        private void CheckReadSize(int read)
        {
            if (read != Header.ElementCount)
            {
                Log.Warning("Read {read} bytes, expected {Header.ElementCount}", read, Header.ElementCount);
            }
        }

        public bool ReadBulkDataInto(byte[] data, int offset = 0)
        {
            if (data.Length - offset < Header.ElementCount)
            {
                Log.Error("Data buffer is too small");
                return false;
            }

            var Ar = (FAssetArchive)_savedAr.Clone(); // TODO: remove and use FArchive.ReadAt
            Ar.Position = _dataPosition;
            if (BulkDataFlags.HasFlag(BULKDATA_ForceInlinePayload))
            {
#if DEBUG
                Log.Debug("bulk data in .uexp file (Force Inline Payload) (flags={BulkDataFlags}, pos={HeaderOffsetInFile}, size={HeaderSizeOnDisk}))", BulkDataFlags, Header.OffsetInFile, Header.SizeOnDisk);
#endif
                CheckReadSize(Ar.Read(data, offset, Header.ElementCount));
            }
            else if (BulkDataFlags.HasFlag(BULKDATA_OptionalPayload))
            {
#if DEBUG
                Log.Debug("bulk data in {CookedIndex}.uptnl file (Optional Payload) (flags={BulkDataFlags}, pos={HeaderOffsetInFile}, size={HeaderSizeOnDisk}))", Header.CookedIndex, BulkDataFlags, Header.OffsetInFile, Header.SizeOnDisk);
#endif
                if (!TryGetBulkPayload(Ar, PayloadType.UPTNL, out var uptnlAr)) return false;

                CheckReadSize(uptnlAr.ReadAt(Header.OffsetInFile, data, offset, Header.ElementCount));
            }
            else if (BulkDataFlags.HasFlag(BULKDATA_PayloadInSeperateFile))
            {
#if DEBUG
                Log.Debug("bulk data in {CookedIndex}.ubulk file (Payload In Separate File) (flags={BulkDataFlags}, pos={HeaderOffsetInFile}, size={HeaderSizeOnDisk}))", Header.CookedIndex, BulkDataFlags, Header.OffsetInFile, Header.SizeOnDisk);
#endif
                if (!TryGetBulkPayload(Ar, PayloadType.UBULK, out var ubulkAr)) return false;

                CheckReadSize(ubulkAr.ReadAt(Header.OffsetInFile, data, offset, Header.ElementCount));
            }
            else if (BulkDataFlags.HasFlag(BULKDATA_PayloadAtEndOfFile))
            {
#if DEBUG
                Log.Debug("bulk data in .uexp file (Payload At End Of File) (flags={BulkDataFlags}, pos={HeaderOffsetInFile}, size={HeaderSizeOnDisk}))", BulkDataFlags, Header.OffsetInFile, Header.SizeOnDisk);
#endif
                // stored in same file, but at different position
                // save archive position
                if (Header.OffsetInFile + Header.ElementCount <= Ar.Length)
                {
                    CheckReadSize(Ar.ReadAt(Header.OffsetInFile, data, offset, Header.ElementCount));
                }
                else throw new ParserException(Ar, $"Failed to read PayloadAtEndOfFile, {Header.OffsetInFile} is out of range");
            }
            else if (BulkDataFlags.HasFlag(BULKDATA_LazyLoadable) || BulkDataFlags.HasFlag(BULKDATA_None))
            {
                CheckReadSize(Ar.Read(data, offset, Header.ElementCount));
            }

            Ar.Dispose();
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool TryGetBulkPayload(FAssetArchive Ar, PayloadType type, [MaybeNullWhen(false)] out FAssetArchive payloadAr)
        {
            payloadAr = null;
            if (Header.CookedIndex.IsDefault)
            {
                Ar.TryGetPayload(type, out payloadAr);
            }
            else if (Ar.Owner?.Provider is IVfsFileProvider vfsFileProvider)
            {
                var path = Path.ChangeExtension(Ar.Name, $"{Header.CookedIndex}.{type.ToString().ToLowerInvariant()}");
                if (vfsFileProvider.TryGetGameFile(path, out var file) && file.TryCreateReader(out var reader))
                {
                    payloadAr = new FAssetArchive(reader, Ar.Owner);
                }
            }

            return payloadAr != null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetDataSize() => Header.ElementCount;
    }
}
