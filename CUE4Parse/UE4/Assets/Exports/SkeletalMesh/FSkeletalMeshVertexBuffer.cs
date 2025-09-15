using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Objects.Engine;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.SkeletalMesh;

[JsonConverter(typeof(FSkeletalMeshVertexBufferConverter))]
public class FSkeletalMeshVertexBuffer
{
    public int NumTexCoords;
    public FVector MeshExtension;
    public FVector MeshOrigin;
    public bool bUseFullPrecisionUVs;
    public bool bUsePackedPosition;
    public bool bExtraBoneInfluences;
    public FGPUVertHalf[] VertsHalf;
    public FGPUVertFloat[] VertsFloat;
    public FGPUVertHalfPacked[] VertsHalfPacked;
    public FGPUVertFloatPacked[] VertsFloatPacked;

    public FSkeletalMeshVertexBuffer()
    {
        VertsHalf = [];
        VertsFloat = [];
        VertsHalfPacked = [];
        VertsFloatPacked = [];
    }

    public FSkeletalMeshVertexBuffer(FArchive Ar, int NumTexCoords) : this()
    {
        var stripDataFlags = new FStripDataFlags(Ar, FPackageFileVersion.CreateUE4Version(EUnrealEngineObjectUE4Version.STATIC_SKELETAL_MESH_SERIALIZATION_FIX));

        if (Ar.Ver < EUnrealEngineObjectUE3Version.VER_USE_FLOAT16_SKELETAL_MESH_UVS)
        {
            Ar.ReadBulkArray(() => new FSoftVertex(Ar));
        }

        if (Ar.Ver >= EUnrealEngineObjectUE3Version.VER_ADDED_MULTIPLE_UVS_TO_SKELETAL_MESH && Ar.Game != EGame.GAME_RocketLeague)
        {
            NumTexCoords = Ar.Read<int>();
        }

        bUseFullPrecisionUVs = Ar.ReadBoolean();

        if (Ar.Ver >= EUnrealEngineObjectUE4Version.SUPPORT_GPUSKINNING_8_BONE_INFLUENCES && FSkeletalMeshCustomVersion.Get(Ar) < FSkeletalMeshCustomVersion.Type.UseSeparateSkinWeightBuffer)
        {
            bExtraBoneInfluences = Ar.ReadBoolean();
        }

        if (Ar.Game >= EGame.GAME_UE4_0) bUsePackedPosition = true;
        if (Ar.Ver >= EUnrealEngineObjectUE3Version.VER_SKELETAL_MESH_SUPPORT_PACKED_POSITION && Ar.Game < EGame.GAME_UE4_0)
        {
            bUsePackedPosition = Ar.ReadBoolean();
            MeshExtension = new FVector(Ar);
            MeshOrigin = new FVector(Ar);
        }

        if (!bUseFullPrecisionUVs)
        {
            if (!bUsePackedPosition)
            {
                VertsHalfPacked = Ar.ReadBulkArray(() => new FGPUVertHalfPacked(Ar, NumTexCoords));
            }
            else
            {
                VertsHalf = Ar.ReadBulkArray(() => new FGPUVertHalf(Ar, bExtraBoneInfluences, NumTexCoords));
            }
        }
        else
        {
            if (bUsePackedPosition)
            {
                VertsFloatPacked = Ar.ReadBulkArray(() => new FGPUVertFloatPacked(Ar, NumTexCoords));
            }
            else
            {
                VertsFloat = Ar.ReadArray(() => new FGPUVertFloat(Ar, bExtraBoneInfluences, NumTexCoords));
            }
        }
    }

    public int GetVertexCount()
    {
        if (VertsHalf.Length > 0) return VertsHalf.Length;
        if (VertsFloat.Length > 0) return VertsFloat.Length;
        return 0;
    }
}