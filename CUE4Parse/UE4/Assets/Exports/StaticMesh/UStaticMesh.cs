using System;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Objects.Engine;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Versions;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.StaticMesh;

public class UStaticMesh : UObject
{
    public bool bCooked { get; private set; }
    public FPackageIndex BodySetup { get; private set; }
    public FPackageIndex NavCollision { get; private set; }
    public FGuid LightingGuid { get; private set; }
    public FPackageIndex[] Sockets { get; private set; } // UStaticMeshSocket[]
    public FStaticMeshRenderData? RenderData { get; private set; }
    public FStaticMaterial[]? StaticMaterials { get; private set; }
    public ResolvedObject?[] Materials { get; private set; } // UMaterialInterface[]
    public int LODForCollision { get; private set; }

    public override void Deserialize(FAssetArchive Ar, long validPos)
    {
        base.Deserialize(Ar, validPos);
        Materials = [];
        LODForCollision = GetOrDefault(nameof(LODForCollision), 0);

        var stripDataFlags = Ar.Read<FStripDataFlags>();
        bCooked = Ar.ReadBoolean();
        if (Ar.Game == EGame.GAME_Farlight84) Ar.Position += 1; // Extra byte?
        BodySetup = new FPackageIndex(Ar);

        if (Ar.Versions["StaticMesh.HasNavCollision"])
            NavCollision = new FPackageIndex(Ar);

        if (!stripDataFlags.IsEditorDataStripped())
        {
             if (Ar.Ver < EUnrealEngineObjectUE4Version.DEPRECATED_STATIC_MESH_THUMBNAIL_PROPERTIES_REMOVED)
             {
                 var dummyThumbnailAngle = new FRotator(Ar);
                 var dummyThumbnailDistance = Ar.Read<float>();
             }

             if (FRenderingObjectVersion.Get(Ar) < FRenderingObjectVersion.Type.DeprecatedHighResSourceMesh)
             {
                 var highResSourceMeshName = Ar.ReadFString();
                 var highResSourceMeshCRC = Ar.Read<uint>();
             }
        }

        LightingGuid = Ar.Read<FGuid>(); // LocalLightingGuid
        Sockets = Ar.ReadArray(() => new FPackageIndex(Ar));

        // https://github.com/EpicGames/UnrealEngine/blob/ue5-main/Engine/Source/Runtime/Engine/Private/StaticMesh.cpp#L6701
        if (bCooked)
        {
            RenderData = Ar.Game switch
            {
                EGame.GAME_GameForPeace => new GFPStaticMeshRenderData(Ar, GetOrDefault<bool>("bIsStreamable")),
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
                if ((Ar.Game is EGame.GAME_FragPunk && Ar.ReadBoolean()) || Ar.Game is EGame.GAME_CrystalOfAtlan)
                {
                    Ar.SkipBulkArrayData();
                    Ar.SkipBulkArrayData();
                    if (Ar.Game is EGame.GAME_CrystalOfAtlan) Ar.SkipBulkArrayData();
                }
                else
                {
                    Ar.SkipFixedArray(12); // Vertices
                    Ar.SkipFixedArray(2); // Indices
                }

            }
        }

        if (Ar.Game == EGame.GAME_FateTrigger) Ar.Position += 4;

        if (Ar.Game >= EGame.GAME_UE4_14)
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
                StaticMaterials = bHasSpeedTreeWind ? GetOrDefault("StaticMaterials",  Array.Empty<FStaticMaterial>()) : Ar.ReadArray(() => new FStaticMaterial(Ar));

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
            EGame.GAME_DaysGone => Ar.Read<int>() * 4,
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
