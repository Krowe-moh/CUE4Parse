using System;
using CUE4Parse.UE4.Assets.Readers;
using System.Collections.Generic;
using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Versions;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.Engine
{
    public class FPackageTreeEntry
    {
        public string FullObjectName;
        public string ClassName;

        public FPackageTreeEntry(FAssetArchive Ar)
        {
            FullObjectName = Ar.ReadFString();
            ClassName = Ar.ReadFString();
        }
    }

    public class FCookedBulkDataInfo
    {
        public long SavedBulkDataFlags;
        public int SavedElementCount;
        public int SavedBulkDataOffsetInFile;
        public int SavedBulkDataSizeOnDisk;
        public FName TextureFileCacheName;

        public FCookedBulkDataInfo(FAssetArchive Ar)
        {
            SavedBulkDataFlags = Ar.Read<int>();
            SavedElementCount = Ar.Read<int>();
            SavedBulkDataOffsetInFile = Ar.Read<int>();
            SavedBulkDataSizeOnDisk = Ar.Read<int>();
            TextureFileCacheName = Ar.ReadFName();
        }
    }

    public class FCookedTextureFileCacheInfo
    {
        public FGuid TextureFileCacheGuid;
        public FName TextureFileCacheName;
        public double LastSaved;

        public FCookedTextureFileCacheInfo(FAssetArchive Ar)
        {
            TextureFileCacheGuid = Ar.Read<FGuid>();
            TextureFileCacheName = Ar.ReadFName();
            LastSaved = Ar.Read<double>();
        }
    }

    public class UPersistentCookerData : Assets.Exports.UObject
    {
        public Dictionary<string, FPackageTreeEntry[]> ClassMap;
        public Dictionary<string, Dictionary<string, FPackageTreeEntry[]>> LocalizationMap;
        public Dictionary<string, FCookedBulkDataInfo> CookedBulkDataInfoMap;
        public Dictionary<string, FCookedTextureFileCacheInfo> CookedTextureFileCacheInfoMap;
        public Dictionary<string, double> FilenameToTimeMap;
        public Dictionary<string, int> FilenameToCookedVersion;
        public long TextureFileCacheWaste;
        public double LastNonSeekfreeCookTime;

        public override void Deserialize(FAssetArchive Ar, long validPos)
        {
            base.Deserialize(Ar, validPos);

            if (Ar.Ver > EUnrealEngineObjectUE3Version.VER_REMOVE_BAD_ANIMSEQ)
            {
                ClassMap = Ar.ReadMap(() => Ar.ReadFString(), () => Ar.ReadArray(() => new FPackageTreeEntry(Ar)));
                LocalizationMap = Ar.ReadMap(
                    () => Ar.ReadFString(),
                    () => Ar.ReadMap(() => Ar.ReadFString(), () => Ar.ReadArray(() => new FPackageTreeEntry(Ar)))
                );
                Ar.Read<int>();
                Ar.Read<int>();
            }
            CookedBulkDataInfoMap = Ar.ReadMap(() => Ar.ReadFString(), () => new FCookedBulkDataInfo(Ar));
            FilenameToTimeMap = Ar.ReadMap(() => Ar.ReadFString(), () => Ar.Read<double>());
            TextureFileCacheWaste = Ar.Read<long>();
            //LastNonSeekfreeCookTime = Ar.Read<double>();
            FilenameToCookedVersion = Ar.ReadMap(() => Ar.ReadFString(), () => Ar.Read<int>());
            if(Ar.Ver >= EUnrealEngineObjectUE3Version.VER_ADDED_TEXTURE_FILECACHE_GUIDS)
            {
                CookedTextureFileCacheInfoMap = Ar.ReadMap(() => Ar.ReadFString(), () => new FCookedTextureFileCacheInfo(Ar));
            }
        }
        protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer)
        {
            base.WriteJson(writer, serializer);

            writer.WritePropertyName("ClassMap");
            serializer.Serialize(writer, ClassMap);

            writer.WritePropertyName("LocalizationMap");
            serializer.Serialize(writer, LocalizationMap);

            writer.WritePropertyName("CookedBulkDataInfoMap");
            serializer.Serialize(writer, CookedBulkDataInfoMap);

            writer.WritePropertyName("FilenameToTimeMap");
            serializer.Serialize(writer, FilenameToTimeMap);

            writer.WritePropertyName("TextureFileCacheWaste");
            writer.WriteValue(TextureFileCacheWaste);

            writer.WritePropertyName("LastNonSeekfreeCookTime");
            writer.WriteValue(LastNonSeekfreeCookTime);

            writer.WritePropertyName("FilenameToCookedVersion");
            serializer.Serialize(writer, FilenameToCookedVersion);

            writer.WritePropertyName("CookedTextureFileCacheInfoMap");
            serializer.Serialize(writer, CookedTextureFileCacheInfoMap);
        }
    }
}
