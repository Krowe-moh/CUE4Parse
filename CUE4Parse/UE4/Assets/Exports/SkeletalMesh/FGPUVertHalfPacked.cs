using CUE4Parse.UE4.Objects.Meshes;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Assets.Exports.SkeletalMesh;

public class FGPUVertHalfPacked : FSkelMeshVertexBase
{
    public readonly FMeshUVHalf[] UV;

    public FGPUVertHalfPacked()
    {
        UV = [];
    }
    public FGPUVertHalfPacked(FArchive Ar, int numSkelUVSets) : this()
    {
        SerializeForEditorr(Ar);
        
        Ar.Read<int>(); // packed index
        UV = Ar.ReadArray<FMeshUVHalf>(numSkelUVSets);
    }
}
