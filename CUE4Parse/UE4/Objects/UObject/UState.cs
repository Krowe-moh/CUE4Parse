using System;
using System.Collections.Generic;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Versions;
using CUE4Parse.Utils;
using Newtonsoft.Json;

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

        if (Ar.Ver < EUnrealEngineObjectUE3Version.REDUCED_PROBEMASK_REMOVED_IGNOREMASK)
        {
            ProbeMask = Ar.Read<long>();
            IgnoreMask = Ar.Read<long>();
        }
        else
        {
            ProbeMask = Ar.Read<long>();
        }

        LabelTableOffset = Ar.Read<short>();
        StateFlags = Ar.Read<StateFlags>();
        if (Ar.Ver > EUnrealEngineObjectUE3Version.MovedFriendlyNameToUFunction)
        {
            FuncMap = Ar.ReadMap(Ar.ReadFName, () => new FPackageIndex(Ar));
        }
    }

    protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer)
    {
        base.WriteJson(writer, serializer);

        writer.WritePropertyName("ProbeMask");
        writer.WriteValue(ProbeMask);

        if (IgnoreMask != 0)
        {
            writer.WritePropertyName("IgnoreMask");
            writer.WriteValue(IgnoreMask);
        }

        writer.WritePropertyName("LabelTableOffset");
        writer.WriteValue(LabelTableOffset);

        if (StateFlags != 0)
        {
            writer.WritePropertyName("StateFlags");
            writer.WriteValue(StateFlags.ToStringBitfield());
        }

        if (FuncMap is { Count: > 0 })
        {
            writer.WritePropertyName("FuncMap");
            serializer.Serialize(writer, FuncMap);
        }
        writer.WriteEndObject();
    }
}
