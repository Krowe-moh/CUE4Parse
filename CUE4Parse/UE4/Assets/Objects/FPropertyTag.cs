using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CUE4Parse.MappingsProvider;
using CUE4Parse.MappingsProvider.Usmap;
using CUE4Parse.UE4.Assets.Objects.Properties;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Exceptions;
using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Objects.UObject.BlueprintDecompiler;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Assets.Objects;

public enum EPropertyTagSerializeType : byte
{
    /** Tag was loaded from an older version or has not yet been saved. */
    Unknown,
    /** Serialization of the property value was skipped. Tag has no value. */
    Skipped,
    /** Serialized with tagged property serialization. */
    Property,
    /** Serialized with binary or native serialization. */
    BinaryOrNative,
};

[Flags]
public enum EPropertyTagFlags : byte
{
    None = 0x00,
    HasArrayIndex = 0x01,
    HasPropertyGuid = 0x02,
    HasPropertyExtensions = 0x04,
    HasBinaryOrNativeSerialize = 0x08,
    BoolTrue = 0x10,
    SkippedSerialize = 0x20
}

[Flags]
public enum EPropertyTagExtension : byte
{
    NoExtension					= 0x00,
    ReserveForFutureUse			= 0x01, // Can be use to add a next group of extension

    ////////////////////////////////////////////////
    // First extension group
    OverridableInformation		= 0x02,

    //
    // Add more extension for the first group here
    //
}

public struct FPropertyTypeNameNode(FAssetArchive Ar)
{
    public FName Name = Ar.ReadFName();
    public int InnerCount = Ar.Read<int>();
}

public static class FPropertyTypeNameUtils
{
    public static string GetName(this Span<FPropertyTypeNameNode> nodes) => nodes.IsEmpty ? "None" : nodes[0].Name.Text;

    public static Span<FPropertyTypeNameNode> GetParameter(this Span<FPropertyTypeNameNode> nodes, int paramIndex)
    {
        if (nodes.IsEmpty) return [];
        if (paramIndex < 0 || paramIndex >= nodes[0].InnerCount) return [];

        var param = 1;
        for (int skip = paramIndex; skip > 0; --skip, ++param)
        {
            skip += nodes[param].InnerCount;
        }

        return nodes[param..];
    }
}

public class FPropertyTag
{

    public FName Name;
    public FName PropertyType;
    public int Size;
    public int ArrayIndex;
    public int? ArraySize;
    public FPropertyTagData? TagData;
    public bool HasPropertyGuid;
    public FGuid? PropertyGuid;
    public FPropertyTagType? Tag;
    public EPropertyTagFlags PropertyTagFlags;
#if DEBUG
    public long Position;
#endif

    public EPropertyTagSerializeType SerializeType => PropertyTagFlags.HasFlag(EPropertyTagFlags.SkippedSerialize)
            ? EPropertyTagSerializeType.Skipped
            : PropertyTagFlags.HasFlag(EPropertyTagFlags.HasBinaryOrNativeSerialize)
                ? EPropertyTagSerializeType.BinaryOrNative : EPropertyTagSerializeType.Property;

    /// <summary>
    /// EPropertyTagFlags.HasArrayIndex is only reliable on UE5 games
    /// ArrayIndex > 0 is used as a fallback for UE4 games but in this case IsIndexed will be false on the first element of the array
    /// </summary>
    public bool IsIndexed => PropertyTagFlags.HasFlag(EPropertyTagFlags.HasArrayIndex) || ArrayIndex > 0;

    public FPropertyTag() { }

