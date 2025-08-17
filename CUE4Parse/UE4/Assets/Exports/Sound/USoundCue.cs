using System.Collections.Generic;
using CUE4Parse.UE4.Assets.Exports.Animation.CurveExpression;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Engine;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Assets.Exports.Sound
{
    public struct NodeEditorData
    {
        public int X;
        public int Y;
    }

    public class USoundCue : USoundBase
    {
        public FPackageIndex? FirstNode;
        public Dictionary<UObject?, NodeEditorData>? EditorData;

        public override void Deserialize(FAssetArchive Ar, long validPos)
        {
            base.Deserialize(Ar, validPos);
            FirstNode = GetOrDefault<FPackageIndex>(nameof(FirstNode));

            if (Ar.Game < EGame.Game_UE4_0)
            {
                EditorData = new Dictionary<UObject, NodeEditorData>();
                int Count = Ar.Read<int>();
                for (int i = 0; i < Count; i++)
                {
                    var key = Ar.ReadUObject(); // Sometimes can be null, ReadMap can't be used.
                    var value = Ar.Read<NodeEditorData>();

                    if (key != null)
                        EditorData[key] = value;
                }
            }
            else if (Ar.Ver >= EUnrealEngineObjectUE4Version.COOKED_ASSETS_IN_EDITOR_SUPPORT)
            {
                var _ = new FStripDataFlags(Ar);
            }
        }
    }
}
