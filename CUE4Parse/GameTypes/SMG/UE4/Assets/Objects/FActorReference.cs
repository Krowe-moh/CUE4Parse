using CUE4Parse.UE4;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.GameTypes.SMG.UE4.Assets.Objects;

public class FActorReference : IUStruct
{
    public bool bIsAlias;
    public string ActorName;
    public byte Type;
    public byte SelectedType;

    public FActorReference(FAssetArchive Ar)
    {
        if (Ar.Game >= GAME_UE4_0)
        {
            bIsAlias = Ar.ReadBoolean();
            ActorName = Ar.ReadFString();
            Type = Ar.Read<byte>();
            SelectedType = Ar.Read<byte>();
        }
        else
        {
            new FPackageIndex(Ar); // Actor
            Ar.Read<FGuid>(); // Guid
        }
    }
}
