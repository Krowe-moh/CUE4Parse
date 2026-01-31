using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CUE4Parse.MappingsProvider;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Assets.Objects;

public class FPropertyTagData
{
    public string? Name;
    public string Type;
    public string? StructType;
    public string? Module;
    public FGuid? StructGuid;
    public bool? Bool;
    public string? EnumName;
    //public bool IsEnumAsByte;
    public string? InnerType;
    public string? ValueType;
    public FPropertyTagData? InnerTypeData;
    public FPropertyTagData? ValueTypeData;
    public UStruct? Struct;
    public UEnum? Enum;

    public FPropertyTagData() { }
    internal FPropertyTagData(FAssetArchive Ar, string type, string name = "", int size = 0)
    {
        Name = name;
        Type = type;
        switch (type)
        {
            case "StructProperty":
                StructType = Ar.ReadFName().Text;
                if (Ar.Ver >= EUnrealEngineObjectUE4Version.STRUCT_GUID_IN_PROPERTY_TAG)
                    StructGuid = Ar.Read<FGuid>();
                break;
            case "BoolProperty":
                Bool = Ar.Ver < EUnrealEngineObjectUE3Version.PROPERTYTAG_BOOL_OPTIMIZATION ? Ar.ReadBoolean() : Ar.ReadFlag();
                break;
            case "ByteProperty":
            case "EnumProperty":
                if (Ar.Ver >= EUnrealEngineObjectUE3Version.BYTEPROP_SERIALIZE_ENUM)
                    EnumName = Ar.ReadFName().Text;
                break;
            case "ArrayProperty":
                if (Ar.Ver >= EUnrealEngineObjectUE4Version.ARRAY_PROPERTY_INNER_TAGS)
                    InnerType = Ar.ReadFName().Text;
                break;
            case "SetProperty":
                if (Ar.Ver >= EUnrealEngineObjectUE4Version.PROPERTY_TAG_SET_MAP_SUPPORT)
                    InnerType = Ar.ReadFName().Text;
                break;
            case "MapProperty":
                if (Ar.Ver >= EUnrealEngineObjectUE4Version.PROPERTY_TAG_SET_MAP_SUPPORT)
                {
                    InnerType = Ar.ReadFName().Text;
                    ValueType = Ar.ReadFName().Text;
                }
                break;
            case "OptionalProperty":
                InnerType = Ar.ReadFName().Text;
                break;
        }
    }

    internal FPropertyTagData(Span<FPropertyTypeNameNode> typeName, string name = "")
    {
        Name = name;
        Type = typeName.GetName();
        switch (Type)
        {
            case "BoolProperty":
                Bool = false;
                break;
            case "StructProperty":
                if (typeName.GetParameter(0) is { IsEmpty: false } structType)
                {
                    StructType = structType.GetName();
                    Module = structType.GetParameter(0).GetName();
                    // doesn't use StructGuid anyway
                    // if (typeName.GetParameter(1) is { } guid) StructGuid = new Guid(guid.GetName);
                }
                break;
            case "ByteProperty":
            case "EnumProperty":
                if (typeName.GetParameter(0) is { IsEmpty: false } enumType)
                {
                    EnumName = enumType.GetName();
                    Module = enumType.GetParameter(0).GetName();
                }
                break;
            case "ArrayProperty":
            case "SetProperty":
            case "OptionalProperty":
                if (typeName.GetParameter(0) is { IsEmpty: false } innerType)
                {
                    InnerType = innerType.GetName();
                    InnerTypeData = InnerType != "None" && innerType[0].InnerCount != 0 ? new FPropertyTagData(innerType, InnerType) : null;
                }
                break;
            case "MapProperty":
                if (typeName.GetParameter(0) is { IsEmpty: false } keyType && typeName.GetParameter(1) is { IsEmpty: false } valueType)
                {
                    InnerType = keyType.GetName();
                    InnerTypeData = InnerType != "None" && keyType[0].InnerCount != 0 ? new FPropertyTagData(keyType, InnerType) : null;
                    ValueType = valueType.GetName();
                    ValueTypeData = InnerType != "None" && valueType[0].InnerCount != 0 ? new FPropertyTagData(valueType, ValueType) : null;
                }
                break;
        }
    }

