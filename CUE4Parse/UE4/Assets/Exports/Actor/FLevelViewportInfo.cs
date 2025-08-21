using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Core.Math;

namespace CUE4Parse.UE4.Assets.Exports.Actor;

// too lazy to put it correct spot for now
public class FLevelViewportInfo
{
    public FVector CamPosition;
    public FRotator CamRotation;
    public float CamOrthoZoom;

    public FLevelViewportInfo(FAssetArchive Ar)
    {
        CamPosition = Ar.Read<FVector>();
        CamRotation = Ar.Read<FRotator>();
        CamOrthoZoom = Ar.Read<float>();
    }
}
