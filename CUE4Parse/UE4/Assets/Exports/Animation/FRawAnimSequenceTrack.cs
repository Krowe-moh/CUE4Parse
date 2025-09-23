using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Assets.Exports.Animation;

public class FRawAnimSequenceTrack : IUStruct
{
    public readonly FVector[] PosKeys;
    public readonly FQuat[] RotKeys;
    public readonly FVector[] ScaleKeys;
    public readonly float[] KeyTimes;

    public FRawAnimSequenceTrack(FArchive Ar)
    {
        if (Ar.Ver < EUnrealEngineObjectUE3Version.VER_NATIVE_RAWANIMDATA_SERIALIZATION)
        {
            PosKeys = Ar.ReadArray<FVector>();
            RotKeys = Ar.ReadArray<FQuat>();
            KeyTimes = Ar.ReadArray<float>();
            return;
        }

        PosKeys = Ar.ReadBulkArray<FVector>();
        RotKeys = Ar.ReadBulkArray<FQuat>();
        ScaleKeys = Ar.Ver >= EUnrealEngineObjectUE4Version.ANIM_SUPPORT_NONUNIFORM_SCALE_ANIMATION ? Ar.ReadBulkArray<FVector>() : [];
        KeyTimes = Ar.Ver < EUnrealEngineObjectUE3Version.VER_RAW_ANIMDATA_REDUX ? Ar.ReadBulkArray<float>() : [];
    }
}
