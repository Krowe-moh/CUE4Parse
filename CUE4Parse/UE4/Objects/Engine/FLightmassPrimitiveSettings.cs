﻿using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Objects.Engine
{
    public readonly struct FLightmassPrimitiveSettings : IUStruct
    {
        public readonly bool bUseTwoSidedLighting;
        public readonly bool bShadowIndirectOnly;
        public readonly bool bUseEmissiveForStaticLighting;
        public readonly bool bUseVertexNormalForHemisphereGather;
        public readonly float EmissiveLightFalloffExponent;
        public readonly float EmissiveLightExplicitInfluenceRadius;
        public readonly float EmissiveBoost;
        public readonly float DiffuseBoost;
        public readonly float SpecularBoost;
        public readonly float FullyOccludedSamplesFraction;

        public FLightmassPrimitiveSettings(FArchive Ar)
        {
            if (Ar.Ver >= EUnrealEngineObjectUE3Version.VER_SHADOW_INDIRECT_ONLY_OPTION)
            {
                bUseTwoSidedLighting = Ar.ReadBoolean();
                bShadowIndirectOnly = Ar.ReadBoolean();
                FullyOccludedSamplesFraction = Ar.Read<float>();
            }

            if (Ar.Ver >= EUnrealEngineObjectUE3Version.VER_INTEGRATED_LIGHTMASS)
            {
                bUseEmissiveForStaticLighting = Ar.ReadBoolean();
                bUseVertexNormalForHemisphereGather = Ar.Ver >= EUnrealEngineObjectUE4Version.NEW_LIGHTMASS_PRIMITIVE_SETTING ? Ar.ReadBoolean() : false;
                EmissiveLightFalloffExponent = Ar.Read<float>();
            }

            if (Ar.Ver >= EUnrealEngineObjectUE3Version.VER_ADDDED_EXPLICIT_EMISSIVE_LIGHT_RADIUS)
            {
                EmissiveLightExplicitInfluenceRadius = Ar.Read<float>();
            }

            EmissiveBoost = Ar.Read<float>();
            DiffuseBoost = Ar.Read<float>();
            if (Ar.Game < EGame.GAME_UE4_0)
            {
                SpecularBoost = Ar.Read<float>();
            }
        }
    }
}