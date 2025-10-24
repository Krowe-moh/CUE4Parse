using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Assets.Exports.StaticMesh;

public class FRawStaticIndexBuffer() : FRawIndexBuffer
{
    public FRawStaticIndexBuffer(FArchive Ar) : this()
    {
        if (Ar.Ver < EUnrealEngineObjectUE4Version.SUPPORT_32BIT_STATIC_MESH_INDICES)
        {
            SetIndices(Ar.ReadBulkArray<ushort>());
        }
        else
        {
            var is32bit = Ar.ReadBoolean();
            var data = Ar.ReadBulkArray<byte>();
            using var tempAr = new FByteArchive("IndicesReader", data, Ar.Versions);

            if (Ar.Versions["RawIndexBuffer.HasShouldExpandTo32Bit"])
            {
                Indices16 = Ar.ReadBulkArray<ushort>();
                if (Ar.Ver < EUnrealEngineObjectUE3Version.VER_RENDERING_REFACTOR) Ar.Read<int>();
            }
            else
            {
                var count = (int)tempAr.Length / 2;
                SetIndices(tempAr.ReadArray<ushort>(count));
            }

            if (Ar.Game == EGame.GAME_PlayerUnknownsBattlegrounds && Buffer is not null)
            {
                var cur = 0;
                for (var i = 0; i < Buffer.Length; i++)
                {
                    cur += (short)Buffer[i];
                    Buffer[i] = (ushort)cur;
                }
            }
        }
    }
}