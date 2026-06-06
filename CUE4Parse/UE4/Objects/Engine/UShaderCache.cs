using System;
using System.Collections.Generic;
using CUE4Parse.UE4.Assets.Exports.Material;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Versions;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.Engine
{
    public class FIndividualCompressedShaderInfo
    {
        public ushort ChunkIndex;
        public ushort UncompressedCodeLength;
        public int UncompressedCodeOffset;
        public FIndividualCompressedShaderInfo(FAssetArchive Ar)
        {
            ChunkIndex = Ar.Read<ushort>();
            UncompressedCodeOffset = Ar.Read<int>();
            UncompressedCodeLength = Ar.Read<ushort>();
        }
    }

    public class FCompressedShaderCodeChunk
    {
        public int UncompressedSize;
        public byte[] CompressedCode;

        public FCompressedShaderCodeChunk(FAssetArchive Ar)
        {
            UncompressedSize = Ar.Read<int>();
            CompressedCode = Ar.ReadArray<byte>();
        }
    }

    public class FTypeSpecificCompressedShaderCode
    {
        // Map from shader guid to the information required to decompress that shader
        public Dictionary<Guid, FIndividualCompressedShaderInfo> CompressedShaderInfos;

        // Code chunks for this shader type that were split apart due to size limits
        public FCompressedShaderCodeChunk[] CodeChunks;

        public FTypeSpecificCompressedShaderCode(FAssetArchive Ar)
        {
            CompressedShaderInfos = Ar.ReadMap(() => Ar.Read<Guid>(), () => new FIndividualCompressedShaderInfo(Ar));
            CodeChunks = Ar.ReadArray(() => new FCompressedShaderCodeChunk(Ar));
        }
    }
    public struct FShader
    {
        public enum ShaderFrequency : byte
        {
            Vertex = 0,
            Pixel = 1,
            PixelUDK = 3
        }

        public FName ShaderType;
        public FGuid Guid;
        public ShaderFrequency Frequency;
        public byte[] ShaderByteCode;
        public uint ParameterMapCRC;
        public int InstructionCount;
        public byte Platform;
        public FName? VertexFactoryType;

        public FShader(FAssetArchive Ar)
        {
            if (Ar.Ver >= EUnrealEngineObjectUE3Version.FIXED_AUTO_SHADER_VERSIONING)
            {
                Ar.ReadArray<short>();
            }

            Platform = Ar.Read<byte>();
            Frequency = (ShaderFrequency)Ar.Read<byte>();
            ShaderByteCode = Ar.ReadArray<byte>();
            ParameterMapCRC = Ar.Read<uint>();
            Guid = Ar.Read<FGuid>();
            ShaderType = Ar.ReadFName();
            if (Ar.Ver >= EUnrealEngineObjectUE3Version.FIXED_AUTO_SHADER_VERSIONING)
            {
                new FSHAHash(Ar); // SavedHash
            }
            InstructionCount = Ar.Read<int>();
            //VertexFactoryType = Ar.ReadFName();
        }
    }
    public class UShaderCache : Assets.Exports.UObject
    {
        public EShaderPlatform Platform;
        public int ShaderCachePriority;
        public FShader[] shaders;
        public Dictionary<FName, int>? ShaderTypeMap;
        public Dictionary<FName, int>? VertexFactoryMap;

        public override void Deserialize(FAssetArchive Ar, long validPos)
        {
            base.Deserialize(Ar, validPos);

            if (Ar.Ver > EUnrealEngineObjectUE3Version.SHADER_CACHE_PRIORITY)
            {
                ShaderCachePriority = Ar.Read<int>();
            }

            if (Ar.Ver < EUnrealEngineObjectUE3Version.GLOBAL_SHADER_FILE)
            {
                Platform = Ar.Read<EShaderPlatform>();
                ShaderTypeMap = Ar.ReadMap(Ar.ReadFName, () => Ar.Read<int>());
                VertexFactoryMap = Ar.ReadMap(Ar.ReadFName, () => Ar.Read<int>());
            }
            else
            {
                Platform = Ar.Read<EShaderPlatform>();
                if (Ar.Ver < EUnrealEngineObjectUE3Version.FIXED_AUTO_SHADER_VERSIONING)
                {
                    ShaderTypeMap = Ar.ReadMap(Ar.ReadFName, () => Ar.Read<int>());
                }
            }

            if (Ar.Ver >= EUnrealEngineObjectUE3Version.SHADER_COMPRESSION)
            {
                Ar.ReadMap(Ar.ReadFName, () => new FTypeSpecificCompressedShaderCode(Ar));
            }

            var NumShaders = Ar.Read<int>();

            for (int i = 0; i < NumShaders; i++)
            {
                Ar.ReadFName(); // ShaderType

                Ar.Read<FGuid>(); // ShaderId

                if (Ar.Ver >= EUnrealEngineObjectUE3Version.FIXED_AUTO_SHADER_VERSIONING)
                {
                    new FSHAHash(Ar); // SavedHash
                }

                var SkipOffset = Ar.Read<int>();
                //shaders = Ar.ReadArray(() => new FShader(Ar));
                Ar.Position = SkipOffset;
            }

            Ar.Read<int>(); // Array but complex, so just read array count
        }

        protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer)
        {
            base.WriteJson(writer, serializer);

            writer.WritePropertyName("Platform");
            serializer.Serialize(writer, Platform);

            writer.WritePropertyName("ShaderCachePriority");
            writer.WriteValue(ShaderCachePriority);

            writer.WritePropertyName("shaders");
            serializer.Serialize(writer, shaders);

            if (ShaderTypeMap?.Count > 0)
            {
                writer.WritePropertyName("ShaderTypeMap");
                serializer.Serialize(writer, ShaderTypeMap);
            }

            if (VertexFactoryMap?.Count > 0)
            {
                writer.WritePropertyName("VertexFactoryMap");
                serializer.Serialize(writer, VertexFactoryMap);
            }
        }
    }
}
