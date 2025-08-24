using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Assets.Exports.Engine.Font
{
    public class UMultiFont : UObject
    {
        public override void Deserialize(FAssetArchive Ar, long validPos)
        {
            base.Deserialize(Ar, validPos);

            if (Ar.Ver < EUnrealEngineObjectUE3Version.VER_FIXED_FONTS_SERIALIZATION)
            {
                Ar.Read<int>(); // ResolutionTestTable
            }
        }
    }
}
