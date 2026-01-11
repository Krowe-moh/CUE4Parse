using CUE4Parse.UE4.Assets.Exports.BuildData;
using CUE4Parse.UE4.Assets.Exports.Component;
using CUE4Parse.UE4.Assets.Exports.Material;
using CUE4Parse.UE4.Assets.Exports.StaticMesh;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Objects.RenderCore;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Assets.Exports.Component;

public class FStaticReceiverData
{
    /** The receiving component. */
    FPackageIndex Component;

    /** Source vertex data. */
    IUStruct[] Vertices;

    /** Index buffer. */
    short[] Indices;

    /** Number of decal triangles. */
    int NumTriangles;

    /** Lightmap. */
    FLightMap LightMap1D;

    public FStaticReceiverData(FAssetArchive Ar)
    {
        Component = new FPackageIndex(Ar);
        if (Ar.Ver < EUnrealEngineObjectUE3Version.DECAL_ADDED_DECAL_VERTEX_LIGHTMAP_COORD)
        {
            Vertices = Ar.ReadBulkArray(() => new FOldDecalVertex(Ar));
        }
        else
        {
            Vertices = Ar.ReadBulkArray(() => new FDecalVertex(Ar));
        }
        Indices = Ar.ReadBulkArray<short>();
        Ar.Read<int>(); // NumTriangles
        if (Ar.Ver > EUnrealEngineObjectUE3Version.DECAL_STATIC_DECALS_SERIALIZED) return;
        FLightMap? lightMap = Ar.Read<ELightMapType>() switch
        {
            ELightMapType.LMT_1D => new FLegacyLightMap1D(Ar),
            ELightMapType.LMT_2D => new FLightMap2D(Ar),
            _ => null
        };

        if (Ar.Ver >= EUnrealEngineObjectUE3Version.DECAL_SHADOWMAPS)
        {
            Ar.ReadArray(() => new FPackageIndex(Ar)); // ShadowMap1D;
        }

        if (Ar.Ver >= EUnrealEngineObjectUE3Version.DECAL_SERIALIZE_BSP_ELEMENT)
        {
            Ar.Read<int>(); // Data
        }

        if( Ar.Ver >= EUnrealEngineObjectUE3Version.STATIC_DECAL_INSTANCE_INDEX)
        {
            Ar.Read<int>(); // InstanceIndex
        }
    }
}

public class FOldDecalVertex : IUStruct
{
    FVector Position;
    FPackedNormal TangentX;
    FPackedNormal TangentY;
    FPackedNormal TangentZ;

    /** Decal mesh texture coordinates. */
    FVector2D UV;

    /** Transforms receiver tangent basis into decal tangent basis for normal map lookup. */
    FVector2D NormalTransform0;
    FVector2D NormalTransform1;

    public FOldDecalVertex(FArchive Ar)
    {
        Position = Ar.Read<FVector>();
        TangentX = new FPackedNormal(Ar);
        TangentY = new FPackedNormal(Ar);
        TangentZ = new FPackedNormal(Ar);
        UV = Ar.Read<FVector2D>();
        NormalTransform0 = new FVector2D(Ar);
        NormalTransform1 = new FVector2D(Ar);
    }
}

public class FDecalVertex : IUStruct
{
    FVector Position;
    FPackedNormal TangentX;
    FPackedNormal? TangentY;
    FPackedNormal TangentZ;

    /** Decal mesh texture coordinates. */
    FVector2D UV;

    /** Decal vertex light map coordinated.  Added in engine version VER_DECAL_ADDED_DECAL_VERTEX_LIGHTMAP_COORD. */
    FVector2D		LightMapCoordinate;

    /** Transforms receiver tangent basis into decal tangent basis for normal map lookup. */
    FVector2D NormalTransform0;
    FVector2D NormalTransform1;

    public FDecalVertex(FArchive Ar)
    {
        Position = Ar.Read<FVector>();

        if (Ar.Ver >= EUnrealEngineObjectUE3Version.DECAL_VERTEX_FACTORY_VER1 && Ar.Ver < EUnrealEngineObjectUE3Version.DECAL_VERTEX_FACTORY_VER2)
        {
        }
        else
        {
            TangentX = new FPackedNormal(Ar);
        }

        if (false) // Ar.Ver < EUnrealEngineObjectUE3Version.REMOVE_BINORMAL_TANGENT_VECTOR
        {
            TangentY = new FPackedNormal(Ar);
        }
        TangentZ = new FPackedNormal(Ar);
        if (Ar.Ver < EUnrealEngineObjectUE3Version.DECAL_VERTEX_FACTORY_VER1)
        {
            UV = new FVector2D(Ar);
        }

        if (Ar.Ver >= EUnrealEngineObjectUE3Version.DECAL_ADDED_DECAL_VERTEX_LIGHTMAP_COORD)
        {
            LightMapCoordinate = new FVector2D(Ar);
        }

        if (Ar.Ver < EUnrealEngineObjectUE3Version.DECAL_REMOVED_2X2_NORMAL_TRANSFORM)
        {
            NormalTransform0 = new FVector2D(Ar);
            NormalTransform1 = new FVector2D(Ar);
        }
    }
}

public class FDecalRenderData
{
    public FDecalRenderData(FAssetArchive Ar)
    {
        if (Ar.Ver < EUnrealEngineObjectUE3Version.DECAL_STATIC_DECALS_SERIALIZED)
        {
            Ar.ReadBulkArray(() => new FOldDecalVertex(Ar));

            Ar.ReadBulkArray<short>();
            if (Ar.Ver < EUnrealEngineObjectUE3Version.RENDERING_REFACTOR)
            {
                Ar.Read<int>(); // LegacySize
                new FRawStaticIndexBuffer(Ar);
                Ar.Read<int>(); // NumTriangles
            }

            if (Ar.Ver == EUnrealEngineObjectUE3Version.DECAL_RENDERDATA)
            {
                new FPackageIndex(Ar); // LightMap
            }
        }
    }
}

public class UDecalComponent : USceneComponent
{
    public FPackageIndex? DecalMaterial;
    public FVector? DecalSize;

    public override void Deserialize(FAssetArchive Ar, long validPos)
    {
        base.Deserialize(Ar, validPos);

        if (Ar.Ver < EUnrealEngineObjectUE4Version.DECAL_SIZE)
        {
            DecalSize = FVector.OneVector;
        }

        DecalMaterial = GetOrDefault<FPackageIndex>(nameof(DecalMaterial), new FPackageIndex());
        DecalSize = GetOrDefault<FVector>(nameof(DecalSize));

        if (Ar.Ver >= EUnrealEngineObjectUE3Version.DECAL_STATIC_DECALS_SERIALIZED && Ar.Game < EGame.GAME_UE4_0)
        {
            Ar.ReadArray(() => new FStaticReceiverData(Ar));
        }
        else if (Ar.Game < EGame.GAME_UE4_0)
        {
            if (Ar.Ver >= EUnrealEngineObjectUE3Version.DECAL_REFACTOR)
            {
            }
            else if (Ar.Ver == EUnrealEngineObjectUE3Version.DECAL_RENDERDATA)
            {
                Ar.ReadArray(() => new FDecalRenderData(Ar));
            }
            else if (Ar.Ver >= EUnrealEngineObjectUE3Version.DECAL_RENDERDATA_POINTER)
            {
                Ar.ReadArray(() => new FDecalRenderData(Ar));
            }
        }
    }

    public UMaterialInterface? GetDecalMaterial()
    {
        if (DecalMaterial == null) return null;
        return DecalMaterial?.Load<UMaterialInterface>();
    }
}
