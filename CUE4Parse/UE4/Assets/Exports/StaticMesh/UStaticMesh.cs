using System;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Objects.Engine;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Versions;
using Newtonsoft.Json;
using Serilog;

namespace CUE4Parse.UE4.Assets.Exports.StaticMesh;

public class FkDOPBounds
{
    public FVector V1;
    public FVector V2;

    public FkDOPBounds(FAssetArchive Ar)
    {
        V1 = Ar.Read<FVector>();
        V2 = Ar.Read<FVector>();
    }
}

public class FkDOPNode3
{
    public FkDOPBounds Bounds;
    public int F18;
    public short F1C;
    public short F1E;

    public FkDOPNode3(FAssetArchive Ar)
    {
        Bounds = new FkDOPBounds(Ar);

        F18 = Ar.Read<int>();

        if (Ar.Ver < EUnrealEngineObjectUE3Version.DeprecatedShortProperties || Ar.Ver > EUnrealEngineObjectUE3Version.VER_CLEANUP_SOUNDNODEWAVE)
        {
            F1C = Ar.Read<short>();
            F1E = Ar.Read<short>();
        }
        else
        {
            int tmp1C = Ar.Read<int>();
            int tmp1E = Ar.Read<int>();
            F1C = (short) tmp1C;
            F1E = (short) tmp1E;
        }
    }
}

public class UFracturedStaticMesh : UStaticMesh;

public class UStaticMesh : UObject
{
    public bool bCooked { get; private set; }
    public FPackageIndex BodySetup { get; private set; }
    public FPackageIndex NavCollision { get; private set; }
    public FGuid LightingGuid { get; private set; }
    public FPackageIndex[] Sockets { get; private set; } // UStaticMeshSocket[]
    public FStaticMeshRenderData? RenderData { get; private set; }
    public FStaticMaterial[]? StaticMaterials { get; private set; }
    public ResolvedObject?[] Materials { get; set; } // UMaterialInterface[]
    public int LODForCollision { get; private set; }

