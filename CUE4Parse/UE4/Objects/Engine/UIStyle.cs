using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.UObject;

namespace CUE4Parse.UE4.Objects.Engine;

public class UUIStyle : Assets.Exports.UObject
{
    public override void Deserialize(FAssetArchive Ar, long validPos)
    {
        base.Deserialize(Ar, validPos);
        
        Ar.ReadMap(() => new FPackageIndex(Ar), () => new FPackageIndex(Ar)); // PreviousStateMap
    }
}
