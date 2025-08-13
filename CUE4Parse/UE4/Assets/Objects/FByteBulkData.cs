using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.CompilerServices;
using CUE4Parse.FileProvider.Vfs;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Assets.Utils;
using CUE4Parse.UE4.Exceptions;
using Newtonsoft.Json;
using Serilog;
using static CUE4Parse.UE4.Assets.Objects.EBulkDataFlags;

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
        }

        protected FByteBulkData(FAssetArchive Ar, bool skip = false)
        {
            Header = new FByteBulkDataHeader(Ar);

           
        }

        private void CheckReadSize(int read) 
        {
            if (read != Header.ElementCount) {
                Log.Warning("Read {read} bytes, expected {Header.ElementCount}", read, Header.ElementCount);
            }
        }

        public bool ReadBulkDataInto(byte[] data, int offset = 0) 
        {
            if (data.Length - offset < Header.ElementCount) {
                Log.Error("Data buffer is too small");
                return false;
            }

            var Ar = (FAssetArchive)_savedAr.Clone(); // TODO: remove and use FArchive.ReadAt
            Ar.Position = _dataPosition;
      
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
