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
                if (Ar.Ver < EUnrealEngineObjectUE3Version.VER_PROPERTYTAG_BOOL_OPTIMIZATION)
                {
                    Bool = Ar.ReadBoolean();
                }
                else
                {
                    Bool = Ar.ReadFlag();
                }

                break;
            case "ByteProperty":
            case "EnumProperty":
                if (Ar.Ver >= EUnrealEngineObjectUE3Version.VER_BYTEPROP_SERIALIZE_ENUM)
                {
                    EnumName = Ar.ReadFName().Text;
                }

                break;
            case "ArrayProperty":
                if (Ar.Ver >= EUnrealEngineObjectUE4Version.ARRAY_PROPERTY_INNER_TAGS)
                {
                    InnerType = Ar.ReadFName().Text;
                }
                else
                {
                    // temp manual mappings
                    var map = new Dictionary<string, string[]>
                    {
                        ["FloatProperty"] = new[]
                        {
                            "LookupTable", "ClothMovementScale", "AttackImpactDelay", "WeaponRange", "MaxMagazineSize", "AmmoCount", "ChildBlendTimes", "MagazineSize",
                            "fPerturbIncreaseSpeedFire", "MaxAmmoCount", "MaxMagazineSize", "AngleConstraint", "RandomSpawnPoints", "Value", "LODDistances",
                            "LODSpawnRatios", "TargetWeight", "Child2PerBoneWeight", "ResolutionTestTable", "Weights", "InputVolume", "CurveWeights", "GearRatios"
                        },
                        ["IntProperty"] = new[]
                        {
                            "Constraints", "RandomSeeds", "IncludePaintIDs", "FireInterval", "ThrusterOffsets", "strEquipItemCode", "ShotCost", "sintheta", "costheta", "LineBuffer", "Pages", "dr", "m_BaseSpeed", "ResetChannels", "m_nHairIDs",
                            "ClothIndexBuffer", "ClothWeldedIndices", "ClothWeldingMap", "Indices", "TrackIndices", "BoundsBodies", "FaceTriData", "SpawnOffsets", "m_CameraTrackNodes", "Index", "ClothMeshIndexData", "m_ChildBlendInTime",
                            "CompressedTrackOffsets", "LODMaterialMap", "SupportedEvents", "InstantHitPenetratePower", "ClothingSectionInfo", "iDevotion", "ClothToGraphicsVertMap", "ValidMaterialIndices", "AffectedWheels", "AttachedVertexIndices"
                        },
                        ["EnumProperty"] = new[] { "WheelsToChange", "LicenseWhitelist", "LicenseBlacklist", "SubTunePriority", "WeaponFiringMode", "TargetTypes" },
                        ["ByteProperty"] = new[] { "m_data", "LocalToCompReqBones", "RequiredBones", "RawData", "Types", "SystemMemoryData" },
                        ["BoolProperty"] = new[] { "bEnableShadowCasting", "bForceFireAnimLinkages", "bUseClientsideFireLogic" },
                        ["StrProperty"] = new[]
                        {
                            "ParamNames", "BossLevels", "NestedStringArray", "StringArrayNeedCtor", "BoneNames", "HiddenLevels", "LibraryCategories",
                            "BossArchetypeNames", "TemplateMapFolders", "ReferencedSwfs", "RequireZoneNames", "String", "UseActions",
                            "HelpParamDescriptions", "HelpParamNames", "StyleGroups", "Commands", "EditPackages", "PackagesToBeFullyLoadedAtStartup"
                        },
                        ["NameProperty"] = new[]
                        {
                            "BranchStartBoneName", "PerPolyCollisionBones", "TargetCategories", "PathList", "AdditionalEyeSocketNames", "GodModePickupNames", "FiringStatesArray", "m_DisableBlinkingAnimNodeNameList",
                            "PawnStatus", "KeyNames", "ClothBones", "DisabledEventAliases", "RolesToApplyOverlay", "UnfixedNames", "EnabledSpringNames", "HideBoneNames", "MaterialParameters", "IgnoreEyeSocketNames", "vUseSkillName",
                            "InputNames", "ClothingTeleportRefBones", "AnimTypes", "ComposePrePassBoneNames", "LevelNames", "ArcheTypeNames", "FireModeTypes", "ChildClassNames", "UseTranslationBoneNames", "m_ControlledNodeNames",
                            "MonsterTypes", "ArenaNames", "BendyBoneControllers", "AcceptableDamageTypes", "CompleteActionPressList", "IgnoreActionPressList", "BoostEmitterSockets", "ComponentLayers", "DeleteCategories",
                            "SavedGameFileNames", "TargetParamNames", "MorphNames", "AnimList", "TrackBoneNames", "CompleteActionIgnorePressList", "ActionNames", "PressedKeys", "ValidAssociatedBones", "PrioritizedSkelBranches",
                            "HiddenKismetClassNames", "BadPackageNames", "AlwaysShowInSelectedPlatforms", "StartActionPressList", "FailedActionPressList", "RegularPostMatchCelebrationAnims", "StrengthAnimNodeNameList", "m_EntityList"
                        },
                        ["ObjectProperty"] = new[]
                        {
                            "Expressions", "Elements", "EquipMaterials", "ChildParts", "GameTypesSupportedOnThisMap", "TreeNodes", "RoundCountDownSoundList",
                            "DecalLeftMaterials", "ParticleTemplate", "OverideMaterials", "MeleeAttackAnim", "Attributes", "AttachAll", "CountDownSoundList", "AbilityModifiers",
                            "DecalRightMaterials", "AdditionalWeaponAnimset", "AlternateMaterials", "PawnWeapAnim_Attack", "DetachAny", "SpawnPoints", "AllSlots", "AdditionalWeapon",
                            "WeaponProjectiles", "EquipMorphTargets", "Behaviors", "AttachAny", "Goals", "RuntimeParameters", "NoiseModifiers",
                            "PropertyModifiers", "WeaponFireSnd", "WeaponFireSnd_3P", "EffectSockets", "Items", "Procedures", "SourceMaterials", "Profiles", "Modifiers", "LevelThresholds",
                            "AttachedTuningItems", "UnsupportedPaints", "ExcludePaints", "ShopExclusivePaints", "IncludePaints", "Overrides", "ObjList", "MultipleBoostsOverride", "Features",
                            "StreamingLevels", "Sockets", "FaceFXAnimSets", "RefreshSubscriberNotifies", "Conditions", "PowerupDamageModifiers", "FadeMaterials", "TranslucentMaterials",
                            "FragmentArches", "FireCrackerExplosionTemplates", "ComponentDynArray", "MorphSets", "WeaponForeHandgripAnimSets", "LinksFrom", "SpecialEditions", "DamageEvents",
                            "DecalMaterials", "ModifiedComponent_Array", "ModifiedChild_Defined_Array", "UnmodifiedChild_Defined_Array", "Traits", "WhitelistProducts", "ChargeSFX",
                            "ClientDestroyedActorContent", "m_affectedUsableObjects", "SilhouettePrimitives", "ActionsToExecute", "CrowdActors", "PaintableMaterials", "ProhitibitedDefaultMatchTypes",
                            "AdditiveBasePoseAnimSeq", "AdditiveTargetPoseAnimSeq", "SubTracks", "DefaultMaterials", "ParticleModuleEventsToSendToGame", "OverridesToApply", "TeamArchetypes", "ArmsMesh_Common_Animsets",
                            "StatModComponents", "InventoryList", "ClothingAssets", "FemaleAnimSets", "DamageTypes", "IgnoreDamageTypes", "LinkedEvents", "Decorators", "DefaultCarComponents", "EyeMaterials",
                            "ClassProximityTypes", "MaleAnimSets", "RelatedAdditiveAnimSeqs", "SkelControls", "ConstraintSetup", "BoundsBodies", "ClothingAssets", "SpawnPointLobbyTeams", "SpawnTeams", "HeadMorphTargets",
                            "Expression", "EditorComments", "MeshMaterials", "MaleLobbyAnimSets", "FemaleLobbyAnimSets", "HandSignalAnims", "WeaponProjectilesAT", "SoundPacks", "Bots2", "OverrideStates",
                            "BloodSplatterDecalMaterial", "StaticMeshComponents", "FunctionExpressions", "LightComponents", "MetaData", "RootMorphNodes", "AnimTickArray", "StatCategories", "Titles", "m_FemaleGroupAnimSets",
                            "ParentNodes", "LinkedVariables", "InterpTracks", "InterpGroups", "BodySetup", "Bodies", "Styles", "CollisionComponents", "BlacklistProducts", "ImportantStatEvents", "m_ArchetypeTemplates",
                            "InactiveStatMorphTargetSetes", "Flashlight_MeshComponents", "Flashlight_FlareComponents", "Flashlight_FlareSockets", "ParticleSystemComponents", "ExplosionComponents", "m_HealthChildren",
                            "Wheels", "Flashlight_LightSockets", "FlickerFunctionArchetypes", "GroupAnimSets", "Materials", "StreamingLevels", "AllStatEvents", "CountdownMessages", "GameStates", "m_PhysicsChildren",
                            "AchievementIcons", "ConnectorSockets", "ChildProFXTextures", "ChildNodes", "Sequences", "ReferencedObjects", "InvisiTekMaterials", "GameTypesToUseDefaultProduct", "OffsetNodes2",
                            "References", "Textures", "RemovedArchetypes", "PrefabArchetypes", "Attached", "SequenceObjects", "LFMaterials", "BeamSystems", "ReplacingBotCountdownMessages", "PlayerNodes",
                            "DecalList", "Anim", "ControlHead", "AnimSets", "Components", "Modules", "Targets", "Controls", "SpawnModules", "MapSetsToUseDefaultProduct", "Mutators", "UserReferences", "Flashlight_LightComponents",
                            "UpdateModules", "Emitters", "LODLevels", "ReplayClips", "Skins", "Effects", "ReferencedTextures", "OverrideCarComponents", "SpawnPoints", "DefaultMutators", "c_TurnInPlaceNodes",

                            "AttackSnd", "DeathSnd", "HitSnd", "IdleSnd", "KnockedSnd", "PushedAwaySnd", "WakeUpSnd",
                        },
                        ["ClassProperty"] = new[] { "WeaponClasses", "PreviewAnimSets", "SupportSynchronizeClasses" }
                    };

                    // how do I check if it's NOT "Type": "FXActor_TA"
                    if (name is "Attachments" or "Children" or "ShadowMaps" or "WeaponFireAnim" or "WeaponIdleAnims" or "InactiveStates" && size == 4)
                    {
                        InnerType = "ObjectProperty";
                        return;
                    }
                    if (name == "Points" && size == 4)
                    {
                        InnerType = "FloatProperty";
                        return;
                    }

                    if (map.Any(kv => kv.Value.Contains(name)))
                    {
                        InnerType = map.First(kv => kv.Value.Contains(name)).Key;
                        return;
                    }

                    switch (name)
                    {
                        case "FaceNormalDirections":
                        case "SourceOffsetDefaults":
                        case "ClothMeshNormalData":
                        case "PreCachedPhysScale":
                        case "EdgeDirections":
                        case "VertexData":
                        case "SpawnOffset":
                        case "PosKeys":
                            InnerTypeData = new FPropertyTagData("Vector", name);
                            break;
                        case "RotKeys":
                            InnerTypeData = new FPropertyTagData("Quat", name);
                            break;
                        case "DecalRotation":
                        case "SpawnFacing":
                            InnerTypeData = new FPropertyTagData("Rotator", name);
                            break;
                        case "FacePlaneData":
                        case "PermutedVertexData":
                            InnerTypeData = new FPropertyTagData("Plane", name);
                            break;
                        case "DefaultColorList":
                        case "ColorBlindColorList":
                            InnerTypeData = new FPropertyTagData("LinearColor", name);
                            break;
                        case "IrrelevantLights":
                        case "ReferencedTextureGuids":
                            InnerTypeData = new FPropertyTagData("Guid", name);
                            break;
                        case "Characters":
                            InnerTypeData = new FPropertyTagData("FontCharacter", name);
                            break;
                    }
                    InnerType = "StructProperty";
                }

                break;
            // Serialize the following if version is past PROPERTY_TAG_SET_MAP_SUPPORT
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
