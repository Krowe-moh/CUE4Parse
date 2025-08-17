using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Objects.Engine
{
    public readonly struct ThumbnailTableItem : IUStruct
    {
        public readonly string ObjectClassName;
        public readonly string ObjectPath;
        public readonly int ThumbnailOffset;

        public ThumbnailTableItem(FArchive Ar)
        {
            ObjectClassName = Ar.ReadFString();
            ObjectPath = Ar.ReadFString();
            ThumbnailOffset = Ar.Read<int>();
        }
    }
}