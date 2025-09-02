using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Assets.Exports.Component;

public class UPrimitiveComponent : USceneComponent
{
    public override void Deserialize(FAssetArchive Ar, long validPos)
    {
        if (Ar.Ver >= EUnrealEngineObjectUE3Version.AddedComponentGuid && Ar.Ver < EUnrealEngineObjectUE3Version.VER_REMOVED_COMPONENT_GUID) 
        {
            Ar.Read<FGuid>();
        }
    }
};
