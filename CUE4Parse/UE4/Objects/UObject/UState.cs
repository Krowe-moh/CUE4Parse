using System;
using System.Collections.Generic;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Objects.UObject;

[Flags]
public enum StateFlags : uint
{
    Editable    = 1 << 0,
    Auto        = 1 << 1,
    Simulated   = 1 << 2,
}

public class UState : UStruct
{
    public long ProbeMask;
    public long IgnoreMask;
    public short LabelTableOffset;
    public StateFlags StateFlags;
    public Dictionary<FName, FPackageIndex /*UFunction*/> FuncMap;

    public override void Deserialize(FAssetArchive Ar, long validPos)
    {
        base.Deserialize(Ar, validPos);

        ProbeMask = Ar.Read<long>();
        if (Ar.Ver < EUnrealEngineObjectUE3Version.VER_REDUCED_PROBEMASK_REMOVED_IGNOREMASK)
        {
            IgnoreMask = Ar.Read<long>();
        }

        LabelTableOffset = Ar.Read<short>();
        StateFlags = Ar.Read<StateFlags>();
        if (Ar.Ver > EUnrealEngineObjectUE3Version.MovedFriendlyNameToUFunction)
        {
            FuncMap = new Dictionary<FName, FPackageIndex>();
            var funcMapNum = Ar.Read<int>();
            for (var i = 0; i < funcMapNum; i++)
            {
                FuncMap[Ar.ReadFName()] = new FPackageIndex(Ar);
            }
        }
    }
}
