using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Assets.Exports.SkeletalMesh;

public class FSkinWeightInfo
{
    private const int NUM_INFLUENCES_UE4 = 4;
    private const int MAX_TOTAL_INFLUENCES_UE4 = 8;
    private int NUM_INFLUENCES_UE3 = 4;

    public ushort[] BoneIndex;
    public ushort[] BoneWeight;
    public readonly bool bUse16BitBoneWeight = false;

    public FSkinWeightInfo()
    {
        BoneIndex = new ushort[NUM_INFLUENCES_UE4];
        BoneWeight = new ushort[NUM_INFLUENCES_UE4];
    }

    public FSkinWeightInfo(FArchive Ar, bool bExtraBoneInfluences, bool bUse16BitBoneIndex = false, bool bUse16BitBoneWeight = false, int length = 0)
    {
        this.bUse16BitBoneWeight = bUse16BitBoneWeight;

        if (Ar.Ver >= EUnrealEngineObjectUE3Version.VER_ADDED_MULTIPLE_UVS_TO_SKELETAL_MESH)
            NUM_INFLUENCES_UE3 = 4;

        if (Ar.Game < EGame.GAME_UE4_0)
        {
            BoneIndex = new ushort[NUM_INFLUENCES_UE3];
            BoneWeight = new ushort[NUM_INFLUENCES_UE3];

            if (Ar.Ver >= EUnrealEngineObjectUE3Version.VER_USE_UMA_RESOURCE_ARRAY_MESH_DATA)
            {
                var idx = Ar.ReadArray<byte>(NUM_INFLUENCES_UE3);
                var wt = Ar.ReadArray<byte>(NUM_INFLUENCES_UE3);

                for (int i = 0; i < NUM_INFLUENCES_UE3; i++)
                {
                    BoneIndex[i] = idx[i];
                    BoneWeight[i] = wt[i];
                }
            }
            else
            {
                var arr = Ar.ReadArray<short>(NUM_INFLUENCES_UE3);
                for (int i = 0; i < NUM_INFLUENCES_UE3; i++)
                {
                    BoneIndex[i] = (ushort)(arr[i] & 0xFF);
                    BoneWeight[i] = (ushort)((arr[i] >> 8) & 0xFF);
                }
            }

            return;
        }

        var numSkelInfluences =
            length > 0
                ? length
                : (bExtraBoneInfluences ? MAX_TOTAL_INFLUENCES_UE4 : NUM_INFLUENCES_UE4);

        BoneIndex = bUse16BitBoneIndex
            ? Ar.ReadArray<ushort>(numSkelInfluences)
            : Ar.ReadArray(numSkelInfluences, () => (ushort)Ar.Read<byte>());

        BoneWeight = bUse16BitBoneWeight
            ? Ar.ReadArray<ushort>(numSkelInfluences)
            : Ar.ReadArray(numSkelInfluences, () => (ushort)Ar.Read<byte>());
    }
}
