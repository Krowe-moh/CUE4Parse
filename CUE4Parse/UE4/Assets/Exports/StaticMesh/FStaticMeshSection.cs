using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.StaticMesh;

public class FPS3StaticMeshData
{
    public int[] IoBufferSize;
    public int[] ScratchBufferSize;
    public short[] CommandBufferHoleSize;
    public short[] IndexBias;
    public short[] VertexCount;
    public short[] TriangleCount;
    public short[] FirstVertex;
    public short[] FirstTriangle;

    public FPS3StaticMeshData(FArchive Ar)
    {
        IoBufferSize = Ar.ReadArray<int>();
        ScratchBufferSize = Ar.ReadArray<int>();
        CommandBufferHoleSize = Ar.ReadArray<short>();
        IndexBias = Ar.ReadArray<short>();
        VertexCount = Ar.ReadArray<short>();
        TriangleCount = Ar.ReadArray<short>();
        FirstVertex = Ar.ReadArray<short>();
        FirstTriangle = Ar.ReadArray<short>();
    }
}

[JsonConverter(typeof(FStaticMeshSectionConverter))]
public class FStaticMeshSection
{
    public ResolvedObject? Material;
    public int MaterialIndex;
    public int FirstIndex;
    public int NumTriangles;
    public int MinVertexIndex;
    public int MaxVertexIndex;
    public bool bEnableCollision;
    public bool bCastShadow;
    public bool bForceOpaque;
    public bool bVisibleInRayTracing;
    public bool bAffectDistanceFieldLighting;
    public int? CustomData;

    public FStaticMeshSection(FArchive Ar)
    {
        if (Ar.Game < EGame.GAME_UE4_0)
        {
            Material = new FPackageIndex((FAssetArchive)Ar).ResolvedObject;
            bEnableCollision = Ar.ReadBoolean();
            Ar.ReadBoolean(); // OldEnableCollision
            if (Ar.Ver >= EUnrealEngineObjectUE3Version.AddedCastShadow) bCastShadow = Ar.ReadBoolean();
            FirstIndex = Ar.Read<int>();
            NumTriangles = Ar.Read<int>();
            MinVertexIndex = Ar.Read<int>();
            MaxVertexIndex = Ar.Read<int>();
            if (Ar.Ver >= EUnrealEngineObjectUE3Version.VER_STATICMESH_VERSION_16) MaterialIndex = Ar.Read<int>();
            if (Ar.Ver >= EUnrealEngineObjectUE3Version.VER_STATICMESH_FRAGMENTINDEX) Ar.SkipFixedArray(8);
            if (Ar.Ver >= EUnrealEngineObjectUE3Version.VER_ADDED_PLATFORMMESHDATA)
            {
                var bLoadPlatformData = Ar.ReadFlag();
                if (bLoadPlatformData)
                {
                    new FPS3StaticMeshData(Ar);
                }
            }
            return;
        }

        MaterialIndex = Ar.Read<int>();
        FirstIndex = Ar.Read<int>();
        NumTriangles = Ar.Read<int>();
        MinVertexIndex = Ar.Read<int>();
        MaxVertexIndex = Ar.Read<int>();
        bEnableCollision = Ar.ReadBoolean();
        bCastShadow = Ar.ReadBoolean();
        if (Ar.Game == EGame.GAME_PlayerUnknownsBattlegrounds) Ar.Position += 5; // byte + int
        bForceOpaque = FRenderingObjectVersion.Get(Ar) >= FRenderingObjectVersion.Type.StaticMeshSectionForceOpaqueField && Ar.ReadBoolean();
        if (Ar.Game == EGame.GAME_MortalKombat1) Ar.Position += 8; // "None" FName
        if (Ar.Game == EGame.GAME_BlueProtocol) CustomData = Ar.Read<short>(); // Must be read before bVisibleInRayTracing
        bVisibleInRayTracing = !Ar.Versions["StaticMesh.HasVisibleInRayTracing"] || Ar.ReadBoolean();
        if (Ar.Game is EGame.GAME_Grounded or EGame.GAME_Dauntless) Ar.Position += 8;
        bAffectDistanceFieldLighting = Ar.Game >= EGame.GAME_UE5_1 && Ar.ReadBoolean();
        if (Ar.Game is EGame.GAME_RogueCompany or EGame.GAME_Grounded or EGame.GAME_Grounded2 or EGame.GAME_RacingMaster
            or EGame.GAME_MetroAwakening or EGame.GAME_Avowed or EGame.GAME_OutlastTrials) Ar.Position += 4;
        if (Ar.Game is EGame.GAME_InfinityNikki)
        {
            CustomData = Ar.Read<int>();
            Ar.Position += 8;
        }
    }
}
