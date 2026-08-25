using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Objects.Meshes;
using CUE4Parse.UE4.Objects.RenderCore;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.StaticMesh;

[JsonConverter(typeof(FStaticMeshUVItemConverter))]
public partial class FStaticMeshUVItem
{
    public readonly FPackedNormal[] Normal;
    public readonly FMeshUVFloat[] UV;
    public readonly FVector Position;
    public readonly FColor Color;

    public FStaticMeshUVItem(FArchive Ar, bool useHighPrecisionTangents, int numStaticUVSets, bool useStaticFloatUVs)
    {
        if (Ar.Ver < EUnrealEngineObjectUE3Version.MovedColorFromUVItem)
        {
            if (Ar.Ver < EUnrealEngineObjectUE3Version.MovedColorFromUVItem)
            {
                Position = Ar.Read<FVector>();
                Color = Ar.Read<FColor>();
            }
            Normal = SerializeTangents(Ar, useHighPrecisionTangents);
            if (Ar.Game == GAME_APBReloaded)
            {
                goto SkipColor;
            }
            if (Ar.Ver >= EUnrealEngineObjectUE3Version.STATICMESH_VERTEXCOLOR && Ar.Ver < EUnrealEngineObjectUE3Version.MESH_PAINT_SYSTEM)
            {
                Color = Ar.Read<FColor>();
            }
            SkipColor:
            UV = SerializeTexcoords(Ar, numStaticUVSets, useStaticFloatUVs);
        }

        var uvFloat = new FMeshUVFloat[numStaticUVSets];
        for (var i = 0; i < numStaticUVSets; i++)
        {
            uvFloat[i] = (FMeshUVFloat) Ar.Read<FMeshUVHalf>();
        }
        return uvFloat;
    }
}
