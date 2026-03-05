using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Objects.Engine
{
    public struct LightmassPrimitiveSettings
    {
        public bool UseTwoSidedLighting;
        public bool ShadowIndirectOnly;
        public bool UseEmissiveForStaticLighting;
        public float EmissiveLightFalloffExponent;
        public float EmissiveLightExplicitInfluenceRadius;
        public float EmissiveBoost;
        public float DiffuseBoost;
        public float SpecularBoost;
        public float FullyOccludedSamplesFraction;

        public LightmassPrimitiveSettings(FAssetArchive Ar)
        {
            if (Ar.Ver >= EUnrealEngineObjectUE3Version.SHADOW_INDIRECT_ONLY_OPTION)
            {
                UseTwoSidedLighting = Ar.ReadBoolean();
                ShadowIndirectOnly = Ar.ReadBoolean();
                FullyOccludedSamplesFraction = Ar.Read<float>();
            }

            if (Ar.Ver >= EUnrealEngineObjectUE3Version.INTEGRATED_LIGHTMASS)
            {
                UseEmissiveForStaticLighting = Ar.ReadBoolean();
                EmissiveLightFalloffExponent = Ar.Read<float>();
            }

            if (Ar.Ver >= EUnrealEngineObjectUE3Version.ADDDED_EXPLICIT_EMISSIVE_LIGHT_RADIUS)
            {
                EmissiveLightExplicitInfluenceRadius = Ar.Read<float>();
            }

            EmissiveBoost = Ar.Read<float>();
            DiffuseBoost = Ar.Read<float>();
            SpecularBoost = Ar.Read<float>();
        }
    }
    public struct LightingChannelContainer
    {
        public byte Initialized;
        public byte BSP;
        public byte Static;
        public byte Dynamic;

        public LightingChannelContainer(FAssetArchive Ar)
        {
            Initialized = Ar.Read<byte>();
            BSP = Ar.Read<byte>();
            Static = Ar.Read<byte>();
            Dynamic = Ar.Read<byte>();
        }
    }

    public class UPolys : Assets.Exports.UObject
    {
        public int Num;
        public int Max;
        public FPackageIndex ElementOwner;
        public FPoly[] Element;

        public override void Deserialize(FAssetArchive Ar, long validPos)
        {
            base.Deserialize(Ar, validPos);

            if (Ar.Ver < EUnrealEngineObjectUE4Version.BSP_UNDO_FIX)
            {
                Num = Ar.Read<int>();
                Max = Ar.Read<int>();

                if (Ar.Ver >= EUnrealEngineObjectUE3Version.SERIALIZE_TTRANSARRAY_OWNER)
                {
                    ElementOwner = new FPackageIndex(Ar);
                }
            }

            Element = Ar.ReadArray(Num, () => new FPoly(Ar));
        }
    }
}
