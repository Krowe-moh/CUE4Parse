using System.Collections.Generic;
using System.Runtime.InteropServices;
using CommunityToolkit.HighPerformance;
using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Versions;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.Engine;

[StructLayout(LayoutKind.Sequential)]
public readonly struct FPrecomputedVisibilityCell : IUStruct
{
    public readonly FVector Min;
    public readonly ushort ChunkIndex;
    public readonly ushort DataOffset;

    public FPrecomputedVisibilityCell(FAssetArchive Ar)
    {
        Min = new FVector(Ar);
        ChunkIndex = Ar.Read<ushort>();
        DataOffset = Ar.Read<ushort>();
    }
}

[JsonConverter(typeof(FCompressedVisibilityChunkConverter))]
public readonly struct FCompressedVisibilityChunk : IUStruct
{
    public readonly bool bCompressed;
    public readonly int UncompressedSize;
    public readonly byte[] Data;

    public FCompressedVisibilityChunk(FAssetArchive Ar)
    {
        bCompressed = Ar.ReadBoolean();
        UncompressedSize = Ar.Read<int>();
        Data = Ar.ReadArray<byte>();
    }
}

public readonly struct FPrecomputedVisibilityBucket : IUStruct
{
    public readonly int CellDataSize;
    public readonly FPrecomputedVisibilityCell[] Cells;
    public readonly FCompressedVisibilityChunk[] CellDataChunks;

    public FPrecomputedVisibilityBucket(FAssetArchive Ar)
    {
        CellDataSize = Ar.Read<int>();
        Cells = Ar.ReadArray(() => new FPrecomputedVisibilityCell(Ar));
        CellDataChunks = Ar.ReadArray(() => new FCompressedVisibilityChunk(Ar));
    }
}

[StructLayout(LayoutKind.Sequential)]
public readonly struct FPrecomputedVisibilityHandler : IUStruct
{
    public readonly FVector2D PrecomputedVisibilityCellBucketOriginXY;
    public readonly float PrecomputedVisibilityCellSizeXY;
    public readonly float PrecomputedVisibilityCellSizeZ;
    public readonly int PrecomputedVisibilityCellBucketSizeXY;
    public readonly int PrecomputedVisibilityNumCellBuckets;
    public readonly FPrecomputedVisibilityBucket[] PrecomputedVisibilityCellBuckets;

    public FPrecomputedVisibilityHandler(FAssetArchive Ar)
    {
        PrecomputedVisibilityCellBucketOriginXY = new FVector2D(Ar);
        PrecomputedVisibilityCellSizeXY = Ar.Read<float>();
        PrecomputedVisibilityCellSizeZ = Ar.Read<float>();
        PrecomputedVisibilityCellBucketSizeXY = Ar.Read<int>();
        PrecomputedVisibilityNumCellBuckets = Ar.Read<int>();
        //PrecomputedVisibilityCellBuckets = Ar.ReadArray(() => new FPrecomputedVisibilityBucket(Ar));
    }
}

public readonly struct FPrecomputedVolumeDistanceField : IUStruct
{
    public readonly float VolumeMaxDistance;
    public readonly FBox VolumeBox;
    public readonly int VolumeSizeX;
    public readonly int VolumeSizeY;
    public readonly int VolumeSizeZ;
    public readonly FColor[] Data;

    public FPrecomputedVolumeDistanceField(FAssetArchive Ar)
    {
        VolumeMaxDistance = Ar.Read<float>();
        VolumeBox = new FBox(Ar);
        VolumeSizeX = Ar.Read<int>();
        VolumeSizeY = Ar.Read<int>();
        VolumeSizeZ = Ar.Read<int>();
        Data = Ar.ReadArray<FColor>();
    }
}

[StructLayout(LayoutKind.Sequential)]
public readonly struct FStreamableTextureInstance : IUStruct
{
    public readonly FSphere BoundingSphere;
    public readonly float TexelFactor;

    public FStreamableTextureInstance(FAssetArchive Ar)
    {
        BoundingSphere = new FSphere(Ar);
        TexelFactor = Ar.Read<float>();
    }
}

