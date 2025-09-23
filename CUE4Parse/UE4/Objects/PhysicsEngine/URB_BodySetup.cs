using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Objects.PhysicsEngine
{
    public class URB_BodySetup : Assets.Exports.UObject
    {
        public override void Deserialize(FAssetArchive Ar, long validPos)
        {
            base.Deserialize(Ar, validPos);

            if (Ar.Ver >= EUnrealEngineObjectUE3Version.VER_PRECACHE_STATICMESH_COLLISION)
            {
                Ar.ReadArray(() => Ar.ReadArray(() => Ar.ReadArray<byte>())); // PreCachedPhysData
            }
        }
    }
}
