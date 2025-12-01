using System.Collections.Generic;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Engine;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Versions;
using Newtonsoft.Json;

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
        public Dictionary<FPackageIndex?, NodeEditorData>? EditorData;

        public override void Deserialize(FAssetArchive Ar, long validPos)
        {
            base.Deserialize(Ar, validPos);
            FirstNode = GetOrDefault<FPackageIndex>(nameof(FirstNode));

            // ver is temp.
            if (Ar.Ver >= EUnrealEngineObjectUE3Version.VER_IMPROVED_ANGLEBASEDSSAO && Ar.Ver < EUnrealEngineObjectUE4Version.SOUND_NODE_INHERIT_FROM_ED_GRAPH_NODE)
            {
                EditorData = new Dictionary<FPackageIndex, NodeEditorData>();
                int Count = Ar.Read<int>();
                for (int i = 0; i < Count; i++)
                {
                    var key = new FPackageIndex(Ar); // Sometimes can be null, ReadMap can't be used.
                    var value = Ar.Read<NodeEditorData>();

                    if (key != null)
                        EditorData[key] = value;
                }
            }

            if (Ar.Ver >= EUnrealEngineObjectUE4Version.COOKED_ASSETS_IN_EDITOR_SUPPORT)
            {
                var _ = new FStripDataFlags(Ar);
            }
        }

        protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer)
        {
            base.WriteJson(writer, serializer);

            if (EditorData != null && EditorData.Count > 0)
            {
                writer.WritePropertyName("EditorData");
                writer.WriteStartObject();

                foreach (var kvp in EditorData)
                {
                    string keyStr = kvp.Key?.ToString() ?? "null";
                    writer.WritePropertyName(keyStr);
                    serializer.Serialize(writer, kvp.Value);
                }

                writer.WriteEndObject();
            }
        }
    }
}
