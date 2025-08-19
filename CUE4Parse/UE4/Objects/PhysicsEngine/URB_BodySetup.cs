using CUE4Parse.UE4.Assets.Readers;
namespace CUE4Parse.UE4.Objects.PhysicsEngine
{
    public class URB_BodySetup : Assets.Exports.UObject
    {
        public override void Deserialize(FAssetArchive Ar, long validPos)
        {
            base.Deserialize(Ar, validPos);

            Ar.ReadArray(() => Ar.ReadArray<byte>()); // PreCachedPhysData
        }
    }
}