    public FPropertyTag(FAssetArchive Ar, PropertyInfo info, ReadType type)
    {
        Name = new FName(info.Name);
        PropertyType = new FName(info.MappingType.Type);
        ArrayIndex = info.Index;
        ArraySize = info.ArraySize;
        TagData = new FPropertyTagData(info.MappingType);
        HasPropertyGuid = false;
        PropertyGuid = null;
        PropertyTagFlags = ArraySize > 1 ? EPropertyTagFlags.HasArrayIndex : EPropertyTagFlags.None;

        var pos = Ar.Position;
#if DEBUG
        Position = pos;
#endif
        try
        {
            Tag = FPropertyTagType.ReadPropertyTagType(Ar, PropertyType.Text, TagData, type);
        }
        catch (ParserException e)
        {
            throw new ParserException($"Failed to read FPropertyTagType {TagData?.ToString() ?? PropertyType.Text} {Name.Text}", e);
        }

        Size = (int) (Ar.Position - pos);
    }

    public FPropertyTag(FAssetArchive Ar, bool readData)
    {
        Name = Ar.ReadFName();
        if (Name.IsNone)
            return;

        if (Ar.Ver >= EUnrealEngineObjectUE5Version.PROPERTY_TAG_COMPLETE_TYPE_NAME)
        {
            var nodes = new List<FPropertyTypeNameNode>();
            var remaining = 1;
            do
            {
                var node = new FPropertyTypeNameNode(Ar);
                nodes.Add(node);
                remaining += node.InnerCount - 1;
            }
            while (remaining > 0);

            var typeName = CollectionsMarshal.AsSpan(nodes);
            PropertyType = typeName.GetName();
            TagData = new FPropertyTagData(typeName, Name.Text);

            Size = Ar.Read<int>();
            PropertyTagFlags = (EPropertyTagFlags) Ar.Read<byte>();
            if (PropertyTagFlags.HasFlag(EPropertyTagFlags.BoolTrue)) TagData.Bool = true;
            ArrayIndex = PropertyTagFlags.HasFlag(EPropertyTagFlags.HasArrayIndex) ? Ar.Read<int>() : 0;
            HasPropertyGuid = PropertyTagFlags.HasFlag(EPropertyTagFlags.HasPropertyGuid);
            PropertyGuid = HasPropertyGuid ? Ar.Read<FGuid>() : null;

            if (PropertyTagFlags.HasFlag(EPropertyTagFlags.HasPropertyExtensions))
            {
                var tagExtensions = Ar.Read<EPropertyTagExtension>();
                if (tagExtensions.HasFlag(EPropertyTagExtension.OverridableInformation))
                {
                    var OverrideOperation = Ar.Read<byte>(); // EOverriddenPropertyOperation
                    var bExperimentalOverridableLogic = Ar.ReadBoolean();
                }
            }
        }
        else
        {
            if (Ar.Ver < EUnrealEngineObjectUE3Version.RefactoredPropertyTags)
            {
                var info = Ar.Read<byte>();
                var type = (EPropertyType2) ((info & 0x0F));
                PropertyType = type.ToString();
                ArrayIndex = info & 0x80;

                TagData = new FPropertyTagData();

                switch (type)
                {
                    case EPropertyType2.StructProperty:
                        TagData.StructType = Ar.ReadFName().Text; // ItemName
                        break;
                }

                Size = DeserializePackedSize(Ar, (byte) (info & 0x70));

                switch (type)
                {
                    case EPropertyType2.BoolProperty:
                        TagData.Bool = ArrayIndex != 0;
                        Tag = FPropertyTagType.ReadPropertyTagType(Ar, PropertyType.Text, TagData, ReadType.ZERO, Size);
                        return;

                    default:
                    {
                        if (ArrayIndex != 0)
                        {
                            byte b = Ar.Read<byte>();

                            if ((b & 0x80) == 0)
                            {
                                ArrayIndex = b;
                            }
                            else if ((b & 0xC0) == 0x80)
                            {
                                byte c = Ar.Read<byte>();
                                ArrayIndex = ((b & 0x7F) << 8) + c;
                            }
                            else
                            {
                                byte c = Ar.Read<byte>();
                                byte d = Ar.Read<byte>();
                                byte e = Ar.Read<byte>();
                                ArrayIndex = ((b & 0x3F) << 24) + (c << 16) + (d << 8) + e;
                            }
                        }

                        break;
                    }
                }

                switch (type)
                {
                    case EPropertyType2.StructProperty:
                        Ar.Position += Size;
                        return;
                }

                goto gurt;
            }

            PropertyType = Ar.ReadFName();
            Size = Ar.Read<int>();
            ArrayIndex = Ar.Read<int>();

            TagData = new FPropertyTagData(Ar, PropertyType.Text, Name.Text);
            if (Ar.Ver >= EUnrealEngineObjectUE4Version.PROPERTY_GUID_IN_PROPERTY_TAG)
            {
                HasPropertyGuid = Ar.ReadFlag();
                if (HasPropertyGuid)
                {
                    PropertyGuid = Ar.Read<FGuid>();
                }
            }

            if (Ar.Ver >= EUnrealEngineObjectUE5Version.PROPERTY_TAG_EXTENSION_AND_OVERRIDABLE_SERIALIZATION)
            {
                var tagExtensions = Ar.Read<EPropertyTagExtension>();
                if (tagExtensions.HasFlag(EPropertyTagExtension.OverridableInformation))
                {
                    var OverrideOperation = Ar.Read<byte>(); // EOverriddenPropertyOperation
                    var bExperimentalOverridableLogic = Ar.ReadBoolean();
                }
            }
        }

        gurt:
        if (!readData) return;

        var pos = Ar.Position;
#if DEBUG
        Position = pos;
#endif
        var finalPos = pos + Size;
        try
        {
            Tag = FPropertyTagType.ReadPropertyTagType(Ar, PropertyType.Text, TagData, ReadType.NORMAL, Size);
#if DEBUG
            if (finalPos != Ar.Position)
            {
                Log.Debug("FPropertyTagType {0} {1} was not read properly, pos {2}, calculated pos {3}", TagData?.ToString() ?? PropertyType.Text, Name.Text, Ar.Position, finalPos);
            }
#endif
        }
        catch (ParserException e)
        {
#if DEBUG
            if (finalPos != Ar.Position)
            {
                Log.Warning(e, "Failed to read FPropertyTagType {0} {1}, skipping it", TagData?.ToString() ?? PropertyType.Text, Name.Text);
            }
#endif
        }
        finally
        {
            // Always seek to calculated position, no need to crash
            Ar.Position = finalPos;
        }
    }