    internal FPropertyTagData(PropertyType info)
    {
        Type = info.Type;
        StructType = info.StructType;
        StructGuid = null;
        Bool = info.Bool;
        EnumName = info.EnumName;
        //IsEnumAsByte = info.IsEnumAsByte == true;
        InnerTypeData = info.InnerType != null ? new FPropertyTagData(info.InnerType) : null;
        InnerType = InnerTypeData?.Type;
        ValueTypeData = info.ValueType != null ? new FPropertyTagData(info.ValueType) : null;
        ValueType = ValueTypeData?.Type;
        Struct = info.Struct;
        Enum = info.Enum;
    }

    public static FPropertyTagData? GetArrayStructType(FAssetArchive Ar, string? name, int elementSize)
    {
        if (name is "Attachments" or "Children" or "ShadowMaps" or "WeaponFireAnim"
                or "WeaponIdleAnims" && elementSize == 4)
        {
            return new FPropertyTagData("ObjectProperty", name);
        }

        if (name == "Points" && elementSize == 4)
        {
            return new FPropertyTagData("FloatProperty", name);
        }

        var map = new Dictionary<string, string[]>
        {
            ["FloatProperty"] = new[] { "m_ClosingAlpha", "m_BufferUpgrades", "m_SpeedUpgrades", "m_OpeningAlpha", "LookupTable", "ClothMovementScale", "AttackImpactDelay", "WeaponRange", "MaxMagazineSize", "AmmoCount", "ChildBlendTimes", "MagazineSize", "fPerturbIncreaseSpeedFire", "MaxAmmoCount", "MaxMagazineSize", "AngleConstraint", "RandomSpawnPoints", "Value", "LODDistances", "LODSpawnRatios", "TargetWeight", "Child2PerBoneWeight", "ResolutionTestTable", "Weights", "InputVolume", "CurveWeights", "GearRatios" },
            ["IntProperty"] = new[] { "PawnStatsColumns", "m_RecoilControls", "DamageStatsColumns", "WeaponStatsColumns", "PlayerStatsColumns", "TeamStatsColumns", "HighlightEvents", "Constraints", "ColumnIds", "ViewIds", "m_BombUpgrades", "RandomSeeds", "IncludePaintIDs", "FireInterval", "ThrusterOffsets", "strEquipItemCode", "ShotCost", "sintheta", "costheta", "LineBuffer", "Pages", "dr", "m_BaseSpeed", "ResetChannels", "m_nHairIDs", "ClothIndexBuffer", "ClothWeldedIndices", "ClothWeldingMap", "Indices", "TrackIndices", "BoundsBodies", "FaceTriData", "SpawnOffsets", "m_CameraTrackNodes", "Index", "ClothMeshIndexData", "m_ChildBlendInTime", "CompressedTrackOffsets", "LODMaterialMap", "SupportedEvents", "InstantHitPenetratePower", "ClothingSectionInfo", "iDevotion", "ClothToGraphicsVertMap", "ValidMaterialIndices", "AffectedWheels", "AttachedVertexIndices" },
            ["EnumProperty"] = new[] { "TriangleSorting", "RandomMonsterTypes", "Formats", "WheelsToChange", "LicenseWhitelist", "LicenseBlacklist", "SubTunePriority", "WeaponFiringMode", "TargetTypes" },
            ["ByteProperty"] = new[] { "m_data", "LocalToCompReqBones", "RequiredBones", "RawData", "Types", "SystemMemoryData" },
            ["BoolProperty"] = new[] { "bEnableShadowCasting", "bForceFireAnimLinkages", "bUseClientsideFireLogic" },
            ["StrProperty"] = new[] { "m_Messages", "ModuleMenu_ModuleRejections", "InvalidObjNames", "m_HelpText", "m_HelpTitle", "KilledNoEnemies", "UsedNoSlowmo", "DroppedNoBombs", "WorstScoreEver", "PreviousScoreWasBetter", "BeatPreviousScore", "BeatLocalHighscore", "m_ListStrings", "ParamNames", "BossLevels", "NestedStringArray", "StringArrayNeedCtor", "BoneNames", "HiddenLevels", "LibraryCategories", "BossArchetypeNames", "TemplateMapFolders", "ReferencedSwfs", "RequireZoneNames", "String", "UseActions", "HelpParamDescriptions", "HelpParamNames", "StyleGroups", "Commands", "EditPackages", "PackagesToBeFullyLoadedAtStartup" },
            ["NameProperty"] = new[] { "PacakgesToPreDecompress", "TrackControllerName", "BranchStartBoneName", "ForbiddenKeys", "PerPolyCollisionBones", "TargetCategories", "PathList", "AdditionalEyeSocketNames", "GodModePickupNames", "FiringStatesArray", "m_nmRecoilControlNames", "m_DisableBlinkingAnimNodeNameList", "PawnStatus", "KeyNames", "ClothBones", "DisabledEventAliases", "RolesToApplyOverlay", "UnfixedNames", "EnabledSpringNames", "HideBoneNames", "MaterialParameters", "IgnoreEyeSocketNames", "vUseSkillName", "InputNames", "ClothingTeleportRefBones", "AnimTypes", "ComposePrePassBoneNames", "LevelNames", "ArcheTypeNames", "FireModeTypes", "ChildClassNames", "UseTranslationBoneNames", "m_ControlledNodeNames", "MonsterTypes", "ArenaNames", "BendyBoneControllers", "AcceptableDamageTypes", "CompleteActionPressList", "IgnoreActionPressList", "BoostEmitterSockets", "ComponentLayers", "DeleteCategories", "SavedGameFileNames", "TargetParamNames", "MorphNames", "AnimList", "TrackBoneNames", "CompleteActionIgnorePressList", "ActionNames", "PressedKeys", "ValidAssociatedBones", "PrioritizedSkelBranches", "HiddenKismetClassNames", "BadPackageNames", "AlwaysShowInSelectedPlatforms", "StartActionPressList", "FailedActionPressList", "RegularPostMatchCelebrationAnims", "StrengthAnimNodeNameList" },
            ["ObjectProperty"] = new[] { "WeaponFireSnd1p", "FireCameraAnim", "WeaponFireAnim", "ForceMaterials", "ArmFireAnim", "PawnClassArray", "DefaultSoundCue", "Sounds", "DefaultInventory", "SubObjectHealths", "ImpactPS", "ExpansionVolumes", "ArchetypeProximityTypes", "FragmentMeshes", "LifetimePolicies", "SMComponents", "EquipmentInfoSet", "PST_StuckSpears", "PreDeath", "PostDeath", "WeightmapTextures", "LandscapeComponents", "Expressions", "m_AnimSets1p", "Meshes", "m_Elements", "m_PromoWeapons", "m_DefaultWeapons", "m_EntityList", "Elements", "EquipMaterials", "ChildParts", "GameTypesSupportedOnThisMap", "TreeNodes", "RoundCountDownSoundList", "DecalLeftMaterials", "ParticleTemplate", "OverideMaterials", "MeleeAttackAnim", "Attributes", "AttachAll", "CountDownSoundList", "AbilityModifiers", "DecalRightMaterials", "AdditionalWeaponAnimset", "AlternateMaterials", "PawnWeapAnim_Attack", "DetachAny", "SpawnPoints", "AllSlots", "AdditionalWeapon", "WeaponProjectiles", "EquipMorphTargets", "Behaviors", "AttachAny", "Goals", "RuntimeParameters", "NoiseModifiers", "PropertyModifiers", "WeaponFireSnd", "WeaponFireSnd_3P", "EffectSockets", "Items", "Procedures", "SourceMaterials", "Profiles", "Modifiers", "LevelThresholds", "AttachedTuningItems", "UnsupportedPaints", "ExcludePaints", "ShopExclusivePaints", "IncludePaints", "Overrides", "ObjList", "MultipleBoostsOverride", "Features", "StreamingLevels", "Sockets", "FaceFXAnimSets", "RefreshSubscriberNotifies", "Conditions", "PowerupDamageModifiers", "FadeMaterials", "TranslucentMaterials", "FragmentArches", "FireCrackerExplosionTemplates", "ComponentDynArray", "MorphSets", "WeaponForeHandgripAnimSets", "LinksFrom", "SpecialEditions", "DamageEvents", "DecalMaterials", "ModifiedComponent_Array", "ModifiedChild_Defined_Array", "UnmodifiedChild_Defined_Array", "Traits", "WhitelistProducts", "ChargeSFX", "ClientDestroyedActorContent", "m_affectedUsableObjects", "SilhouettePrimitives", "ActionsToExecute", "CrowdActors", "PaintableMaterials", "ProhitibitedDefaultMatchTypes", "AdditiveBasePoseAnimSeq", "AdditiveTargetPoseAnimSeq", "SubTracks", "DefaultMaterials", "ParticleModuleEventsToSendToGame", "OverridesToApply", "TeamArchetypes", "ArmsMesh_Common_Animsets", "StatModComponents", "InventoryList", "ClothingAssets", "FemaleAnimSets", "DamageTypes", "IgnoreDamageTypes", "LinkedEvents", "Decorators", "DefaultCarComponents", "EyeMaterials", "ClassProximityTypes", "MaleAnimSets", "RelatedAdditiveAnimSeqs", "SkelControls", "ConstraintSetup", "BoundsBodies", "ClothingAssets", "SpawnPointLobbyTeams", "SpawnTeams", "HeadMorphTargets", "Expression", "EditorComments", "MeshMaterials", "MaleLobbyAnimSets", "FemaleLobbyAnimSets", "HandSignalAnims", "WeaponProjectilesAT", "SoundPacks", "Bots2", "OverrideStates", "BloodSplatterDecalMaterial", "StaticMeshComponents", "FunctionExpressions", "LightComponents", "MetaData", "RootMorphNodes", "AnimTickArray", "StatCategories", "Titles", "m_FemaleGroupAnimSets", "ParentNodes", "LinkedVariables", "InterpTracks", "InterpGroups", "BodySetup", "Bodies", "Styles", "CollisionComponents", "BlacklistProducts", "ImportantStatEvents", "m_ArchetypeTemplates", "InactiveStatMorphTargetSetes", "Flashlight_MeshComponents", "Flashlight_FlareComponents", "Flashlight_FlareSockets", "ParticleSystemComponents", "ExplosionComponents", "m_HealthChildren", "Wheels", "Flashlight_LightSockets", "FlickerFunctionArchetypes", "GroupAnimSets", "Materials", "StreamingLevels", "AllStatEvents", "CountdownMessages", "GameStates", "m_PhysicsChildren", "AchievementIcons", "ConnectorSockets", "ChildProFXTextures", "ChildNodes", "Sequences", "ReferencedObjects", "InvisiTekMaterials", "GameTypesToUseDefaultProduct", "OffsetNodes2", "References", "Textures", "RemovedArchetypes", "PrefabArchetypes", "Attached", "SequenceObjects", "LFMaterials", "BeamSystems", "ReplacingBotCountdownMessages", "PlayerNodes", "DecalList", "Anim", "ControlHead", "AnimSets", "Components", "Modules", "Targets", "Controls", "SpawnModules", "MapSetsToUseDefaultProduct", "Mutators", "UserReferences", "Flashlight_LightComponents", "UpdateModules", "Emitters", "LODLevels", "ReplayClips", "Skins", "Effects", "ReferencedTextures", "OverrideCarComponents", "SpawnPoints", "DefaultMutators", "c_TurnInPlaceNodes", "AttackSnd", "DeathSnd", "HitSnd", "IdleSnd", "KnockedSnd", "PushedAwaySnd", "WakeUpSnd" },
            ["ClassProperty"] = new[] { "WeaponClasses", "PreviewAnimSets", "m_ExcludedClasses", "m_LeaderboardStatClass", "SupportSynchronizeClasses" },
            ["StructProperty"] = new[] { "TextureParameterValues", "ScalarParameterValues", "VectorParameterValues" }
        };

        if (name != null && map.Any(kv => kv.Value.Contains(name)))
        {
            var key = map.First(kv => kv.Value.Contains(name)).Key;
            return new FPropertyTagData(key, name);
        }

        var amap = new Dictionary<string, string[]>
        {
            ["Vector"] = new[] { "ClothMeshPosData", "ClothMeshNormalData", "FaceNormalDirections", "SourceOffsetDefaults", "ClothMeshNormalData", "PreCachedPhysScale", "EdgeDirections", "VertexData", "SpawnOffset", "PosKeys", "m_OpeningPosition", "m_ClosingPosition" },
            ["Quat"] = new[] { "RotKeys" },
            ["Rotator"] = new[] { "DecalRotation", "SpawnFacing" },
            ["Plane"] = new[] { "FacePlaneData", "PermutedVertexData" },
            ["LinearColor"] = new[] { "LightComplexityColors", "DefaultColorList", "ColorBlindColorList", "m_StateBGColors", "m_StateFontColors", "IconColor" },
            ["Guid"] = new[] { "IrrelevantLights", "ReferencedTextureGuids" },
            ["FontCharacter"] = new[] { "Characters" },
        };

        if (name != null && amap.Any(kv => kv.Value.Contains(name)))
        {
            var key = amap.First(kv => kv.Value.Contains(name)).Key;
            return new FPropertyTagData("StructProperty", key);
        }
        var propertyMaps = new Dictionary<string, HashSet<string>>
        {
            ["ByteProperty"]   = new(),
            ["FloatProperty"]  = new(),
            ["IntProperty"]    = new(),
            ["EnumProperty"]   = new(),
            ["BoolProperty"]   = new(),
            ["StrProperty"]    = new(),
            ["NameProperty"]   = new(),
            ["ClassProperty"]  = new(),
            ["ObjectProperty"] = new(),
        };

        for (var i = 0; i < Ar.Owner.ExportMapLength; i++)
        {
            var pointer = new FPackageIndex(Ar.Owner, i + 1).ResolvedObject;
            if (pointer is null)
                continue;

            var className = pointer.Class?.Object?.Value?.Name;
            if (className is null)
                continue;

            if (!propertyMaps.TryGetValue(className, out var set))
                continue;

            set.Add(pointer.Name.Text);
        }

        foreach (var kvp in propertyMaps)
        {
            if (kvp.Value.Contains(name))
                return new FPropertyTagData(kvp.Key, name);
        }


        return new FPropertyTagData("StructProperty", name);
    }

    internal FPropertyTagData(string structType, string name = "")
    {
        Name = name;
        Type = "StructProperty";
        StructType = structType;
    }

    public override string ToString()
    {
        var sb = new StringBuilder(Type);
        switch (Type)
        {
            case "StructProperty":
                sb.AppendFormat("<{0}>", StructType);
                break;
            case "ByteProperty" when EnumName != null:
            case "EnumProperty":
                sb.AppendFormat("<{0}>", EnumName);
                break;
            case "ArrayProperty":
            case "SetProperty":
            case "OptionalProperty":
                sb.AppendFormat("<{0}>", InnerTypeData?.ToString() ?? InnerType);
                break;
            case "MapProperty":
                sb.AppendFormat("<{0}, {1}>", InnerTypeData?.ToString() ?? InnerType, ValueTypeData?.ToString() ?? ValueType);
                break;
        }

        return sb.ToString();
    }
}
