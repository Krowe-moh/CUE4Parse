using CUE4Parse.UE4.Assets.Exports.Texture;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Versions;
using CUE4Parse.Utils;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.UObject;

public class UFunction : UStruct
{
    public EFunctionFlags FunctionFlags;
    public FPackageIndex? EventGraphFunction; // UFunction
    public int EventGraphCallOffset;

    public override void Deserialize(FAssetArchive Ar, long validPos)
    {
        base.Deserialize(Ar, validPos);
        if (Ar.Game < EGame.GAME_UE4_0)
        {
            if (Ar.Ver < EUnrealEngineObjectUE3Version.Release64)
            {
                var paramsSize = Ar.Read<ushort>();
            }

            var NativeToken = Ar.Read<ushort>();

            if (Ar.Ver < EUnrealEngineObjectUE3Version.Release64)
            {
                var paramsCount = Ar.Read<byte>();
            }

            var OperPrecedence = Ar.Read<byte>();

            if (Ar.Ver < EUnrealEngineObjectUE3Version.Release64)
            {
                var returnValueOffset = Ar.Read<ushort>();
            }
        }

        FunctionFlags = Ar.Read<EFunctionFlags>();
        // rocket league, maybe use flag?
        if (Ar.Game is EGame.GAME_AshesOfCreation or EGame.GAME_RocketLeague) Ar.Position += 4;
        
        // Replication info
        if (FunctionFlags.HasFlag(EFunctionFlags.FUNC_Net))
        {
            // Unused.
            var RepOffset = Ar.Read<short>();
        }

        if (Ar.Ver >= EUnrealEngineObjectUE3Version.MovedFriendlyNameToUFunction && !Ar.Owner.Summary.PackageFlags.HasFlag(EPackageFlags.PKG_Cooked) && Ar.Platform != ETexturePlatform.XboxAndPlaystation)
        {
            // ignore platform.
            // vro this broken vro
            var FriendlyName = Ar.ReadFName();
        }

        if (Ar.Ver >= EUnrealEngineObjectUE4Version.SERIALIZE_BLUEPRINT_EVENTGRAPH_FASTCALLS_IN_UFUNCTION)
        {
            EventGraphFunction = new FPackageIndex(Ar);
            EventGraphCallOffset = Ar.Read<int>();
        }
    }

    internal EAccessMode GetAccessMode()
    {
        if (FunctionFlags.HasFlag(EFunctionFlags.FUNC_Public))
            return EAccessMode.Public;

        if (FunctionFlags.HasFlag(EFunctionFlags.FUNC_Protected))
            return EAccessMode.Protected;

        return EAccessMode.Private;
    }

    protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer)
    {
        base.WriteJson(writer, serializer);

        writer.WritePropertyName("FunctionFlags");
        writer.WriteValue(FunctionFlags.ToStringBitfield());

        if (EventGraphFunction is { IsNull: false })
        {
            writer.WritePropertyName("EventGraphFunction");
            serializer.Serialize(writer, EventGraphFunction);
        }

        if (EventGraphCallOffset != 0)
        {
            writer.WritePropertyName("EventGraphCallOffset");
            writer.WriteValue(EventGraphCallOffset);
        }
    }
}
