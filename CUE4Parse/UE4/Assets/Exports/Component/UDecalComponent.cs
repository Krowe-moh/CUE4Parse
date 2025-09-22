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
    FOldDecalVertex[] Vertices;

    /** Index buffer. */
    short[] Indices;

    /** Number of decal triangles. */
    int NumTriangles;

    /** Lightmap. */
    FLightMap LightMap1D;

    public FStaticReceiverData(FAssetArchive Ar)
    {
        Component = new FPackageIndex(Ar);
        Vertices = Ar.ReadBulkArray(() => new FOldDecalVertex(Ar));
        Indices = Ar.ReadBulkArray<short>();
        Ar.Read<int>(); // NumTriangles
        if (Ar.Ver > EUnrealEngineObjectUE3Version.VER_DECAL_STATIC_DECALS_SERIALIZED) return;
        FLightMap? lightMap = Ar.Read<ELightMapType>() switch
        {
            ELightMapType.LMT_1D => new FLegacyLightMap1D(Ar),
            ELightMapType.LMT_2D => new FLightMap2D(Ar),
            _ => null
        };
    }
}

public class FOldDecalVertex
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

public class FDecalRenderData
{
    public FDecalRenderData(FAssetArchive Ar)
    {
        if (Ar.Ver < EUnrealEngineObjectUE3Version.VER_DECAL_STATIC_DECALS_SERIALIZED)
        {
            Ar.ReadBulkArray(() => new FOldDecalVertex(Ar));

            Ar.ReadBulkArray<short>();
            if (Ar.Ver < EUnrealEngineObjectUE3Version.VER_RENDERING_REFACTOR)
            {
                Ar.Read<int>(); // LegacySize
                new FRawStaticIndexBuffer(Ar);
                Ar.Read<int>(); // NumTriangles
            }

            if (Ar.Ver == EUnrealEngineObjectUE3Version.VER_DECAL_RENDERDATA)
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

        if (Ar.Ver >= EUnrealEngineObjectUE3Version.VER_DECAL_STATIC_DECALS_SERIALIZED && Ar.Game < EGame.GAME_UE4_0)
        {
            Ar.ReadArray(() => new FStaticReceiverData(Ar));
        }
        else if (Ar.Game < EGame.GAME_UE4_0)
        {
            if (Ar.Ver >= EUnrealEngineObjectUE3Version.VER_DECAL_REFACTOR)
            {
            }
            else if (Ar.Ver == EUnrealEngineObjectUE3Version.VER_DECAL_RENDERDATA)
            {
                Ar.ReadArray(() => new FDecalRenderData(Ar));
            }
            else if (Ar.Ver >= EUnrealEngineObjectUE3Version.VER_DECAL_RENDERDATA_POINTER)
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
