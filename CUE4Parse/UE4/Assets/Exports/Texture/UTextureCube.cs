using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Engine;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Assets.Exports.Texture;

public class UTextureCube : UTexture
{
    public override void Deserialize(FAssetArchive Ar, long validPos)
    {
        base.Deserialize(Ar, validPos);

        
        if (Ar.Game < EGame.GAME_UE4_0 && Ar.Ver < EUnrealEngineObjectUE3Version.VER_RENDERING_REFACTOR)
        {
            var SizeX = Ar.Read<int>();
            var SizeY = Ar.Read<int>();
            var format = Ar.Read<int>();
            Format = (EPixelFormat)format;
            var numMips = Ar.Read<int>();
        }
        
        if (Ar.Game >= EGame.GAME_UE4_0)
        {
            var stripFlags = new FStripDataFlags(Ar);
            var bCooked = Ar.ReadBoolean();

            if (bCooked)
            {
                DeserializeCookedPlatformData(Ar);
            }
        }
    }
}

public class UTextureCubeArray : UTexture
{
    public override void Deserialize(FAssetArchive Ar, long validPos)
    {
        base.Deserialize(Ar, validPos);

        var stripFlags = new FStripDataFlags(Ar);
        var bCooked = Ar.ReadBoolean();

        if (bCooked)
        {
            DeserializeCookedPlatformData(Ar);
        }
    }
}
