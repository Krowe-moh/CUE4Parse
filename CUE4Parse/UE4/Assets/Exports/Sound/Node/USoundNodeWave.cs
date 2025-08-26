using System.IO;
using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Assets.Exports.Sound.Node
{
    public class USoundNodeWave : UObject
    {
        public FByteBulkData DefaultSound;
        public FByteBulkData PCSound;
        public FByteBulkData XboxSound;
        public FByteBulkData PS3Sound;
        public FByteBulkData WIIUSound;
        public FByteBulkData IPhoneSound;
        public FByteBulkData FlashSound;
        
        public override void Deserialize(FAssetArchive Ar, long validPos)
        {
            base.Deserialize(Ar, validPos);

            if (Ar.Ver < EUnrealEngineObjectUE3Version.AddedPCSoundData)
            {
                Ar.ReadFName(); // FileType
            }

            if (Ar.Ver >= EUnrealEngineObjectUE3Version.AddedChannelsSoundInfo && Ar.Ver < EUnrealEngineObjectUE3Version.DisplacedSoundChannelProperties)
            {
                Ar.ReadArray<int>(); // ChannelOffsets
                Ar.ReadArray<int>(); // ChannelSizes
            }

            DefaultSound = new FByteBulkData(Ar);
            
            if (Ar.Ver >= EUnrealEngineObjectUE3Version.AddedChannelsSoundInfo && Ar.Ver < EUnrealEngineObjectUE3Version.DisplacedSoundChannelProperties)
            {
                Ar.Read<int>(); // ChannelCount
            }
            
            if (Ar.Ver >= EUnrealEngineObjectUE3Version.AddedPCSoundData)
            {
                PCSound = new FByteBulkData(Ar);
            }
            
            if (Ar.Ver >= EUnrealEngineObjectUE3Version.AddedXenonSoundData)
            {
                XboxSound = new FByteBulkData(Ar);
            }
            
            if (Ar.Ver >= EUnrealEngineObjectUE3Version.AddedPS3SoundData)
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
    }
}