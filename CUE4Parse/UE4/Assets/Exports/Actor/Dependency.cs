using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Objects.UObject;

namespace CUE4Parse.UE4.Assets.Exports.Actor;

// too lazy to put it correct spot for now
public class Dependency
{
    public FVector CamPosition;
    public FRotator CamRotation;
    public float CamOrthoZoom;

    public Dependency(FAssetArchive Ar)
    {
        new FPackageIndex(Ar);
        Ar.Read<int>();
        Ar.Read<uint>();
    }
}
