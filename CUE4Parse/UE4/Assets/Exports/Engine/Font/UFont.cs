using System.Collections.Generic;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Assets.Exports.Engine.Font
{
    public class UFont : UObject
    {
        public Dictionary<ushort, ushort>? CharRemap;

        public override void Deserialize(FAssetArchive Ar, long validPos)
        {
            base.Deserialize(Ar, validPos);

            if (Ar.Ver < EUnrealEngineObjectUE3Version.Release122)
            {
                Ar.ReadArray(() => new FFontCharacter(Ar)); // Characters
                Ar.Read<int>(); // CharactersPerPage
            } else if (Ar.Ver < EUnrealEngineObjectUE3Version.VER_CHANGED_COMPRESSION_CHUNK_SIZE_TO_128)
            {
                Ar.ReadArray(() => new FFontCharacter(Ar)); // Characters
                Ar.ReadArray(() => new FPackageIndex(Ar)); // Textures
                if (Ar.Ver >= EUnrealEngineObjectUE3Version.Release119)
                {
                    Ar.Read<int>(); // Kerning
                }
            }

            if (Ar.Ver >= EUnrealEngineObjectUE3Version.Release69)
            {
                var num = Ar.Read<int>();
                CharRemap = new Dictionary<ushort, ushort>(num);
                for (var i = 0; i < num; ++i)
                {
                    CharRemap[Ar.Read<ushort>()] = Ar.Read<ushort>();
                }

                if (Ar.Ver < EUnrealEngineObjectUE3Version.VER_CHANGED_COMPRESSION_CHUNK_SIZE_TO_128)
                {
                    Ar.ReadBoolean(); // IsRemapped
                }
            }
        }
    }
}
