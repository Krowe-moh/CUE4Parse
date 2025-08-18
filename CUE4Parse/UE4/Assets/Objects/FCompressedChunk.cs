using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Assets.Objects
{
    public readonly struct FCompressedChunk
    {
        public readonly int UncompressedOffset;
        public readonly int UncompressedSize;
        public readonly int CompressedOffset;
        public readonly int CompressedSize;

        public FCompressedChunk(FAssetArchive Ar)
        {
            if (Ar.Game == EGame.GAME_RocketLeague)
            {
                UncompressedOffset = Ar.Read<int>();
                UncompressedSize = Ar.Read<int>();
                goto streamStandardSize;
            }

            UncompressedOffset = Ar.Read<int>();
            UncompressedSize = Ar.Read<int>();
            streamStandardSize:
            CompressedOffset = Ar.Read<int>();
            CompressedSize = Ar.Read<int>();
        }
    }
}