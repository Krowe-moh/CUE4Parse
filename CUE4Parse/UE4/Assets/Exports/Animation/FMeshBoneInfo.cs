using CUE4Parse.ActorX;
using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.Animation
{
    [JsonConverter(typeof(FMeshBoneInfoConverter))]
    public struct FMeshBoneInfo
    {
        public readonly FName Name;
        public readonly int ParentIndex;
        public readonly VJointPosPsk Pos;
        public FMeshBoneInfo(FArchive Ar)
        {
            Name = Ar.ReadFName();
            if (Ar.Game < EGame.GAME_UE4_0)
            {
                Ar.Read<int>();
                Pos = new VJointPosPsk(Ar);
                Ar.Read<int>();
            }
            ParentIndex = Ar.Read<int>();
            if (Ar.Ver >= EUnrealEngineObjectUE3Version.VER_SKELMESH_DRAWSKELTREEMANAGER && Ar.Game < EGame.GAME_UE4_0)
            {
                Ar.Read<int>();
            }

            if (Ar.Game >= EGame.GAME_UE4_0 && Ar.Ver < EUnrealEngineObjectUE4Version.REFERENCE_SKELETON_REFACTOR)
            {
                Ar.Read<FColor>();
            }
        }

        public FMeshBoneInfo(FName name, int parentIndex)
        {
            Name = name;
            ParentIndex = parentIndex;
        }

        public override string ToString() => $"{Name}";
    }
}
