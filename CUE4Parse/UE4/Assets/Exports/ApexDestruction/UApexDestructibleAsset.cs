using CUE4Parse.UE4.Assets.Readers;

namespace CUE4Parse.UE4.Assets.Exports.ApexDestruction;

public class UApexClothingAsset : UApexDestructibleAsset;
public class UApexDestructibleAsset : UObject
{
    public override void Deserialize(FAssetArchive Ar, long validPos)
    {
        base.Deserialize(Ar, validPos);

        var bAssetValid = Ar.ReadBoolean();

        if (bAssetValid)
        {
            Ar.ReadArray<byte>(); // NameBuffer
            Ar.ReadArray<byte>(); // NxDestructibleAssetBuffer
        }
    }
}
