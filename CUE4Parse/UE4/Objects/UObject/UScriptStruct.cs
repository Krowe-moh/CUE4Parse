using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Objects.UObject;

public class UScriptStruct : UStruct
{
    public EStructFlags StructFlags;

    public override void Deserialize(FAssetArchive Ar, long validPos)
    {
        base.Deserialize(Ar, validPos);

        if (Ar.Ver >= EUnrealEngineObjectUE3Version.AddedStructFlagsToScriptStruct)
        {
            StructFlags = Ar.Read<EStructFlags>();
        }
        DeserializePropertiesTagged(Properties, Ar, true);
    }
}
