using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Assets.Objects
{
    public readonly struct FCompressedChunk
    {
        public readonly long UncompressedOffset;
        public readonly long UncompressedSize;
        public readonly int CompressedOffset;
        public readonly int CompressedSize;

        public FCompressedChunk(FAssetArchive Ar)
        {
            if (Ar.Game == EGame.GAME_RocketLeague)
            {
                UncompressedOffset = Ar.Read<long>();
                UncompressedSize = Ar.Read<long>();
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
