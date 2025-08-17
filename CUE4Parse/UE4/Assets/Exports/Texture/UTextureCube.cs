using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Engine;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Assets.Exports.Texture;

public class UTextureCube : UTexture
{
    public override void Deserialize(FAssetArchive Ar, long validPos)
    {
        base.Deserialize(Ar, validPos);

        if (Ar.Ver > EUnrealEngineObjectUE4Version.DETERMINE_BY_GAME)
        {
            var stripFlags = new FStripDataFlags(Ar);
            bool bCooked = Ar.ReadBoolean();

            if (bCooked)
            {
                DeserializeCookedPlatformData(Ar);
            }
        }
    }
}