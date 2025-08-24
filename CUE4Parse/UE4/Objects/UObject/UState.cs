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
        
        if (Ar.Game >= EGame.GAME_UE4_0) return;
        
        if (Ar.Ver < EUnrealEngineObjectUE3Version.VER_REDUCED_PROBEMASK_REMOVED_IGNOREMASK)
        {
            ProbeMask = Ar.Read<long>();
            IgnoreMask = Ar.Read<long>();
        }
        else
        {
            ProbeMask = Ar.Read<long>();
        }

        LabelTableOffset = Ar.Read<short>();
        StateFlags = (StateFlags)Ar.Read<int>();
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