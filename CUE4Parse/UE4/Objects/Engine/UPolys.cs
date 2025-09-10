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
            if (Ar.Ver >= EUnrealEngineObjectUE3Version.VER_SHADOW_INDIRECT_ONLY_OPTION)
            {
                UseTwoSidedLighting = Ar.ReadBoolean();
                ShadowIndirectOnly = Ar.ReadBoolean();
                FullyOccludedSamplesFraction = Ar.Read<float>();
            }

            if (Ar.Ver >= EUnrealEngineObjectUE3Version.VER_INTEGRATED_LIGHTMASS)
            {
                UseEmissiveForStaticLighting = Ar.ReadBoolean();
                EmissiveLightFalloffExponent = Ar.Read<float>();
            }

            if (Ar.Ver >= EUnrealEngineObjectUE3Version.VER_ADDDED_EXPLICIT_EMISSIVE_LIGHT_RADIUS)
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
    public class FPoly
    {
        public int VertexCount;
        public FVector Base;
        public FVector Normal;
        public FVector TextureU;
        public FVector TextureV;
        public FVector[] Vertex;
        public uint PolyFlags;
        public FPackageIndex Actor;
        public FPackageIndex Material;
        public FName ItemName;
        public int Link;
        public int BrushPoly;
        public short PanU;
        public short PanV;
        public float LightMapScale;
        public float ShadowMapScale;
        public LightingChannelContainer LightingChannels;
        public LightmassPrimitiveSettings LightmassSettings;
        public FName RulesetVariation;
        public FPackageIndex Ruleset;
        
        public FPoly(FAssetArchive Ar)
        {
            VertexCount = Ar.Ver < EUnrealEngineObjectUE3Version.VER_FPOLYVERTEXARRAY ? Ar.Read<int>() : -1;

            Base = Ar.Read<FVector>();
            Normal = Ar.Read<FVector>();
            TextureU = Ar.Read<FVector>();
            TextureV = Ar.Read<FVector>();

            if (VertexCount == -1)
            {
                Vertex = Ar.ReadArray(() => Ar.Read<FVector>());
            }
            else
            {
                Vertex = Ar.ReadArray(VertexCount, () => Ar.Read<FVector>());
            }

            PolyFlags = (uint)Ar.Read<int>();
            if (Ar.Ver < EUnrealEngineObjectUE3Version.VER_MOVED_EXPORTIMPORTMAPS_ADDED_TOTALHEADERSIZE) PolyFlags |= 0xe00;
            Actor = new FPackageIndex(Ar);

            if (Ar.Ver < EUnrealEngineObjectUE3Version.TextureDeprecatedFromPoly)
            {
                Material = new FPackageIndex(Ar);
            }

            ItemName = Ar.ReadFName();

            if (Ar.Ver >= EUnrealEngineObjectUE3Version.TextureDeprecatedFromPoly)
            {
                Material = new FPackageIndex(Ar);
            }

            Link = Ar.Read<int>();
            BrushPoly = Ar.Read<int>();

            if (Ar.Ver < EUnrealEngineObjectUE3Version.PanUVRemovedFromPoly)
            {
                PanU = Ar.Read<short>();
                PanV = Ar.Read<short>();
            }

            if (Ar.Ver >= EUnrealEngineObjectUE3Version.LightMapScaleAddedToPoly && Ar.Ver < EUnrealEngineObjectUE3Version.VER_TWOSIDEDSIGN_PARAMETERS)
            {
                LightMapScale = Ar.Read<float>();
            }

            if (Ar.Ver >= EUnrealEngineObjectUE3Version.VER_TWOSIDEDSIGN_PARAMETERS)
            {
                ShadowMapScale = Ar.Read<float>();
            }

            if (Ar.Ver >= EUnrealEngineObjectUE3Version.VER_BSP_LIGHTING_CHANNEL_SUPPORT)
            {
                LightingChannels = new LightingChannelContainer(Ar);
            }

            if (Ar.Ver >= EUnrealEngineObjectUE3Version.VER_INTEGRATED_LIGHTMASS)
            {
                LightmassSettings = new LightmassPrimitiveSettings(Ar);
            }

            if (Ar.Ver < EUnrealEngineObjectUE3Version.VER_FPOLY_RULESET_VARIATIONNAME)
            {
                if (Ar.Ver >= EUnrealEngineObjectUE3Version.VER_ADD_FPOLY_PBRULESET_POINTER)
                {
                    Ruleset = new FPackageIndex(Ar);
                }
            }
            else
            {
                RulesetVariation = Ar.ReadFName();
            }
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

            Num = Ar.Read<int>();
            Max = Ar.Read<int>();

            if (Ar.Ver >= EUnrealEngineObjectUE3Version.VER_SERIALIZE_TTRANSARRAY_OWNER)
            {
                ElementOwner = new FPackageIndex(Ar);
            }
            
            Element = Ar.ReadArray(Num, () => new FPoly(Ar));
        }
    }
}
