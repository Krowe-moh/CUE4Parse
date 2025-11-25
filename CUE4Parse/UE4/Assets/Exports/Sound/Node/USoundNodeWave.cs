using System.IO;
using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Assets.Exports.Sound.Node
{
    public class USoundNodeWave : UObject
    {
        public FFormatContainer? CompressedFormatData;
        public FByteBulkData? RawSound;
        public FByteBulkData? PCSound;
        public FByteBulkData? XboxSound;
        public FByteBulkData? PS3Sound;
        public FByteBulkData? WIIUSound;
        public FByteBulkData? IPhoneSound;
        public FByteBulkData? FlashSound;

        public override void Deserialize(FAssetArchive Ar, long validPos)
        {
            base.Deserialize(Ar, validPos);

            var bCooked = Ar.Ver > EUnrealEngineObjectUE4Version.ADD_COOKED_TO_SOUND_NODE_WAVE && Ar.ReadBoolean();

            if (Ar.Ver < EUnrealEngineObjectUE3Version.VER_ADDED_CACHED_COOKED_PC_DATA)
            {
                Ar.ReadFName(); // FileType
            }

            if (Ar.Ver >= EUnrealEngineObjectUE3Version.VER_UPDATED_SOUND_NODE_WAVE && Ar.Ver < EUnrealEngineObjectUE3Version.VER_CLEANUP_SOUNDNODEWAVE)
            {
                Ar.ReadArray<int>(); // ChannelOffsets
                Ar.ReadArray<int>(); // ChannelSizes
            }

            if (bCooked)
            {
                CompressedFormatData = new FFormatContainer(Ar);
            }
            else
            {
                RawSound = new FByteBulkData(Ar);
            }

            if (Ar.Ver >= EUnrealEngineObjectUE3Version.VER_ADDED_RAW_SURROUND_DATA && Ar.Ver < EUnrealEngineObjectUE3Version.VER_UPDATED_SOUND_NODE_WAVE)
            {
                Ar.ReadArray(() => new FByteBulkData(Ar));
            }

            if (Ar.Ver >= EUnrealEngineObjectUE3Version.VER_ADDED_NUM_CHANNELS && Ar.Ver < EUnrealEngineObjectUE3Version.VER_CLEANUP_SOUNDNODEWAVE)
            {
                Ar.Read<int>(); // ChannelCount
            }

            if (Ar.Ver < EUnrealEngineObjectUE4Version.ADD_SOUNDNODEWAVE_TO_DDC)
            {
                if (Ar.Ver >= EUnrealEngineObjectUE3Version.VER_ADDED_CACHED_COOKED_PC_DATA)
                {
                    PCSound = new FByteBulkData(Ar);
                }

                if (Ar.Game == EGame.GAME_SuddenAttack2) return;

                if (Ar.Ver >= EUnrealEngineObjectUE3Version.VER_ADDED_CACHED_COOKED_XBOX360_DATA)
                {
                    XboxSound = new FByteBulkData(Ar);
                }

                if (Ar.Ver >= EUnrealEngineObjectUE3Version.VER_ADDED_CACHED_COOKED_PS3_DATA)
                {
                    PS3Sound = new FByteBulkData(Ar);
                }

                if (Ar.Ver >= EUnrealEngineObjectUE3Version.VER_WIIU_COMPRESSED_SOUNDS)
                {
                    WIIUSound = new FByteBulkData(Ar);
                }

                if (Ar.Ver >= EUnrealEngineObjectUE3Version.VER_IPHONE_COMPRESSED_SOUNDS)
                {
                    IPhoneSound = new FByteBulkData(Ar);
                }

                if (Ar.Ver >= EUnrealEngineObjectUE3Version.VER_FLASH_MERGE_TO_MAIN)
                {
                    FlashSound = new FByteBulkData(Ar);
                }
            }

            // todo find the date they removed, if they did.
            if (Ar.Ver >= EUnrealEngineObjectUE4Version.ADD_SOUNDNODEWAVE_GUID)
            {
                Ar.Read<FGuid>(); // CompressedDataGuid
            }
        }
    }
}
