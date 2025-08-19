using System;
using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Objects.Engine;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Assets.Exports.Texture;

public class UTexture2D : UTexture
{
    public FIntPoint ImportedSize { get; private set; }
    public TextureAddress AddressX { get; private set; }
    public TextureAddress AddressY { get; private set; }

    public override TextureAddress GetTextureAddressX() => AddressX;
    public override TextureAddress GetTextureAddressY() => AddressY;

    public override void Deserialize(FAssetArchive Ar, long validPos)
    {
        base.Deserialize(Ar, validPos);
        ImportedSize = GetOrDefault<FIntPoint>(nameof(ImportedSize));
        AddressX = GetOrDefault<TextureAddress>(nameof(AddressX));
        AddressY = GetOrDefault<TextureAddress>(nameof(AddressY));

        var bCooked = false;
        if (Ar.Game >= EGame.GAME_UE4_0)
        {
            var stripDataFlags = Ar.Read<FStripDataFlags>();
            bCooked = Ar.Ver >= EUnrealEngineObjectUE4Version.ADD_COOKED_TO_TEXTURE2D && Ar.ReadBoolean();
        }

        if (Ar.Ver < EUnrealEngineObjectUE3Version.DisplacedUTextureProperties)
        {
            var SizeX = Ar.Read<int>();
            var SizeY = Ar.Read<int>();
            var format = Ar.Read<int>();
            Format = (EPixelFormat)format;
        }
        if (Ar.Ver < EUnrealEngineObjectUE4Version.TEXTURE_SOURCE_ART_REFACTOR)
        {
            var legacyMips = Array.Empty<FTexture2DMipMap>();

            var bHasLegacyMips = Ar.Game >= EGame.GAME_UE4_0 ? GetOrDefault("bDisableDerivedDataCache_DEPRECATED", false) : true;
            if (bHasLegacyMips)
            {
                legacyMips = Ar.ReadArray(() => new FTexture2DMipMap(Ar));
            }

            if (Ar.Ver >= EUnrealEngineObjectUE3Version.VER_ADDED_TEXTURE_FILECACHE_GUIDS)
            {
                var textureFileCacheGuidDeprecated = Ar.Read<FGuid>();
            }
            
            if (Ar.Ver >= EUnrealEngineObjectUE3Version.VER_ADDED_CACHED_IPHONE_DATA)
            {
                Ar.ReadArray(() => new FTexture2DMipMap(Ar));
            }
            
            if (Ar.Ver >= EUnrealEngineObjectUE3Version.VER_VERSION_NUMBER_FIX_FOR_FLASH_TEXTURES)
            {
                Ar.Read<int>();
                Ar.ReadArray(() => new FTexture2DMipMap(Ar));
                new FByteBulkData(Ar);
            }

            if (Ar.Ver >= EUnrealEngineObjectUE3Version.VER_ANDROID_ETC_SEPARATED)
            {
                Ar.ReadArray(() => new FTexture2DMipMap(Ar));
            }

            Format = GetOrDefault(nameof(Format), EPixelFormat.PF_Unknown);

            if (bHasLegacyMips && legacyMips.Length > 0)
            {
                PlatformData.Mips = legacyMips;
            }
        }

        if (bCooked)
        {
            var bSerializeMipData = true;
            if (Ar.Game >= EGame.GAME_UE5_3)
            {
                // Controls whether FByteBulkData is serialized??
                bSerializeMipData = Ar.ReadBoolean();
            }

            DeserializeCookedPlatformData(Ar, bSerializeMipData);
        }
    }
}
