using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Objects.Engine;

struct FUIMouseCursor
{
    private string CursorStyle;
    private FPackageIndex Cursor;

    public FUIMouseCursor(FAssetArchive Ar)
    {
        CursorStyle = Ar.ReadFName().Text;
        Cursor = new FPackageIndex(Ar);
    }
};

public class UUISkin : Assets.Exports.UObject
{
    public override void Deserialize(FAssetArchive Ar, long validPos)
    {
        base.Deserialize(Ar, validPos);

        if (Ar.Ver < EUnrealEngineObjectUE3Version.VER_REFACTORED_UISKIN)
        {
            Ar.ReadMap(() => Ar.Read<FGuid>(), () => Ar.Read<FGuid>()); // WidgetStyleMap
        }
        if (Ar.Ver >= EUnrealEngineObjectUE3Version.VER_ADDED_CURSOR_MAP)
        {
            Ar.ReadMap(Ar.ReadFName, () => new FUIMouseCursor(Ar)); // CursorMap
        }
    }
}
