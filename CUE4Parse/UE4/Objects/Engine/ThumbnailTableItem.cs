﻿using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Objects.Engine
{
    public readonly struct ThumbnailTableItem : IUStruct
    {
        public readonly string ObjectClassName;
        public readonly string ObjectPath;
        public readonly int ThumbnailOffset;

        public ThumbnailTableItem(FArchive Ar)
        {
            if (Ar.Ver >= EUnrealEngineObjectUE3Version.VER_CONTENT_BROWSER_FULL_NAMES)
            {
                ObjectClassName = Ar.ReadFString();
            }
            ObjectPath = Ar.ReadFString();
            ThumbnailOffset = Ar.Read<int>();
        }
    }
}
