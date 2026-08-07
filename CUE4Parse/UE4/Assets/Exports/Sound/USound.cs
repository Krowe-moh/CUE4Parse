using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.UObject;

namespace CUE4Parse.UE4.Assets.Exports.Sound;

public class UMusic : USound;
public class USound : UObject
{
    public FName FileType;
    public FByteBulkData Data;

    public override void Deserialize(FAssetArchive Ar, long validPos)
    {
        base.Deserialize(Ar, validPos);
        FileType = Ar.ReadFName();

        Ar.Read<int>(); // skipOffset
        Data = new FByteBulkData(Ar.ReadArray<byte>());
    }
}
