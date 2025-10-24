using CUE4Parse.UE4.Assets.Exports.StaticMesh;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Assets.Exports.SkeletalMesh;

public class FMultisizeIndexContainer() : FRawIndexBuffer
{
    public FMultisizeIndexContainer(FArchive Ar) : this()
    {
        var dataSize = 0x02;
        if (Ar.Ver > EUnrealEngineObjectUE3Version.VER_DWORD_SKELETAL_MESH_INDICES)
        {
            if (Ar.Ver < EUnrealEngineObjectUE4Version.KEEP_SKEL_MESH_INDEX_DATA)
            {
                Ar.Position += 4; //var bOldNeedsCPUAccess = Ar.ReadBoolean();
            }

            dataSize = Ar.Read<byte>();
        }
        if (Ar.Game == EGame.GAME_OutlastTrials) Ar.Position += 4;

        if (dataSize == 0x02)
        {
            SetIndices(Ar.ReadBulkArray<ushort>());
        }
        else
        {
            SetIndices(Ar.ReadBulkArray<uint>());
        }
    }

    public FMultisizeIndexContainer(uint[] indices) : this()
    {
        SetIndices(indices);
    }
}
