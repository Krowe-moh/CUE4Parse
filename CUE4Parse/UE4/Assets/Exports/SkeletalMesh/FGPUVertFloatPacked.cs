using CUE4Parse.UE4.Objects.Meshes;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Assets.Exports.SkeletalMesh;

public class FGPUVertFloatPacked : FSkelMeshVertexBase
{
    public FMeshUVFloat[] UV;

    public FGPUVertFloatPacked()
    {
        UV = [];
    }

    public FGPUVertFloatPacked(FArchive Ar, int numSkelUVSets) : this()
    {
        SerializeForEditorr(Ar);
        
        Ar.Read<int>(); // packed index
        UV = Ar.ReadArray<FMeshUVFloat>(numSkelUVSets);
    }
}
