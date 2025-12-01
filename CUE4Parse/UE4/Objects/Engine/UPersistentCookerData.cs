using CUE4Parse.UE4.Assets.Readers;
using System.Collections.Generic;
using CUE4Parse.UE4.Assets.Exports.Texture;
using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Versions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

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
        [JsonConverter(typeof(StringEnumConverter))]
        public EBulkDataFlags SavedBulkDataFlags;
        public int SavedElementCount;
        public int SavedBulkDataOffsetInFile;
        public int SavedBulkDataSizeOnDisk;
        public FName TextureFileCacheName;

        public FCookedBulkDataInfo(FAssetArchive Ar)
        {
            SavedBulkDataFlags = Ar.Read<EBulkDataFlags>();
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

    public class FCookedTextureUsageInfo
    {
        public string[] PackageNames;
        [JsonConverter(typeof(StringEnumConverter))]
        public EPixelFormat Format;
        [JsonConverter(typeof(StringEnumConverter))]
        public TextureGroup LODGroup;
        public int SizeX;
        public int SizeY;
        public int StoredOnceMipSize;
        public int DuplicatedMipSize;

        public FCookedTextureUsageInfo(FAssetArchive Ar)
        {
            PackageNames = Ar.ReadArray(() => Ar.ReadFString());
            Format = Ar.Read<EPixelFormat>();
            LODGroup = (TextureGroup)Ar.Read<byte>();
            SizeX = Ar.Read<int>();
            SizeY = Ar.Read<int>();
            StoredOnceMipSize = Ar.Read<int>();
            DuplicatedMipSize = Ar.Read<int>();
        }
    }

    public class FForceCookedInfo
    {
        public Dictionary<string, bool> CookedContentList;

        public FForceCookedInfo(FAssetArchive Ar)
        {
            CookedContentList = Ar.ReadMap(() => Ar.ReadFString(), () => Ar.ReadBoolean());
        }
    }

    public class FCookStats
    {
        public double TotalTime;
        public double CreateIniFilesTime;
        public double LoadSectionPackagesTime;
        public double LoadNativePackagesTime;
        public double LoadDependenciesTime;
        public double LoadPackagesTime;
        public double LoadPerMapPackagesTime;
        public double LoadCombinedStartupPackagesTime;
        public double CheckDependentPackagesTime;
        public double ExternGShaderCacheLoadTime;
        public double SaveShaderCacheTime;
        public double CopyShaderCacheTime;
        public double ExternGRHIShaderCompileTime_Total;
        public double ExternGRHIShaderCompileTime_PS3;
        public double ExternGRHIShaderCompileTime_XBOXD3D;
        public double ExternGRHIShaderCompileTime_NGP;
        public double ExternGRHIShaderCompileTime_PCD3D_SM3;
        public double ExternGRHIShaderCompileTime_PCD3D_SM5;
        public double ExternGRHIShaderCompileTime_PCOGL;

        public double CleanupMaterialsTime;
        public double CookTime;
        public double CookPhysicsTime;
        public double CookTextureTime;
        public double CookSoundTime;
        public double CookSoundCueTime;
        public double LocSoundTime;
        public double CookMovieTime;
        public double CookStripTime;
        public double CookSkeletalMeshTime;
        public double CookStaticMeshTime;
        public double CookParticleSystemTime;
        public double CookParticleSystemDuplicateRemovalTime;
        public double CookLandscapeTime;

        public double PackageSaveTime;
        public double PackageLocTime;

        public double PrepareForSavingTime;
        public double PrepareForSavingTextureTime;
        public double PrepareForSavingTerrainTime;
        public double PrepareForSavingMaterialTime;
        public double PrepareForSavingMaterialInstanceTime;
        public double PrepareForSavingStaticMeshTime;

        public double CollectGarbageAndVerifyTime;

        public double PrePackageIterationTime;
        public double PackageIterationTime;
        public double PrepPackageTime;
        public double InitializePackageTime;
        public double FinalizePackageTime;
        public double PostPackageIterationTime;

        public double PersistentFaceFXTime;
        public double PersistentFaceFXDeterminationTime;
        public double PersistentFaceFXGenerationTime;

        public ulong TFCTextureAlreadySaved;
        public ulong TFCTextureSaved;

        public double ExternGCompressorTime;
        public ulong ExternGCompressorSrcBytes;
        public ulong ExternGCompressorDstBytes;
        public double ExternGArchiveSerializedCompressedSavingTime;

        public FCookStats(FAssetArchive Ar)
        {
            TotalTime = Ar.Read<double>();
            CreateIniFilesTime = Ar.Read<double>();
            LoadSectionPackagesTime = Ar.Read<double>();
            LoadNativePackagesTime = Ar.Read<double>();
            LoadDependenciesTime = Ar.Read<double>();
            LoadPackagesTime = Ar.Read<double>();
            LoadPerMapPackagesTime = Ar.Read<double>();
            LoadCombinedStartupPackagesTime = Ar.Read<double>();
            CheckDependentPackagesTime = Ar.Read<double>();
            ExternGShaderCacheLoadTime = Ar.Read<double>();
            SaveShaderCacheTime = Ar.Read<double>();
            CopyShaderCacheTime = Ar.Read<double>();
            ExternGRHIShaderCompileTime_Total = Ar.Read<double>();
            ExternGRHIShaderCompileTime_PS3 = Ar.Read<double>();
            ExternGRHIShaderCompileTime_XBOXD3D = Ar.Read<double>();
            ExternGRHIShaderCompileTime_NGP = Ar.Read<double>();
            ExternGRHIShaderCompileTime_PCD3D_SM3 = Ar.Read<double>();
            ExternGRHIShaderCompileTime_PCD3D_SM5 = Ar.Read<double>();
            ExternGRHIShaderCompileTime_PCOGL = Ar.Read<double>();

            CleanupMaterialsTime = Ar.Read<double>();
            CookTime = Ar.Read<double>();
            CookPhysicsTime = Ar.Read<double>();
            CookTextureTime = Ar.Read<double>();
            CookSoundTime = Ar.Read<double>();
            CookSoundCueTime = Ar.Read<double>();
            LocSoundTime = Ar.Read<double>();
            CookMovieTime = Ar.Read<double>();
            CookStripTime = Ar.Read<double>();
            CookSkeletalMeshTime = Ar.Read<double>();
            CookStaticMeshTime = Ar.Read<double>();
            CookParticleSystemTime = Ar.Read<double>();
            CookParticleSystemDuplicateRemovalTime = Ar.Read<double>();
            CookLandscapeTime = Ar.Read<double>();

            PackageSaveTime = Ar.Read<double>();
            PackageLocTime = Ar.Read<double>();

            PrepareForSavingTime = Ar.Read<double>();
            PrepareForSavingTextureTime = Ar.Read<double>();
            PrepareForSavingTerrainTime = Ar.Read<double>();
            PrepareForSavingMaterialTime = Ar.Read<double>();
            PrepareForSavingMaterialInstanceTime = Ar.Read<double>();
            PrepareForSavingStaticMeshTime = Ar.Read<double>();

            CollectGarbageAndVerifyTime = Ar.Read<double>();

            PrePackageIterationTime = Ar.Read<double>();
            PackageIterationTime = Ar.Read<double>();
            PrepPackageTime = Ar.Read<double>();
            InitializePackageTime = Ar.Read<double>();
            FinalizePackageTime = Ar.Read<double>();
            PostPackageIterationTime = Ar.Read<double>();

            PersistentFaceFXTime = Ar.Read<double>();
            PersistentFaceFXDeterminationTime = Ar.Read<double>();
            PersistentFaceFXGenerationTime = Ar.Read<double>();

            TFCTextureAlreadySaved = Ar.Read<ulong>();
            TFCTextureSaved = Ar.Read<ulong>();

            ExternGCompressorTime = Ar.Read<double>();
            ExternGCompressorSrcBytes = Ar.Read<ulong>();
            ExternGCompressorDstBytes = Ar.Read<ulong>();
            ExternGArchiveSerializedCompressedSavingTime = Ar.Read<double>();
        }
    }

    public class UPersistentCookerData : Assets.Exports.UObject
    {
        public Dictionary<string, FPackageTreeEntry[]> ClassMap;
        public Dictionary<string, Dictionary<string, FPackageTreeEntry[]>> LocalizationMap;
        public Dictionary<string, FCookedBulkDataInfo> CookedBulkDataInfoMap;
        public Dictionary<string, FCookedTextureFileCacheInfo> CookedTextureFileCacheInfoMap;
        public Dictionary<string, FCookedTextureUsageInfo> TextureUsageInfos;
        public Dictionary<string, FForceCookedInfo> CookedPrefixCommonInfoMap;
        public Dictionary<string, FForceCookedInfo> PMapForcedObjectsMap;
        public Dictionary<string, FSHAHash> FilenameToScriptSHA;
        public Dictionary<string, double> FilenameToTimeMap;
        public Dictionary<string, int> FilenameToCookedVersion;
        public FCookStats CookStats;
        public string[] ChildCookWarnings;
        public string[] ChildCookErrors;
        public long TextureFileCacheWaste;
        public double LastNonSeekfreeCookTime;

        public override void Deserialize(FAssetArchive Ar, long validPos)
        {
            base.Deserialize(Ar, validPos);

            if (Ar.Ver > EUnrealEngineObjectUE3Version.VER_RECALCULATE_MAXACTIVEPARTICLE)
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
            FilenameToCookedVersion = Ar.ReadMap(() => Ar.ReadFString(), () => Ar.Read<int>());
            if(Ar.Ver >= EUnrealEngineObjectUE3Version.VER_ADDED_TEXTURE_FILECACHE_GUIDS)
            {
                CookedTextureFileCacheInfoMap = Ar.ReadMap(() => Ar.ReadFString(), () => new FCookedTextureFileCacheInfo(Ar));
                TextureUsageInfos = Ar.ReadMap(() => Ar.ReadFString(), () => new FCookedTextureUsageInfo(Ar));
                CookedPrefixCommonInfoMap = Ar.ReadMap(() => Ar.ReadFString(), () => new FForceCookedInfo(Ar));
                PMapForcedObjectsMap = Ar.ReadMap(() => Ar.ReadFString(), () => new FForceCookedInfo(Ar));
                FilenameToScriptSHA = Ar.ReadMap(() => Ar.ReadFString(), () => new FSHAHash(Ar));
                ChildCookWarnings = Ar.ReadArray(() => Ar.ReadFString());
                ChildCookErrors = Ar.ReadArray(() => Ar.ReadFString());
                //CookStats = new FCookStats(Ar);
            }
        }

        protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer)
        {
            base.WriteJson(writer, serializer);

            if (ClassMap != null)
            {
                writer.WritePropertyName("ClassMap");
                serializer.Serialize(writer, ClassMap);
            }

            if (LocalizationMap != null)
            {
                writer.WritePropertyName("LocalizationMap");
                serializer.Serialize(writer, LocalizationMap);
            }

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

            if (CookedTextureFileCacheInfoMap != null)
            {
                writer.WritePropertyName("CookedTextureFileCacheInfoMap");
                serializer.Serialize(writer, CookedTextureFileCacheInfoMap);
            }

            if (TextureUsageInfos != null)
            {
                writer.WritePropertyName("TextureUsageInfos");
                serializer.Serialize(writer, TextureUsageInfos);
            }

            if (CookedPrefixCommonInfoMap != null)
            {
                writer.WritePropertyName("CookedPrefixCommonInfoMap");
                serializer.Serialize(writer, CookedPrefixCommonInfoMap);
            }

            if (PMapForcedObjectsMap != null)
            {
                writer.WritePropertyName("PMapForcedObjectsMap");
                serializer.Serialize(writer, PMapForcedObjectsMap);
            }

            if (FilenameToScriptSHA != null)
            {
                writer.WritePropertyName("FilenameToScriptSHA");
                serializer.Serialize(writer, FilenameToScriptSHA);
            }

            if (ChildCookWarnings != null)
            {
                writer.WritePropertyName("ChildCookWarnings");
                serializer.Serialize(writer, ChildCookWarnings);
            }

            if (ChildCookErrors != null)
            {
                writer.WritePropertyName("ChildCookErrors");
                serializer.Serialize(writer, ChildCookErrors);
            }

            if (CookStats != null)
            {
                writer.WritePropertyName("CookStats");
                serializer.Serialize(writer, CookStats);
            }
        }
    }
}
