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
            if (Ar.Game == EGame.GAME_RocketLeague)
            {
                UncompressedOffset = (int)Ar.Read<long>();
                UncompressedSize = (int)Ar.Read<long>();
                goto SkipToCompressed;
            }

            UncompressedOffset = Ar.Read<int>();
            UncompressedSize = Ar.Read<int>();
            SkipToCompressed:
            CompressedOffset = Ar.Read<int>();
            CompressedSize = Ar.Read<int>();
        }
    }
}