[StructLayout(LayoutKind.Sequential)]
public readonly struct FDynamicTextureInstance : IUStruct
{
    public readonly FSphere BoundingSphere;
    public readonly float TexelFactor;
    public readonly FPackageIndex Texture;
    public readonly bool bAttached;
    public readonly float OriginalRadius;

    public FDynamicTextureInstance(FAssetArchive Ar)
    {
        BoundingSphere = new FSphere(Ar);
        TexelFactor = Ar.Read<float>();
        Texture = new FPackageIndex(Ar);
        bAttached = Ar.ReadBoolean();
        OriginalRadius = Ar.Read<float>();
    }
}

[StructLayout(LayoutKind.Sequential)]
public readonly struct FCachedPhysSMData : IUStruct
{
    public readonly FVector Scale3D;
    public readonly int CachedDataIndex;

    public FCachedPhysSMData(FAssetArchive Ar)
    {
        Scale3D = new FVector(Ar);
        CachedDataIndex = Ar.Read<int>();
    }
}

[StructLayout(LayoutKind.Sequential)]
public readonly struct FCachedPerTriPhysSMData : IUStruct
{
    public readonly FVector Scale3D;
    public readonly int CachedDataIndex;

    public FCachedPerTriPhysSMData(FAssetArchive Ar)
    {
        Scale3D = new FVector(Ar);
        CachedDataIndex = Ar.Read<int>();
    }
}
public class FStreamableTextureInfo
{
    public FPackageIndex Texture;
    public FStreamableTextureInstance[] TextureInstances;

    public FStreamableTextureInfo(FAssetArchive Ar)
    {
        Texture = new FPackageIndex(Ar);
        TextureInstances = Ar.ReadArray(() => new FStreamableTextureInstance(Ar));
    }
}

public class FStreamableSoundInstance
{
    public FSphere BoundingSphere;

    public FStreamableSoundInstance(FAssetArchive Ar)
    {
        BoundingSphere = Ar.Read<FSphere>();
    }
}

public class FStreamableSoundInfo
{
    public FPackageIndex SoundNodeWave;
    public FStreamableSoundInstance[] SoundInstances;

    public FStreamableSoundInfo(FAssetArchive Ar)
    {
        SoundNodeWave = new FPackageIndex(Ar);
        SoundInstances = Ar.ReadArray(() => new FStreamableSoundInstance(Ar));
    }
}

public class ULevel : Assets.Exports.UObject
{
    public FPackageIndex WorldSettings;
    public FPackageIndex WorldDataLayers;
    public FSoftObjectPath WorldPartitionRuntimeCell;
    
    public FPackageIndex?[] Actors;
    public FURL URL;
    public FPackageIndex Model;
    public FPackageIndex[] ModelComponents;
    public FPackageIndex LevelScriptActor;
    public FPackageIndex? NavListStart;
    public FPackageIndex? NavListEnd;
    public FPrecomputedVisibilityHandler? PrecomputedVisibilityHandler;
    public FPrecomputedVolumeDistanceField? PrecomputedVolumeDistanceField;
    public FPackageIndex[] GameSequences;

    public Dictionary<FPackageIndex, FStreamableTextureInstance[]> TextureToInstancesMap; // UTexture2D* -> FStreamableTextureInstance[]
    public Dictionary<FPackageIndex, FDynamicTextureInstance[]> DynamicTextureInstances; // UPrimitiveComponent* -> FDynamicTextureInstance[]
    public Dictionary<FPackageIndex, bool> ForceStreamTextures;

    public FPackageIndex? CoverListStart;
    public FPackageIndex? CoverListEnd;
    public FPackageIndex? PylonListStart;
    public FPackageIndex? PylonListEnd;

    public FPackageIndex[] CrossLevelActors;

