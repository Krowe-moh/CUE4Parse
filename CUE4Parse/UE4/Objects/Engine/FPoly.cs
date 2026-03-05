using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Objects.Engine;

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
        VertexCount = Ar.Ver < EUnrealEngineObjectUE3Version.FPOLYVERTEXARRAY ? Ar.Read<int>() : -1;

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

        PolyFlags = (uint) Ar.Read<int>();
        if (Ar.Ver < EUnrealEngineObjectUE3Version.MOVED_EXPORTIMPORTMAPS_ADDED_TOTALHEADERSIZE) PolyFlags |= 0xe00;
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

        if (Ar.Ver >= EUnrealEngineObjectUE3Version.LightMapScaleAddedToPoly && Ar.Ver < EUnrealEngineObjectUE3Version.TWOSIDEDSIGN_PARAMETERS)
        {
            LightMapScale = Ar.Read<float>();
        }

        if (Ar.Ver >= EUnrealEngineObjectUE3Version.TWOSIDEDSIGN_PARAMETERS)
        {
            ShadowMapScale = Ar.Read<float>();
        }

        if (Ar.Ver >= EUnrealEngineObjectUE3Version.BSP_LIGHTING_CHANNEL_SUPPORT)
        {
            LightingChannels = new LightingChannelContainer(Ar);
        }

        if (Ar.Ver >= EUnrealEngineObjectUE3Version.INTEGRATED_LIGHTMASS)
        {
            LightmassSettings = new LightmassPrimitiveSettings(Ar);
        }

        if (Ar.Ver < EUnrealEngineObjectUE3Version.FPOLY_RULESET_VARIATIONNAME)
        {
            if (Ar.Ver >= EUnrealEngineObjectUE3Version.ADD_FPOLY_PBRULESET_POINTER)
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
