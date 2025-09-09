using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Objects.Meshes;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Assets.Exports.SkeletalMesh;

public class FSoftVertex : FSkelMeshVertexBase
{
    private const int MAX_SKELETAL_UV_SETS_UE4 = 4;
    private const int MAX_INFLUENCES_UE3 = 4;
    private int MAX_SKELETAL_UV_SETS_UE3 = 1;

    public FMeshUVFloat[] UV;
    public FColor Color;

    public FSoftVertex(FArchive Ar, bool isRigid = false)
    {
        SerializeForEditor(Ar);

        if (Ar.Ver >= EUnrealEngineObjectUE3Version.VER_ADDED_MULTIPLE_UVS_TO_SKELETAL_MESH) MAX_SKELETAL_UV_SETS_UE3 = 4;
        UV = new FMeshUVFloat[MAX_SKELETAL_UV_SETS_UE4];
        for (var i = 0; i < UV.Length; i++)
            UV[i] = Ar.Read<FMeshUVFloat>();

        if (Ar.Ver >= EUnrealEngineObjectUE3Version.VER_ADDED_SKELETAL_MESH_VERTEX_COLORS)
        {
            Color = Ar.Read<FColor>();
        }

        if (isRigid)
        {
            if (Ar.Game < EGame.GAME_UE4_0)
            {
                if (Ar.Ver >= EUnrealEngineObjectUE3Version.VER_IPHONE_AUDIO_VARIABLE_BLOCK_SIZE_COMPRESSION) Ar.Read<int>(); // BoneIndex

                return;
            }
        }
        else
        {
            if (Ar.Ver >= EUnrealEngineObjectUE3Version.VER_USE_UMA_RESOURCE_ARRAY_MESH_DATA)
            {
                Ar.ReadArray<byte>(MAX_INFLUENCES_UE3); // BoneIndex
                Ar.ReadArray<byte>(MAX_INFLUENCES_UE3); // BoneWeight
            }
            else
            {
                Ar.ReadArray<byte>(MAX_INFLUENCES_UE3); // BoneWeight
            }

            return;
        }
        
        Infs = !isRigid ?
            new FSkinWeightInfo(Ar, Ar.Ver >= EUnrealEngineObjectUE4Version.SUPPORT_8_BONE_INFLUENCES_SKELETAL_MESHES) :
            new FSkinWeightInfo { BoneIndex = { [0] = Ar.Read<byte>() }, BoneWeight = { [0] = 255 } };
    }
}

public class FRigidVertex : FSoftVertex
{
    public FRigidVertex(FArchive Ar) : base(Ar, true) { }
}