    public override void Deserialize(FAssetArchive Ar, long validPos)
    {
        base.Deserialize(Ar, validPos);
        WorldSettings = GetOrDefault(nameof(WorldSettings), new FPackageIndex());
        WorldDataLayers = GetOrDefault(nameof(WorldDataLayers), new FPackageIndex());
        WorldPartitionRuntimeCell = GetOrDefault<FSoftObjectPath>(nameof(WorldPartitionRuntimeCell));
        
        if (Ar.Game == EGame.GAME_WorldofJadeDynasty) Ar.Position += 16;
        if (Flags.HasFlag(EObjectFlags.RF_ClassDefaultObject) || Ar.Position >= validPos) return;
        if (FReleaseObjectVersion.Get(Ar) < FReleaseObjectVersion.Type.LevelTransArrayConvertedToTArray) Ar.Position += 4;
        Actors = Ar.ReadArray(() => new FPackageIndex(Ar));
        URL = new FURL(Ar);
        Model = new FPackageIndex(Ar);
        ModelComponents = Ar.ReadArray(() => new FPackageIndex(Ar));
        // if ue3
        Ar.ReadArray(() => new FPackageIndex(Ar)); // GameSequences

        if (Ar.Ver < EUnrealEngineObjectUE3Version.VER_SPLIT_SOUND_FROM_TEXTURE_STREAMING)
        {
            Ar.ReadArray(() => new FStreamableTextureInfo(Ar)); // ResourceInfos (FStreamableResourceInstance but same struct as FStreamableTextureInfo)
        }
        else
        {
            if (Ar.Ver < EUnrealEngineObjectUE3Version.VER_RENDERING_REFACTOR)
            {
                Ar.ReadArray(() => new FStreamableTextureInfo(Ar)); // TextureInfos
            }
            else
            {
                Ar.ReadMap(() => new FPackageIndex(Ar), () => Ar.ReadArray(() => new FStreamableTextureInstance(Ar))); // TextureToInstancesMap
            }

            if (Ar.Ver < EUnrealEngineObjectUE3Version.VER_RENDERING_REFACTOR)
            {
                Ar.ReadArray(() => new FStreamableSoundInfo(Ar)); // SoundInfos
            }
        }

        if (Ar.Ver >= EUnrealEngineObjectUE3Version.VER_DYNAMICTEXTUREINSTANCES)
        {
            Ar.ReadMap(() => new FPackageIndex(Ar), () => Ar.ReadArray(() => new FDynamicTextureInstance(Ar))); // DynamicTextureInstances
        }

        if (Ar.Ver >= EUnrealEngineObjectUE3Version.VER_APEX_DESTRUCTION)
        {
            var size = Ar.Read<int>();
            Ar.Position += size;
        }

        if (Ar.Ver >= EUnrealEngineObjectUE3Version.VER_PRECOOK_PHYS_BSP_TERRAIN)
        {
            Ar.ReadBulkArray<byte>(); // CachedPhysBSPData
        }

        if (Ar.Ver >= EUnrealEngineObjectUE3Version.VER_PRECOOK_PHYS_STATICMESH_CACHE)
        {
            Ar.ReadMap(() => new FPackageIndex(Ar), () => new FCachedPhysSMData(Ar)); // CachedPhysSMDataMap
            Ar.ReadArray(() => Ar.ReadArray(() => Ar.ReadBulkArray<byte>())); // CachedPhysSMDataStore
        }

        if (Ar.Ver >= EUnrealEngineObjectUE3Version.VER_PRECOOK_PERTRI_PHYS_STATICMESH)
        {
            Ar.ReadMap(() => new FPackageIndex(Ar), () => new FCachedPhysSMData(Ar)); // CachedPhysSMDataMap
            Ar.ReadArray(() => Ar.ReadBulkArray<byte>()); // CachedPhysPerTriSMDataStore
        }

        if (Ar.Ver >= EUnrealEngineObjectUE3Version.VER_SAVE_PRECOOK_PHYS_VERSION)
        {
            Ar.Read<int>(); // CachedPhysBSPDataVersion
            Ar.Read<int>(); // CachedPhysSMDataVersion
        }

        if (Ar.Ver >= EUnrealEngineObjectUE3Version.VER_LEVEL_FORCE_STREAM_TEXTURES)
        {
            Ar.ReadMap(() => new FPackageIndex(Ar), () => Ar.ReadBoolean()); // ForceStreamTextures
        }

        if (Ar.Ver > EUnrealEngineObjectUE3Version.VER_CONVEX_BSP)
        {
            Ar.ReadArray(() => Ar.ReadBulkArray<byte>());
            Ar.Read<int>();
        }

        if (Ar.Ver >= EUnrealEngineObjectUE3Version.VER_PERLEVEL_NAVLIST)
        {
            NavListStart = new FPackageIndex(Ar);
            NavListEnd = new FPackageIndex(Ar);
            new FPackageIndex(Ar); // CoverListStart
            new FPackageIndex(Ar); // CoverListEnd
            if (Ar.Ver >= EUnrealEngineObjectUE3Version.VER_PYLONLIST_IN_ULEVEL)
            {
                new FPackageIndex(Ar);
                new FPackageIndex(Ar);
            }

            if (Ar.Ver >= EUnrealEngineObjectUE3Version.VER_COVERGUIDREFS_IN_ULEVEL)
            {
                Ar.Read<int>();
                Ar.Read<int>();
                Ar.Read<int>();
            }

            Ar.Read<int>();
            if (Ar.Ver >= EUnrealEngineObjectUE3Version.VER_GI_CHARACTER_LIGHTING)
            {
                new FPackageIndex(Ar);
            }

            new FVector2D(Ar);
            Ar.Read<float>();
            Ar.Read<float>();
            Ar.Read<int>();
            Ar.Read<int>();
            Ar.ReadArray(() => new FPrecomputedVisibilityHandler(Ar));
        }
        return;
        LevelScriptActor = new FPackageIndex(Ar);
        if (FRenderingObjectVersion.Get(Ar) < FRenderingObjectVersion.Type.RemovedTextureStreamingLevelData) return;
        NavListStart = new FPackageIndex(Ar);
        NavListEnd = new FPackageIndex(Ar);
        if (Ar.Game == EGame.GAME_MetroAwakening && GetOrDefault<bool>("bIsLightingScenario")) return;
        if (Ar.Game is EGame.GAME_StateOfDecay2 or EGame.GAME_WeHappyFew && Ar.ReadBoolean()) return;
        if (Ar.Game == EGame.GAME_OutlastTrials)
        {
            PrecomputedVolumeDistanceField = new FPrecomputedVolumeDistanceField(Ar);
            return;
        }
        PrecomputedVisibilityHandler = new FPrecomputedVisibilityHandler(Ar);
        PrecomputedVolumeDistanceField = new FPrecomputedVolumeDistanceField(Ar);
    }

    protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer)
    {
        base.WriteJson(writer, serializer);

        writer.WritePropertyName("Actors");
        serializer.Serialize(writer, Actors);

        writer.WritePropertyName("URL");
        serializer.Serialize(writer, URL);

        writer.WritePropertyName("Model");
        serializer.Serialize(writer, Model);

        writer.WritePropertyName("ModelComponents");
        serializer.Serialize(writer, ModelComponents);

        writer.WritePropertyName("LevelScriptActor");
        serializer.Serialize(writer, LevelScriptActor);

        writer.WritePropertyName("NavListStart");
        serializer.Serialize(writer, NavListStart);

        writer.WritePropertyName("NavListEnd");
        serializer.Serialize(writer, NavListEnd);

        if (PrecomputedVisibilityHandler == null) return;

        writer.WritePropertyName("PrecomputedVisibilityHandler");
        serializer.Serialize(writer, PrecomputedVisibilityHandler);

        writer.WritePropertyName("PrecomputedVolumeDistanceField");
        serializer.Serialize(writer, PrecomputedVolumeDistanceField);
    }
}