    public override void Deserialize(FAssetArchive Ar, long validPos)
    {
        if(Ar.Game == EGame.GAME_WorldofJadeDynasty) Ar.Position += 12;
        base.Deserialize(Ar, validPos);
        Materials = [];
        LODForCollision = GetOrDefault(nameof(LODForCollision), 0);

        var stripDataFlags = new FStripDataFlags(Ar);
        bCooked = Ar.Ver >= EUnrealEngineObjectUE4Version.STATIC_MESH_REFACTOR && Ar.ReadBoolean();
        var Bounds = new FBoxSphereBounds();
        if (Ar.Ver < EUnrealEngineObjectUE4Version.STATIC_MESH_REFACTOR)
        {
            Bounds = new FBoxSphereBounds(Ar);
        }

        BodySetup = new FPackageIndex(Ar);

        if(Ar.Ver < EUnrealEngineObjectUE3Version.VER_REMOVE_STATICMESH_COLLISIONMODEL)
        {
            new FPackageIndex(Ar); // CollisionModel;
        }

        if (Ar.Versions["StaticMesh.HasNavCollision"])
            NavCollision = new FPackageIndex(Ar);

        if (Ar.Game < EGame.GAME_UE4_0)
        {
            if (Ar.Ver < EUnrealEngineObjectUE3Version.VER_COMPACTKDOPSTATICMESH)
            {
                Ar.ReadBulkArray(() => new FkDOPNode3(Ar));
            }
            else
            {
                Ar.Position += 24;
                Ar.ReadBulkArray(() => Ar.ReadBytes(6)); // bound
            }

            if (Ar.Ver < EUnrealEngineObjectUE3Version.DeprecatedShortProperties || Ar.Ver > EUnrealEngineObjectUE3Version.VER_CLEANUP_SOUNDNODEWAVE)
            {
                Ar.ReadBulkArray(() => Ar.ReadBytes(8)); // Collision Triangle
            }
            else
            {
                Ar.ReadBulkArray(() => Ar.ReadBytes(16)); // Collision Triangle
            }

            var InternalVersion = Ar.Read<int>();
            var STATICMESH_VERSION_CONTENT_TAGS = 17; // Content tags were introduced in SM version 17

            if (InternalVersion >= STATICMESH_VERSION_CONTENT_TAGS && Ar.Ver < EUnrealEngineObjectUE3Version.VER_REMOVED_LEGACY_CONTENT_TAGS)
            {
                Ar.ReadArray(Ar.ReadFName);
            }

            if (Ar.Ver >= EUnrealEngineObjectUE3Version.VER_STATIC_MESH_SOURCE_DATA_COPY)
            {
                var bHaveSourceData = Ar.ReadBoolean();
                if (bHaveSourceData)
                {
                    RenderData = new FStaticMeshRenderData(Ar, true);
                }

                if (Ar.Ver < EUnrealEngineObjectUE3Version.VER_STORE_MESH_OPTIMIZATION_SETTINGS)
                {
                    Ar.ReadArray(Ar.Read<int>); // OptimizationSettings
                }
                else
                {
                    if (Ar.Ver < EUnrealEngineObjectUE3Version.VER_ADDED_EXTRA_MESH_OPTIMIZATION_SETTINGS)
                    {
                        Ar.ReadArray(() => Ar.ReadBytes(7));
                    }
                    else
                    {
                        Ar.ReadArray(() => Ar.ReadBytes(24));
                    }
                }

                Ar.Read<int>(); // bHasBeenSimplified
            }

            if (Ar.Ver >= EUnrealEngineObjectUE3Version.VER_TAG_MESH_PROXIES)
            {
                Ar.ReadBoolean(); // bIsMeshProxy
            }

            RenderData = new FStaticMeshRenderData(Ar);

            Materials = new ResolvedObject[RenderData.LODs[0].Sections.Length];
            for (var i = 0; i < RenderData.LODs[0].Sections.Length; i++)
            {
                Materials[i] = RenderData.LODs[0].Sections[i].Material!;
            }
            RenderData.Bounds = Bounds;

            Ar.ReadArray(() => new FPackageIndex(Ar));
        }

        if (!stripDataFlags.IsEditorDataStripped())
        {
            if (Ar.Ver < EUnrealEngineObjectUE4Version.DEPRECATED_STATIC_MESH_THUMBNAIL_PROPERTIES_REMOVED)
            {
                 var dummyThumbnailAngle = new FRotator(Ar);
                 if (Ar.Ver >= EUnrealEngineObjectUE3Version.VER_STATICMESH_THUMBNAIL_DISTANCE)
                 {
                     var dummyThumbnailDistance = Ar.Read<float>();
                 }
            }

            if (Ar.Ver >= EUnrealEngineObjectUE3Version.VER_STATICMESH_VERSION_18 && FRenderingObjectVersion.Get(Ar) < FRenderingObjectVersion.Type.DeprecatedHighResSourceMesh)
            {
                var Deprecated_HighResSourceMeshName = Ar.ReadFString();
                var Deprecated_HighResSourceMeshCRC = Ar.Game < EGame.GAME_UE4_0 ? Ar.Read<ulong>() : Ar.Read<uint>();
            }
        }

        if (Ar.Ver >= EUnrealEngineObjectUE3Version.VER_INTEGRATED_LIGHTMASS)
        {
            LightingGuid = Ar.Read<FGuid>(); // LocalLightingGuid
        }
        else
        {
            LightingGuid = FGuid.Random();
        }

        if (Ar.Ver >= EUnrealEngineObjectUE3Version.VER_PRESERVE_SMC_VERT_COLORS && Ar.Ver < EUnrealEngineObjectUE4Version.STATIC_MESH_REFACTOR)
        {
            Ar.Read<int>(); // VertexPositionVersionNumber
        }

        if (Ar.Ver >= EUnrealEngineObjectUE3Version.VER_DYNAMICTEXTUREINSTANCES && Ar.Ver < EUnrealEngineObjectUE4Version.REMOVE_CACHED_STATIC_MESH_STREAMING_FACTORS)
        {
            //Ar.ReadArray<int>(); // CachedStreamingTextureFactors
        }

        if (Ar.Ver >= EUnrealEngineObjectUE3Version.VER_KEEP_STATIC_MESH_DEGENERATES && Ar.Ver < EUnrealEngineObjectUE4Version.STATIC_MESH_REFACTOR)
        {
            //Ar.ReadBoolean(); // bRemoveDegenerates
        }

        if (Ar.Ver >= EUnrealEngineObjectUE3Version.VER_INSTANCED_STATIC_MESH_PER_LOD_STATIC_LIGHTING && Ar.Game < EGame.GAME_UE4_0)
        {
            //Ar.ReadBoolean(); // bPerLODStaticLightingForInstancing
            //Ar.Read<int>(); // ConsolePreallocateInstanceCount
        }

        if (Ar.Ver > EUnrealEngineObjectUE4Version.STATIC_MESH_SOCKETS)
        {
            Sockets = Ar.ReadArray(() => new FPackageIndex(Ar));
        }

        if (!Ar.IsFilterEditorOnly || Ar.Game < EGame.GAME_UE4_0)
        {
            return; // so it doesn't throw
        }

        // https://github.com/EpicGames/UnrealEngine/blob/ue5-main/Engine/Source/Runtime/Engine/Private/StaticMesh.cpp#L6701
        if (bCooked)
        {
            RenderData = Ar.Game switch
            {
                EGame.GAME_GameForPeace => new GFPStaticMeshRenderData(Ar, GetOrDefault<bool>("bIsStreamable")),
                EGame.GAME_WeHappyFew => new GFPStaticMeshRenderData(Ar, true),
                _ => RenderData = new FStaticMeshRenderData(Ar)
            };
        }

        if (Ar.Game == EGame.GAME_WutheringWaves && GetOrDefault<bool>("bUseKuroLODDistance") && Ar.ReadBoolean())
        {
            Ar.Position += 64; // 8 per-platform floats
        }

        if (bCooked && Ar.Game is >= EGame.GAME_UE4_20 and < EGame.GAME_UE5_0 && Ar.Game != EGame.GAME_DreamStar) // DS removed this for some reason
        {
            var bHasOccluderData = Ar.ReadBoolean();
            if (bHasOccluderData)
            {
                switch (Ar.Game)
                {
                    case EGame.GAME_CrystalOfAtlan:
                    case EGame.GAME_FragPunk:
                        if (Ar.Game is EGame.GAME_FragPunk && !Ar.ReadBoolean()) break;
                        Ar.SkipBulkArrayData();
                        Ar.SkipBulkArrayData();
                        Ar.SkipBulkArrayData();
                        break;
                    case EGame.GAME_Farlight84:
                    {
                        Ar.SkipBulkArrayData();
                        Ar.SkipBulkArrayData();
                        var count = Ar.Read<int>();
                        for (var i = 0; i < count; i++)
                        {
                            Ar.SkipBulkArrayData();
                            Ar.SkipBulkArrayData();
                        }
                        break;
                    }
                    default:
                        Ar.SkipFixedArray(12); // Vertices
                        Ar.SkipFixedArray(2); // Indices
                        break;
                }
            }
        }

        if (Ar.Game is EGame.GAME_FateTrigger or EGame.GAME_GhostsofTabor or EGame.GAME_Aion2) Ar.Position += 4;

        if (Ar.Ver >= EUnrealEngineObjectUE4Version.SPEEDTREE_STATICMESH)
        {
            var bHasSpeedTreeWind = Ar.ReadBoolean();
            if (bHasSpeedTreeWind)
            {
                Ar.Position = validPos;
                // return;
            }

            if (FEditorObjectVersion.Get(Ar) >= FEditorObjectVersion.Type.RefactorMeshEditorMaterials)
            {
                // UE4.14+ - "Materials" are deprecated, added StaticMaterials
                StaticMaterials = bHasSpeedTreeWind ? GetOrDefault("StaticMaterials", Array.Empty<FStaticMaterial>()) : Ar.ReadArray(() => new FStaticMaterial(Ar));

                Materials = new ResolvedObject[StaticMaterials.Length];
                for (var i = 0; i < Materials.Length; i++)
                {
                    Materials[i] = StaticMaterials[i].MaterialInterface;
                }
            }
        }
        else if (TryGetValue(out FPackageIndex[] materials, "Materials"))
        {
            Materials = new ResolvedObject[materials.Length];
            for (var i = 0; i < materials.Length; i++)
            {
                Materials[i] = materials[i].ResolvedObject!;
            }
        }

        Ar.Position += Ar.Game switch
        {
            EGame.GAME_OutlastTrials => 1,
            EGame.GAME_Farlight84 or EGame.GAME_DuneAwakening => 4,
            EGame.GAME_DaysGone => Ar.Read<int>() * 4 + 4,
            _ => 0
        };
    }

    public void OverrideMaterials(FPackageIndex[] materials)
    {
        for (var i = 0; i < materials.Length; i++)
        {
            if (i >= Materials.Length) break;
            if (materials[i].IsNull) continue;

            Materials[i] = materials[i].ResolvedObject;
        }
    }

    protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer)
    {
        base.WriteJson(writer, serializer);

        writer.WritePropertyName("BodySetup");
        serializer.Serialize(writer, BodySetup);

        writer.WritePropertyName("NavCollision");
        serializer.Serialize(writer, NavCollision);

        writer.WritePropertyName("LightingGuid");
        serializer.Serialize(writer, LightingGuid);

        writer.WritePropertyName("RenderData");
        serializer.Serialize(writer, RenderData);
    }
}
