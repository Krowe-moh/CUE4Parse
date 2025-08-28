using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Assets.Objects
{
    public readonly struct FCompressedChunk
    {
        public readonly int UncompressedOffset;
        public readonly int UncompressedSize;
        public readonly int CompressedOffset;
        public readonly int CompressedSize;

        public FCompressedChunk(FArchive Ar)
        {
            UncompressedOffset = Ar.Game == EGame.GAME_RocketLeague ? (int)Ar.Read<long>() : Ar.Read<int>();
            UncompressedSize = Ar.Read<int>();
            CompressedOffset = Ar.Game == EGame.GAME_RocketLeague ? (int)Ar.Read<long>() : Ar.Read<int>();
            CompressedSize = Ar.Read<int>();
        }
    }
}
