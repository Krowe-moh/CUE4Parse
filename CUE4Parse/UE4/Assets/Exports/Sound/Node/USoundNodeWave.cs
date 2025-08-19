using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Assets.Exports.Sound.Node
{
    public class USoundNodeWave : UObject
    {
        
        public override void Deserialize(FAssetArchive Ar, long validPos)
        {
            base.Deserialize(Ar, validPos);

            if (Ar.Ver < EUnrealEngineObjectUE3Version.AddedPCSoundData)
            {
                Ar.ReadFName(); // FileType?
            }

            if (Ar.Ver >= EUnrealEngineObjectUE3Version.AddedChannelsSoundInfo && Ar.Ver < EUnrealEngineObjectUE3Version.DisplacedSoundChannelProperties)
            {
                Ar.ReadArray<int>();//ChannelOffsets
                Ar.ReadArray<int>();//ChannelSizes
            }

            new FByteBulkData(Ar);
            
            if (Ar.Ver >= EUnrealEngineObjectUE3Version.AddedChannelsSoundInfo && Ar.Ver < EUnrealEngineObjectUE3Version.DisplacedSoundChannelProperties)
            {
                Ar.Read<int>();//ChannelCount
            }
            
            if (Ar.Ver >= EUnrealEngineObjectUE3Version.AddedPCSoundData)
            {
                new FByteBulkData(Ar);
            }
            
            if (Ar.Ver >= EUnrealEngineObjectUE3Version.AddedXenonSoundData)// xbox
            {
                new FByteBulkData(Ar);
            }
            
            if (Ar.Ver >= EUnrealEngineObjectUE3Version.AddedPS3SoundData)
            {
                new FByteBulkData(Ar);
            }
            
            if (Ar.Ver >= EUnrealEngineObjectUE3Version.VER_WIIU_COMPRESSED_SOUNDS)
            {
                new FByteBulkData(Ar);
            }
            
            if (Ar.Ver >= EUnrealEngineObjectUE3Version.VER_IPHONE_COMPRESSED_SOUNDS)
            {
                new FByteBulkData(Ar);
            }
            
            if (Ar.Ver >= EUnrealEngineObjectUE3Version.VER_FLASH_MERGE_TO_MAIN)
            {
                new FByteBulkData(Ar);
            }
        }
    }
}