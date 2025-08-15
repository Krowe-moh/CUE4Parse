using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Objects.Engine
{
    public readonly struct FTextureAllocations : IUStruct
    {
        public readonly int Width;
        public readonly int Height;
        public readonly int MipCount;
        public readonly uint Format;
        public readonly uint CreateFlags;
        public readonly int[] ExportIndices;

        public FTextureAllocations(FArchive Ar)
        {
            Width = Ar.Read<int>();
            Height = Ar.Read<int>();
            MipCount = Ar.Read<int>();
            Format = Ar.Read<uint>();
            CreateFlags = Ar.Read<uint>();
            ExportIndices = Ar.ReadArray<int>();
        }
    }
}