    public FPropertyTag(FName name, FName propertyType, int size, int arrayIndex, FPropertyTagData? tagData, bool hasPropertyGuid, FGuid? propertyGuid, FPropertyTagType? tag)
    {
        Name = name;
        PropertyType = propertyType;
        Size = size;
        ArrayIndex = arrayIndex;
        TagData = tagData;
        HasPropertyGuid = hasPropertyGuid;
        PropertyGuid = propertyGuid;
        Tag = tag;
    }

    public FPropertyTag(FName propertyType, FPropertyTagType tag, FPropertyTagData? tagData = null)
    {
        PropertyType = propertyType;
        Tag = tag;
        TagData = tagData;
    }


    private static int DeserializePackedSize(FArchive Ar, byte sizePack)
    {
        switch (sizePack)
        {
            case 0x00:
                return 1;

            case 0x10:
                return 2;

            case 0x20:
                return 4;

            case 0x30:
                return 12;

            case 0x40:
                return 16;

            case 0x50:
                return Ar.Read<byte>(); // SizeByte

            case 0x60:
                return Ar.Read<ushort>(); // SizeWord

            case 0x70:
                return Ar.Read<int>(); // SizeInt

            default:
                throw new NotImplementedException($"Unknown sizePack {sizePack}");
        }
    }

    internal string GetCppVariable()
    {
        if (!BlueprintDecompilerUtils.GetPropertyTagVariable(this, out var variableType, out var variableValue))
        {
            Log.Warning("Unable to get property type or value for {PropertyType} of type {Name}", PropertyType, Name);
        }

        return $"{variableType} {Name.Text} = {variableValue};";
    }

    public override string ToString() => $"{Name.Text}  -->  {Tag?.ToString() ?? "Failed to parse"}";
}
