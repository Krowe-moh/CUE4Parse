using CUE4Parse.UE4.Objects.Meshes;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Assets.Exports.SkeletalMesh;

public class FGPUVertFloat : FSkelMeshVertexBase
{
    public FMeshUVFloat[] UV;

    public FGPUVertFloat()
    {
        UV = [];
    }

    public FGPUVertFloat(FArchive Ar, bool bExtraBoneInfluences, int numSkelUVSets) : this()
    {
        SerializeForGPU(Ar, bExtraBoneInfluences);

        UV = Ar.ReadArray<FMeshUVFloat>(numSkelUVSets);
    }
}
