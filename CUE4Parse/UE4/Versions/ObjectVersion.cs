using System;
using System.Runtime.CompilerServices;

namespace CUE4Parse.UE4.Versions
{
    public enum EUnrealEngineObjectUE5Version : uint
    {
        // Note that currently the oldest loadable package version is EUnrealEngineObjectUE4Version.OLDEST_LOADABLE_PACKAGE
        // this can be enabled should we ever deprecate UE4 versions entirely
        //OLDEST_LOADABLE_PACKAGE = ???,
        // The original UE5 version, at the time this was added the UE4 version was 522, so UE5 will start from 1000 to show a clear difference

        INITIAL_VERSION = 1000,

        // Support stripping names that are not referenced from export data
        NAMES_REFERENCED_FROM_EXPORT_DATA,

        // Added a payload table of contents to the package summary
        PAYLOAD_TOC,

        // Added data to identify references from and to optional package
        OPTIONAL_RESOURCES,

        // Large world coordinates converts a number of core types to double components by default.
        LARGE_WORLD_COORDINATES,

        // Remove package GUID from FObjectExport
        REMOVE_OBJECT_EXPORT_PACKAGE_GUID,

        // Add IsInherited to the FObjectExport entry
        TRACK_OBJECT_EXPORT_IS_INHERITED,

        // Replace FName asset path in FSoftObjectPath with (package name, asset name) pair FTopLevelAssetPath
        FSOFTOBJECTPATH_REMOVE_ASSET_PATH_FNAMES,

        // Add a soft object path list to the package summary for fast remap
        ADD_SOFTOBJECTPATH_LIST,

        // Added bulk/data resource table
        DATA_RESOURCES,

        // Added script property serialization offset to export table entries for saved, versioned packages
        SCRIPT_SERIALIZATION_OFFSET,

        // Adding property tag extension,
        // Support for overridable serialization on UObject,
        // Support for overridable logic in containers
        PROPERTY_TAG_EXTENSION_AND_OVERRIDABLE_SERIALIZATION,

        // Added property tag complete type name and serialization type
        PROPERTY_TAG_COMPLETE_TYPE_NAME,

        // Changed UE::AssetRegistry::WritePackageData to include PackageBuildDependencies
        ASSETREGISTRY_PACKAGEBUILDDEPENDENCIES,

        // Added meta data serialization offset to for saved, versioned packages
        METADATA_SERIALIZATION_OFFSET,

        // Added VCells to the object graph
        VERSE_CELLS,

        // Changed PackageFileSummary to write FIoHash PackageSavedHash instead of FGuid Guid
        PACKAGE_SAVED_HASH,

        // OS shadow serialization of subobjects
        OS_SUB_OBJECT_SHADOW_SERIALIZATION,

        // Adds a table of hierarchical type information for imports in a package
        IMPORT_TYPE_HIERARCHIES,

        // -----<new versions can be added before this line>-------------------------------------------------
        // this needs to be the last line (see note below)
        AUTOMATIC_VERSION_PLUS_ONE,
        AUTOMATIC_VERSION = AUTOMATIC_VERSION_PLUS_ONE - 1
    }

    public enum EUnrealEngineObjectUE4Version
    {
        DETERMINE_BY_GAME = 0,

        // Pre-release UE4 file versions
        ASSET_REGISTRY_TAGS = 112,
        TEXTURE_DERIVED_DATA2 = 124,
        ADD_COOKED_TO_TEXTURE2D = 125,
        REMOVED_STRIP_DATA = 130,
        REMOVE_EXTRA_SKELMESH_VERTEX_INFLUENCES = 134,
        TEXTURE_SOURCE_ART_REFACTOR = 143,
        ADD_SKELMESH_MESHTOIMPORTVERTEXMAP = 152,
        REMOVE_ARCHETYPE_INDEX_FROM_LINKER_TABLES = 163,
        REMOVE_NET_INDEX = 196,
        BULKDATA_AT_LARGE_OFFSETS = 198,
        SUMMARY_HAS_BULKDATA_OFFSET = 212,

        OLDEST_LOADABLE_PACKAGE = 214,

        // Removed restriction on blueprint-exposed variables from being read-only
        BLUEPRINT_VARS_NOT_READ_ONLY,
        // Added manually serialized element to UStaticMesh (precalculated nav collision)
        STATIC_MESH_STORE_NAV_COLLISION,
        // Changed property name for atmospheric fog
        ATMOSPHERIC_FOG_DECAY_NAME_CHANGE,
        // Change many properties/functions from Translation to Location
        SCENECOMP_TRANSLATION_TO_LOCATION,
        // Material attributes reordering
        MATERIAL_ATTRIBUTES_REORDERING,
        // Collision Profile setting has been added, and all components that exists has to be properly upgraded
        COLLISION_PROFILE_SETTING,
        // Making the blueprint's skeleton class transient
        BLUEPRINT_SKEL_TEMPORARY_TRANSIENT,
        // Making the blueprint's skeleton class serialized again
        BLUEPRINT_SKEL_SERIALIZED_AGAIN,
        // Blueprint now controls replication settings again
        BLUEPRINT_SETS_REPLICATION,
        // Added level info used by World browser
        WORLD_LEVEL_INFO,
        // Changed capsule height to capsule half-height (afterwards)
        AFTER_CAPSULE_HALF_HEIGHT_CHANGE,
        // Added Namepace, GUID (Key) and Flags to FText
        ADDED_NAMESPACE_AND_KEY_DATA_TO_FTEXT,
        // Attenuation shapes
        ATTENUATION_SHAPES,
        // Use IES texture multiplier even when IES brightness is not being used
        LIGHTCOMPONENT_USE_IES_TEXTURE_MULTIPLIER_ON_NON_IES_BRIGHTNESS,
        // Removed InputComponent as a blueprint addable component
        REMOVE_INPUT_COMPONENTS_FROM_BLUEPRINTS,
        // Use an FMemberReference struct in UK2Node_Variable
        VARK2NODE_USE_MEMBERREFSTRUCT,
        // Refactored material expression inputs for UMaterialExpressionSceneColor and UMaterialExpressionSceneDepth
        REFACTOR_MATERIAL_EXPRESSION_SCENECOLOR_AND_SCENEDEPTH_INPUTS,
        // Spline meshes changed from Z forwards to configurable
        SPLINE_MESH_ORIENTATION,
        // Added ReverbEffect asset type
        REVERB_EFFECT_ASSET_TYPE,
        // changed max texcoords from 4 to 8
        MAX_TEXCOORD_INCREASED,
        // static meshes changed to support SpeedTrees
        SPEEDTREE_STATICMESH,
        // Landscape component reference between landscape component and collision component
        LANDSCAPE_COMPONENT_LAZY_REFERENCES,
        // Refactored UK2Node_CallFunction to use FMemberReference
        SWITCH_CALL_NODE_TO_USE_MEMBER_REFERENCE,
        // Added fixup step to remove skeleton class references from blueprint objects
        ADDED_SKELETON_ARCHIVER_REMOVAL,
        // See above, take 2.
        ADDED_SKELETON_ARCHIVER_REMOVAL_SECOND_TIME,
        // Making the skeleton class on blueprints transient
        BLUEPRINT_SKEL_CLASS_TRANSIENT_AGAIN,
        // UClass knows if it's been cooked
        ADD_COOKED_TO_UCLASS,
        // Deprecated static mesh thumbnail properties were removed
        DEPRECATED_STATIC_MESH_THUMBNAIL_PROPERTIES_REMOVED,
        // Added collections in material shader map ids
        COLLECTIONS_IN_SHADERMAPID,
        // Renamed some Movement Component properties, added PawnMovementComponent
        REFACTOR_MOVEMENT_COMPONENT_HIERARCHY,
        // Swap UMaterialExpressionTerrainLayerSwitch::LayerUsed/LayerNotUsed the correct way round
        FIX_TERRAIN_LAYER_SWITCH_ORDER,
        // Remove URB_ConstraintSetup
        ALL_PROPS_TO_CONSTRAINTINSTANCE,
        // Low quality directional lightmaps
        LOW_QUALITY_DIRECTIONAL_LIGHTMAPS,
        // Added NoiseEmitterComponent and removed related Pawn properties.
        ADDED_NOISE_EMITTER_COMPONENT,
        // Add text component vertical alignment
        ADD_TEXT_COMPONENT_VERTICAL_ALIGNMENT,
        // Added AssetImportData for FBX asset types, deprecating SourceFilePath and SourceFileTimestamp
        ADDED_FBX_ASSET_IMPORT_DATA,
        // Remove LevelBodySetup from ULevel
        REMOVE_LEVELBODYSETUP,
        // Refactor character crouching
        REFACTOR_CHARACTER_CROUCH,
        // Trimmed down material shader debug information.
        SMALLER_DEBUG_MATERIALSHADER_UNIFORM_EXPRESSIONS,
        // APEX Clothing
        APEX_CLOTH,
        // Change Collision Channel to save only modified ones than all of them
        // @note!!! once we pass this CL, we can rename FCollisionResponseContainer enum values
        // we should rename to match ECollisionChannel
        SAVE_COLLISIONRESPONSE_PER_CHANNEL,
        // Added Landscape Spline editor meshes
        ADDED_LANDSCAPE_SPLINE_EDITOR_MESH,
        // Fixup input expressions for reading from refraction material attributes.
        CHANGED_MATERIAL_REFACTION_TYPE,
        // Refactor projectile movement, along with some other movement component work.
        REFACTOR_PROJECTILE_MOVEMENT,
        // Remove PhysicalMaterialProperty and replace with user defined enum
        REMOVE_PHYSICALMATERIALPROPERTY,
        // Removed all compile outputs from FMaterial
        PURGED_FMATERIAL_COMPILE_OUTPUTS,
        // Ability to save cooked PhysX meshes to Landscape
        ADD_COOKED_TO_LANDSCAPE,
        // Change how input component consumption works
        CONSUME_INPUT_PER_BIND,
        // Added new Graph based SoundClass Editor
        SOUND_CLASS_GRAPH_EDITOR,
        // Fixed terrain layer node guids which was causing artifacts
        FIXUP_TERRAIN_LAYER_NODES,
        // Added clamp min/max swap check to catch older materials
        RETROFIT_CLAMP_EXPRESSIONS_SWAP,
        // Remove static/movable/stationary light classes
        REMOVE_LIGHT_MOBILITY_CLASSES,
        // Refactor the way physics blending works to allow partial blending
        REFACTOR_PHYSICS_BLENDING,
        // WorldLevelInfo: Added reference to parent level and streaming distance
        WORLD_LEVEL_INFO_UPDATED,
        // Fixed cooking of skeletal/static meshes due to bad serialization logic
        STATIC_SKELETAL_MESH_SERIALIZATION_FIX,
        // Removal of InterpActor and PhysicsActor
        REMOVE_STATICMESH_MOBILITY_CLASSES,
        // Refactor physics transforms
        REFACTOR_PHYSICS_TRANSFORMS,
        // Remove zero triangle sections from static meshes and compact material indices.
        REMOVE_ZERO_TRIANGLE_SECTIONS,
        // Add param for deceleration in character movement instead of using acceleration.
        CHARACTER_MOVEMENT_DECELERATION,
        // Made ACameraActor use a UCameraComponent for parameter storage, etc...
        CAMERA_ACTOR_USING_CAMERA_COMPONENT,
        // Deprecated some pitch/roll properties in CharacterMovementComponent
        CHARACTER_MOVEMENT_DEPRECATE_PITCH_ROLL,
        // Rebuild texture streaming data on load for uncooked builds
        REBUILD_TEXTURE_STREAMING_DATA_ON_LOAD,
        // Add support for 32 bit index buffers for static meshes.
        SUPPORT_32BIT_STATIC_MESH_INDICES,
        // Added streaming install ChunkID to AssetData and UPackage
        ADDED_CHUNKID_TO_ASSETDATA_AND_UPACKAGE,
        // Add flag to control whether Character blueprints receive default movement bindings.
        CHARACTER_DEFAULT_MOVEMENT_BINDINGS,
        // APEX Clothing LOD Info
        APEX_CLOTH_LOD,
        // Added atmospheric fog texture data to be general
        ATMOSPHERIC_FOG_CACHE_DATA,
        // Arrays serialize their inner's tags
        ARRAY_PROPERTY_INNER_TAGS,
        // Skeletal mesh index data is kept in memory in game to support mesh merging.
        KEEP_SKEL_MESH_INDEX_DATA,
        // Added compatibility for the body instance collision change
        BODYSETUP_COLLISION_CONVERSION,
        // Reflection capture cooking
        REFLECTION_CAPTURE_COOKING,
        // Removal of DynamicTriggerVolume, DynamicBlockingVolume, DynamicPhysicsVolume
        REMOVE_DYNAMIC_VOLUME_CLASSES,
        // Store an additional flag in the BodySetup to indicate whether there is any cooked data to load
        STORE_HASCOOKEDDATA_FOR_BODYSETUP,
        // Changed name of RefractionBias to RefractionDepthBias.
        REFRACTION_BIAS_TO_REFRACTION_DEPTH_BIAS,
        // Removal of SkeletalPhysicsActor
        REMOVE_SKELETALPHYSICSACTOR,
        // PlayerController rotation input refactor
        PC_ROTATION_INPUT_REFACTOR,
        // Landscape Platform Data cooking
        LANDSCAPE_PLATFORMDATA_COOKING,
        // Added call for linking classes in CreateExport to ensure memory is initialized properly
        CREATEEXPORTS_CLASS_LINKING_FOR_BLUEPRINTS,
        // Remove native component nodes from the blueprint SimpleConstructionScript
        REMOVE_NATIVE_COMPONENTS_FROM_BLUEPRINT_SCS,
        // Removal of Single Node Instance
        REMOVE_SINGLENODEINSTANCE,
        // Character movement braking changes
        CHARACTER_BRAKING_REFACTOR,
        // Supported low quality lightmaps in volume samples
        VOLUME_SAMPLE_LOW_QUALITY_SUPPORT,
        // Split bEnableTouchEvents out from bEnableClickEvents
        SPLIT_TOUCH_AND_CLICK_ENABLES,
        // Health/Death refactor
        HEALTH_DEATH_REFACTOR,
        // Moving USoundNodeEnveloper from UDistributionFloatConstantCurve to FRichCurve
        SOUND_NODE_ENVELOPER_CURVE_CHANGE,
        // Moved SourceRadius to UPointLightComponent
        POINT_LIGHT_SOURCE_RADIUS,
        // Scene capture actors based on camera actors.
        SCENE_CAPTURE_CAMERA_CHANGE,
        // Moving SkeletalMesh shadow casting flag from LoD details to material
        MOVE_SKELETALMESH_SHADOWCASTING,
        // Changing bytecode operators for creating arrays
        CHANGE_SETARRAY_BYTECODE,
        // Material Instances overriding base material properties.
        MATERIAL_INSTANCE_BASE_PROPERTY_OVERRIDES,
        // Combined top/bottom lightmap textures
        COMBINED_LIGHTMAP_TEXTURES,
        // Forced material lightmass guids to be regenerated
        BUMPED_MATERIAL_EXPORT_GUIDS,
        // Allow overriding of parent class input bindings
        BLUEPRINT_INPUT_BINDING_OVERRIDES,
        // Fix up convex invalid transform
        FIXUP_BODYSETUP_INVALID_CONVEX_TRANSFORM,
        // Fix up scale of physics stiffness and damping value
        FIXUP_STIFFNESS_AND_DAMPING_SCALE,
        // Convert USkeleton and FBoneContrainer to using FReferenceSkeleton.
        REFERENCE_SKELETON_REFACTOR,
        // Adding references to variable, function, and macro nodes to be able to update to renamed values
        K2NODE_REFERENCEGUIDS,
        // Fix up the 0th bone's parent bone index.
        FIXUP_ROOTBONE_PARENT,
        //Allow setting of TextRenderComponents size in world space.
        TEXT_RENDER_COMPONENTS_WORLD_SPACE_SIZING,
        // Material Instances overriding base material properties #2.
        MATERIAL_INSTANCE_BASE_PROPERTY_OVERRIDES_PHASE_2,
        // CLASS_Placeable becomes CLASS_NotPlaceable
        CLASS_NOTPLACEABLE_ADDED,
        // Added LOD info list to a world tile description
        WORLD_LEVEL_INFO_LOD_LIST,
        // CharacterMovement variable naming refactor
        CHARACTER_MOVEMENT_VARIABLE_RENAMING_1,
        // FName properties containing sound names converted to FSlateSound properties
        FSLATESOUND_CONVERSION,
        // Added ZOrder to a world tile description
        WORLD_LEVEL_INFO_ZORDER,
        // Added flagging of localization gather requirement to packages
        PACKAGE_REQUIRES_LOCALIZATION_GATHER_FLAGGING,
        // Preventing Blueprint Actor variables from having default values
        BP_ACTOR_VARIABLE_DEFAULT_PREVENTING,
        // Preventing Blueprint Actor variables from having default values
        TEST_ANIMCOMP_CHANGE,
        // Class as primary asset, name convention changed
        EDITORONLY_BLUEPRINTS,
        // Custom serialization for FEdGraphPinType
        EDGRAPHPINTYPE_SERIALIZATION,
        // Stop generating 'mirrored' cooked mesh for Brush and Model components
        NO_MIRROR_BRUSH_MODEL_COLLISION,
        // Changed ChunkID to be an array of IDs.
        CHANGED_CHUNKID_TO_BE_AN_ARRAY_OF_CHUNKIDS,
        // Worlds have been renamed from "TheWorld" to be named after the package containing them
        WORLD_NAMED_AFTER_PACKAGE,
        // Added sky light component
        SKY_LIGHT_COMPONENT,
        // Added Enable distance streaming flag to FWorldTileLayer
        WORLD_LAYER_ENABLE_DISTANCE_STREAMING,
        // Remove visibility/zone information from UModel
        REMOVE_ZONES_FROM_MODEL,
        // Fix base pose serialization
        FIX_ANIMATIONBASEPOSE_SERIALIZATION,
        // Support for up to 8 skinning influences per vertex on skeletal meshes (on non-gpu vertices)
        SUPPORT_8_BONE_INFLUENCES_SKELETAL_MESHES,
        // Add explicit bOverrideGravity to world settings
        ADD_OVERRIDE_GRAVITY_FLAG,
        // Support for up to 8 skinning influences per vertex on skeletal meshes (on gpu vertices)
        SUPPORT_GPUSKINNING_8_BONE_INFLUENCES,
        // Supporting nonuniform scale animation
        ANIM_SUPPORT_NONUNIFORM_SCALE_ANIMATION,
        // Engine version is stored as a FEngineVersion object rather than changelist number
        ENGINE_VERSION_OBJECT,
        // World assets now have RF_Public
        PUBLIC_WORLDS,
        // Skeleton Guid
        SKELETON_GUID_SERIALIZATION,
        // Character movement WalkableFloor refactor
        CHARACTER_MOVEMENT_WALKABLE_FLOOR_REFACTOR,
        // Lights default to inverse squared
        INVERSE_SQUARED_LIGHTS_DEFAULT,
        // Disabled SCRIPT_LIMIT_BYTECODE_TO_64KB
        DISABLED_SCRIPT_LIMIT_BYTECODE,
        // Made remote role private, exposed bReplicates
        PRIVATE_REMOTE_ROLE,
        // Fix up old foliage components to have static mobility (superseded by FOLIAGE_MOVABLE_MOBILITY)
        FOLIAGE_STATIC_MOBILITY,
        // Change BuildScale from a float to a vector
        BUILD_SCALE_VECTOR,
        // After implementing foliage collision, need to disable collision on old foliage instances
        FOLIAGE_COLLISION,
        // Added sky bent normal to indirect lighting cache
        SKY_BENT_NORMAL,
        // Added cooking for landscape collision data
        LANDSCAPE_COLLISION_DATA_COOKING,
        // Convert CPU tangent Z delta to vector from PackedNormal since we don't get any benefit other than memory
        // we still convert all to FVector in CPU time whenever any calculation
        MORPHTARGET_CPU_TANGENTZDELTA_FORMATCHANGE,
        // Soft constraint limits will implicitly use the mass of the bodies
        SOFT_CONSTRAINTS_USE_MASS,
        // Reflection capture data saved in packages
        REFLECTION_DATA_IN_PACKAGES,
        // Fix up old foliage components to have movable mobility (superseded by FOLIAGE_STATIC_LIGHTING_SUPPORT)
        FOLIAGE_MOVABLE_MOBILITY,
        // Undo BreakMaterialAttributes changes as it broke old content
        UNDO_BREAK_MATERIALATTRIBUTES_CHANGE,
        // Now Default custom profile name isn't NONE anymore due to copy/paste not working properly with it
        ADD_CUSTOMPROFILENAME_CHANGE,
        // Permanently flip and scale material expression coordinates
        FLIP_MATERIAL_COORDS,
        // PinSubCategoryMemberReference added to FEdGraphPinType
        MEMBERREFERENCE_IN_PINTYPE,
        // Vehicles use Nm for Torque instead of cm and RPM instead of rad/s
        VEHICLES_UNIT_CHANGE,
        // removes NANs from all animations when loaded
        // now importing should detect NaNs, so we should not have NaNs in source data
        ANIMATION_REMOVE_NANS,
        // Change skeleton preview attached assets property type
        SKELETON_ASSET_PROPERTY_TYPE_CHANGE,
        // Fix some blueprint variables that have the CPF_DisableEditOnTemplate flag set
        // when they shouldn't
        FIX_BLUEPRINT_VARIABLE_FLAGS,
        // Vehicles use Nm for Torque instead of cm and RPM instead of rad/s part two (missed conversion for some variables
        VEHICLES_UNIT_CHANGE2,
        // Changed order of interface class serialization
        UCLASS_SERIALIZE_INTERFACES_AFTER_LINKING,
        // Change from LOD distances to display factors
        STATIC_MESH_SCREEN_SIZE_LODS,
        // Requires test of material coords to ensure they're saved correctly
        FIX_MATERIAL_COORDS,
        // Changed SpeedTree wind presets to v7
        SPEEDTREE_WIND_V7,
        // NeedsLoadForEditorGame added
        LOAD_FOR_EDITOR_GAME,
        // Manual serialization of FRichCurveKey to save space
        SERIALIZE_RICH_CURVE_KEY,
        // Change the outer of ULandscapeMaterialInstanceConstants and Landscape-related textures to the level in which they reside
        MOVE_LANDSCAPE_MICS_AND_TEXTURES_WITHIN_LEVEL,
        // FTexts have creation history data, removed Key, Namespaces, and SourceString
        FTEXT_HISTORY,
        // Shift comments to the left to contain expressions properly
        FIX_MATERIAL_COMMENTS,
        // Bone names stored as FName means that we can't guarantee the correct case on export, now we store a separate string for export purposes only
        STORE_BONE_EXPORT_NAMES,
        // changed mesh emitter initial orientation to distribution
        MESH_EMITTER_INITIAL_ORIENTATION_DISTRIBUTION,
        // Foliage on blueprints causes crashes
        DISALLOW_FOLIAGE_ON_BLUEPRINTS,
        // change motors to use revolutions per second instead of rads/second
        FIXUP_MOTOR_UNITS,
        // deprecated MovementComponent functions including "ModifiedMaxSpeed" et al
        DEPRECATED_MOVEMENTCOMPONENT_MODIFIED_SPEEDS,
        // rename CanBeCharacterBase
        RENAME_CANBECHARACTERBASE,
        // Change GameplayTagContainers to have FGameplayTags instead of FNames; Required to fix-up native serialization
        GAMEPLAY_TAG_CONTAINER_TAG_TYPE_CHANGE,
        // Change from UInstancedFoliageSettings to UFoliageType, and change the api from being keyed on UStaticMesh* to UFoliageType*
        FOLIAGE_SETTINGS_TYPE,
        // Lights serialize static shadow depth maps
        STATIC_SHADOW_DEPTH_MAPS,
        // Add RF_Transactional to data assets, fixing undo problems when editing them
        ADD_TRANSACTIONAL_TO_DATA_ASSETS,
        // Change LB_AlphaBlend to LB_WeightBlend in ELandscapeLayerBlendType
        ADD_LB_WEIGHTBLEND,
        // Add root component to an foliage actor, all foliage cluster components will be attached to a root
        ADD_ROOTCOMPONENT_TO_FOLIAGEACTOR,
        // FMaterialInstanceBasePropertyOverrides didn't use proper UObject serialize
        FIX_MATERIAL_PROPERTY_OVERRIDE_SERIALIZE,
        // Addition of linear color sampler. color sample type is changed to linear sampler if source texture !sRGB
        ADD_LINEAR_COLOR_SAMPLER,
        // Added StringAssetReferencesMap to support renames of FStringAssetReference properties.
        ADD_STRING_ASSET_REFERENCES_MAP,
        // Apply scale from SCS RootComponent details in the Blueprint Editor to new actor instances at construction time
        BLUEPRINT_USE_SCS_ROOTCOMPONENT_SCALE,
        // Changed level streaming to have a linear color since the visualization doesn't gamma correct.
        LEVEL_STREAMING_DRAW_COLOR_TYPE_CHANGE,
        // Cleared end triggers from non-state anim notifies
        CLEAR_NOTIFY_TRIGGERS,
        // Convert old curve names stored in anim assets into skeleton smartnames
        SKELETON_ADD_SMARTNAMES,
        // Added the currency code field to FTextHistory_AsCurrency
        ADDED_CURRENCY_CODE_TO_FTEXT,
        // Added support for C++11 enum classes
        ENUM_CLASS_SUPPORT,
        // Fixup widget animation class
        FIXUP_WIDGET_ANIMATION_CLASS,
        // USoundWave objects now contain details about compression scheme used.
        SOUND_COMPRESSION_TYPE_ADDED,
        // Bodies will automatically weld when attached
        AUTO_WELDING,
        // Rename UCharacterMovementComponent::bCrouchMovesCharacterDown
        RENAME_CROUCHMOVESCHARACTERDOWN,
        // Lightmap parameters in FMeshBuildSettings
        LIGHTMAP_MESH_BUILD_SETTINGS,
        // Rename SM3 to ES3_1 and updates featurelevel material node selector
        RENAME_SM3_TO_ES3_1,
        // Deprecated separate style assets for use in UMG
        DEPRECATE_UMG_STYLE_ASSETS,
        // Duplicating Blueprints will regenerate NodeGuids after this version
        POST_DUPLICATE_NODE_GUID,
        // Rename USpringArmComponent::bUseControllerViewRotation to bUsePawnViewRotation,
        // Rename UCameraComponent::bUseControllerViewRotation to bUsePawnViewRotation (and change the default value)
        RENAME_CAMERA_COMPONENT_VIEW_ROTATION,
        // Changed FName to be case preserving
        CASE_PRESERVING_FNAME,
        // Rename USpringArmComponent::bUsePawnViewRotation to bUsePawnControlRotation
        // Rename UCameraComponent::bUsePawnViewRotation to bUsePawnControlRotation
        RENAME_CAMERA_COMPONENT_CONTROL_ROTATION,
        // Fix bad refraction material attribute masks
        FIX_REFRACTION_INPUT_MASKING,
        // A global spawn rate for emitters.
        GLOBAL_EMITTER_SPAWN_RATE_SCALE,
        // Cleanup destructible mesh settings
        CLEAN_DESTRUCTIBLE_SETTINGS,
        // CharacterMovementComponent refactor of AdjustUpperHemisphereImpact and deprecation of some associated vars.
        CHARACTER_MOVEMENT_UPPER_IMPACT_BEHAVIOR,
        // Changed Blueprint math equality functions for vectors and rotators to operate as a "nearly" equals rather than "exact"
        BP_MATH_VECTOR_EQUALITY_USES_EPSILON,
        // Static lighting support was re-added to foliage, and mobility was returned to static
        FOLIAGE_STATIC_LIGHTING_SUPPORT,
        // Added composite fonts to Slate font info
        SLATE_COMPOSITE_FONTS,
        // Remove UDEPRECATED_SaveGameSummary, required for UWorld::Serialize
        REMOVE_SAVEGAMESUMMARY,

        //Remove bodyseutp serialization from skeletal mesh component
        REMOVE_SKELETALMESH_COMPONENT_BODYSETUP_SERIALIZATION,
        // Made Slate font data use bulk data to store the embedded font data
        SLATE_BULK_FONT_DATA,
        // Add new friction behavior in ProjectileMovementComponent.
        ADD_PROJECTILE_FRICTION_BEHAVIOR,
        // Add axis settings enum to MovementComponent.
        MOVEMENTCOMPONENT_AXIS_SETTINGS,
        // Switch to new interactive comments, requires boundry conversion to preserve previous states
        GRAPH_INTERACTIVE_COMMENTBUBBLES,
        // Landscape serializes physical materials for collision objects
        LANDSCAPE_SERIALIZE_PHYSICS_MATERIALS,
        // Rename Visiblity on widgets to Visibility
        RENAME_WIDGET_VISIBILITY,
        // add track curves for animation
        ANIMATION_ADD_TRACKCURVES,
        // Removed BranchingPoints from AnimMontages and converted them to regular AnimNotifies.
        MONTAGE_BRANCHING_POINT_REMOVAL,
        // Enforce const-correctness in Blueprint implementations of native C++ const class methods
        BLUEPRINT_ENFORCE_CONST_IN_FUNCTION_OVERRIDES,
        // Added pivot to widget components, need to load old versions as a 0,0 pivot, new default is 0.5,0.5
        ADD_PIVOT_TO_WIDGET_COMPONENT,
        // Added finer control over when AI Pawns are automatically possessed. Also renamed Pawn.AutoPossess to Pawn.AutoPossessPlayer indicate this was a setting for players and not AI.
        PAWN_AUTO_POSSESS_AI,
        // Added serialization of timezone to FTextHistory for AsDate operations.
        FTEXT_HISTORY_DATE_TIMEZONE,
        // Sort ActiveBoneIndices on lods so that we can avoid doing it at run time
        SORT_ACTIVE_BONE_INDICES,
        // Added per-frame material uniform expressions
        PERFRAME_MATERIAL_UNIFORM_EXPRESSIONS,
        // Make MikkTSpace the default tangent space calculation method for static meshes.
        MIKKTSPACE_IS_DEFAULT,
        // Only applies to cooked files, grass cooking support.
        LANDSCAPE_GRASS_COOKING,
        // Fixed code for using the bOrientMeshEmitters property.
        FIX_SKEL_VERT_ORIENT_MESH_PARTICLES,
        // Do not change landscape section offset on load under world composition
        LANDSCAPE_STATIC_SECTION_OFFSET,
        // New options for navigation data runtime generation (static, modifiers only, dynamic)
        ADD_MODIFIERS_RUNTIME_GENERATION,
        // Tidied up material's handling of masked blend mode.
        MATERIAL_MASKED_BLENDMODE_TIDY,
        // Original version of MERGED_ADD_MODIFIERS_RUNTIME_GENERATION_TO_4_7; renumbered to prevent blocking promotion in main.
        MERGED_ADD_MODIFIERS_RUNTIME_GENERATION_TO_4_7_DEPRECATED,
        // Original version of AFTER_MERGED_ADD_MODIFIERS_RUNTIME_GENERATION_TO_4_7; renumbered to prevent blocking promotion in main.
        AFTER_MERGED_ADD_MODIFIERS_RUNTIME_GENERATION_TO_4_7_DEPRECATED,
        // After merging ADD_MODIFIERS_RUNTIME_GENERATION into 4.7 branch
        MERGED_ADD_MODIFIERS_RUNTIME_GENERATION_TO_4_7,
        // After merging ADD_MODIFIERS_RUNTIME_GENERATION into 4.7 branch
        AFTER_MERGING_ADD_MODIFIERS_RUNTIME_GENERATION_TO_4_7,
        // Landscape grass weightmap data is now generated in the editor and serialized.
        SERIALIZE_LANDSCAPE_GRASS_DATA,
        // New property to optionally prevent gpu emitters clearing existing particles on Init().
        OPTIONALLY_CLEAR_GPU_EMITTERS_ON_INIT,
        // Also store the Material guid with the landscape grass data
        SERIALIZE_LANDSCAPE_GRASS_DATA_MATERIAL_GUID,
        // Make sure that all template components from blueprint generated classes are flagged as public
        BLUEPRINT_GENERATED_CLASS_COMPONENT_TEMPLATES_PUBLIC,
        // Split out creation method on ActorComponents to distinguish between native, instance, and simple or user construction script
        ACTOR_COMPONENT_CREATION_METHOD,
        // K2Node_Event now uses FMemberReference for handling references
        K2NODE_EVENT_MEMBER_REFERENCE,
        // FPropertyTag stores GUID of struct
        STRUCT_GUID_IN_PROPERTY_TAG,
        // Remove unused UPolys from UModel cooked content
        REMOVE_UNUSED_UPOLYS_FROM_UMODEL,
        // This doesn't do anything except trigger a rebuild on HISMC cluster trees, in this case to get a good "occlusion query" level
        REBUILD_HIERARCHICAL_INSTANCE_TREES,
        // Package summary includes an CompatibleWithEngineVersion field, separately to the version it's saved with
        PACKAGE_SUMMARY_HAS_COMPATIBLE_ENGINE_VERSION,
        // Track UCS modified properties on Actor Components
        TRACK_UCS_MODIFIED_PROPERTIES,
        // Allowed landscape spline meshes to be stored into landscape streaming levels rather than the spline's level
        LANDSCAPE_SPLINE_CROSS_LEVEL_MESHES,
        // Deprecate the variables used for sizing in the designer on UUserWidget
        DEPRECATE_USER_WIDGET_DESIGN_SIZE,
        // Make the editor views array dynamically sized
        ADD_EDITOR_VIEWS,
        // Updated foliage to work with either FoliageType assets or blueprint classes
        FOLIAGE_WITH_ASSET_OR_CLASS,
        // Allows PhysicsSerializer to serialize shapes and actors for faster load times
        BODYINSTANCE_BINARY_SERIALIZATION,
        // Added fastcall data serialization directly in UFunction
        SERIALIZE_BLUEPRINT_EVENTGRAPH_FASTCALLS_IN_UFUNCTION,
        // Changes to USplineComponent and FInterpCurve
        INTERPCURVE_SUPPORTS_LOOPING,
        // Material Instances overriding base material LOD transitions
        MATERIAL_INSTANCE_BASE_PROPERTY_OVERRIDES_DITHERED_LOD_TRANSITION,
        // Serialize ES2 textures separately rather than overwriting the properties used on other platforms
        SERIALIZE_LANDSCAPE_ES2_TEXTURES,
        // Constraint motor velocity is broken into per-component
        CONSTRAINT_INSTANCE_MOTOR_FLAGS,
        // Serialize bIsConst in FEdGraphPinType
        SERIALIZE_PINTYPE_CONST,
        // Change UMaterialFunction::LibraryCategories to LibraryCategoriesText (old assets were saved before auto-conversion of FArrayProperty was possible)
        LIBRARY_CATEGORIES_AS_FTEXT,
        // Check for duplicate exports while saving packages.
        SKIP_DUPLICATE_EXPORTS_ON_SAVE_PACKAGE,
        // Pre-gathering of gatherable, localizable text in packages to optimize text gathering operation times
        SERIALIZE_TEXT_IN_PACKAGES,
        // Added pivot to widget components, need to load old versions as a 0,0 pivot, new default is 0.5,0.5
        ADD_BLEND_MODE_TO_WIDGET_COMPONENT,
        // Added lightmass primitive setting
        NEW_LIGHTMASS_PRIMITIVE_SETTING,
        // Deprecate NoZSpring property on spring nodes to be replaced with TranslateZ property
        REPLACE_SPRING_NOZ_PROPERTY,
        // Keep enums tight and serialize their values as pairs of FName and value. Don't insert dummy values.
        TIGHTLY_PACKED_ENUMS,
        // Changed Asset import data to serialize file meta data as JSON
        ASSET_IMPORT_DATA_AS_JSON,
        // Legacy gamma support for textures.
        TEXTURE_LEGACY_GAMMA,
        // Added WithSerializer for basic native structures like FVector, FColor etc to improve serialization performance
        ADDED_NATIVE_SERIALIZATION_FOR_IMMUTABLE_STRUCTURES,
        // Deprecated attributes that override the style on UMG widgets
        DEPRECATE_UMG_STYLE_OVERRIDES,
        // Shadowmap penumbra size stored
        STATIC_SHADOWMAP_PENUMBRA_SIZE,
        // Fix BC on Niagara effects from the data object and dev UI changes.
        NIAGARA_DATA_OBJECT_DEV_UI_FIX,
        // Fixed the default orientation of widget component so it faces down +x
        FIXED_DEFAULT_ORIENTATION_OF_WIDGET_COMPONENT,
        // Removed bUsedWithUI flag from UMaterial and replaced it with a new material domain for UI
        REMOVED_MATERIAL_USED_WITH_UI_FLAG,
        // Added braking friction separate from turning friction.
        CHARACTER_MOVEMENT_ADD_BRAKING_FRICTION,
        // Removed TTransArrays from UModel
        BSP_UNDO_FIX,
        // Added default value to dynamic parameter.
        DYNAMIC_PARAMETER_DEFAULT_VALUE,
        // Added ExtendedBounds to StaticMesh
        STATIC_MESH_EXTENDED_BOUNDS,
        // Added non-linear blending to anim transitions, deprecating old types
        ADDED_NON_LINEAR_TRANSITION_BLENDS,
        // AO Material Mask texture
        AO_MATERIAL_MASK,
        // Replaced navigation agents selection with single structure
        NAVIGATION_AGENT_SELECTOR,
        // Mesh particle collisions consider particle size.
        MESH_PARTICLE_COLLISIONS_CONSIDER_PARTICLE_SIZE,
        // Adjacency buffer building no longer automatically handled based on triangle count, user-controlled
        BUILD_MESH_ADJ_BUFFER_FLAG_EXPOSED,
        // Change the default max angular velocity
        MAX_ANGULAR_VELOCITY_DEFAULT,
        // Build Adjacency index buffer for clothing tessellation
        APEX_CLOTH_TESSELLATION,
        // Added DecalSize member, solved backward compatibility
        DECAL_SIZE,
        // Keep only package names in StringAssetReferencesMap
        KEEP_ONLY_PACKAGE_NAMES_IN_STRING_ASSET_REFERENCES_MAP,
        // Support sound cue not saving out editor only data
        COOKED_ASSETS_IN_EDITOR_SUPPORT,
        // Updated dialogue wave localization gathering logic.
        DIALOGUE_WAVE_NAMESPACE_AND_CONTEXT_CHANGES,
        // Renamed MakeRot MakeRotator and rearranged parameters.
        MAKE_ROT_RENAME_AND_REORDER,
        // K2Node_Variable will properly have the VariableReference Guid set if available
        K2NODE_VAR_REFERENCEGUIDS,
        // Added support for sound concurrency settings structure and overrides
        SOUND_CONCURRENCY_PACKAGE,
        // Changing the default value for focusable user widgets to false
        USERWIDGET_DEFAULT_FOCUSABLE_FALSE,
        // Custom event nodes implicitly set 'const' on array and non-array pass-by-reference input params
        BLUEPRINT_CUSTOM_EVENT_CONST_INPUT,
        // Renamed HighFrequencyGain to LowPassFilterFrequency
        USE_LOW_PASS_FILTER_FREQ,
        // UAnimBlueprintGeneratedClass can be replaced by a dynamic class. Use TSubclassOf<UAnimInstance> instead.
        NO_ANIM_BP_CLASS_IN_GAMEPLAY_CODE,
        // The SCS keeps a list of all nodes in its hierarchy rather than recursively building it each time it is requested
        SCS_STORES_ALLNODES_ARRAY,
        // Moved StartRange and EndRange in UFbxAnimSequenceImportData to use FInt32Interval
        FBX_IMPORT_DATA_RANGE_ENCAPSULATION,
        // Adding a new root scene component to camera component
        CAMERA_COMPONENT_ATTACH_TO_ROOT,
        // Updating custom material expression nodes for instanced stereo implementation
        INSTANCED_STEREO_UNIFORM_UPDATE,
        // Texture streaming min and max distance to handle HLOD
        STREAMABLE_TEXTURE_MIN_MAX_DISTANCE,
        // Fixing up invalid struct-to-struct pin connections by injecting available conversion nodes
        INJECT_BLUEPRINT_STRUCT_PIN_CONVERSION_NODES,
        // Saving tag data for Array Property's inner property
        INNER_ARRAY_TAG_INFO,
        // Fixed duplicating slot node names in skeleton due to skeleton preload on compile
        FIX_SLOT_NAME_DUPLICATION,
        // Texture streaming using AABBs instead of Spheres
        STREAMABLE_TEXTURE_AABB,
        // FPropertyTag stores GUID of property
        PROPERTY_GUID_IN_PROPERTY_TAG,
        // Name table hashes are calculated and saved out rather than at load time
        NAME_HASHES_SERIALIZED,
        // Updating custom material expression nodes for instanced stereo implementation refactor
        INSTANCED_STEREO_UNIFORM_REFACTOR,
        // Added compression to the shader resource for memory savings
        COMPRESSED_SHADER_RESOURCES,
        // Cooked files contain the dependency graph for the event driven loader (the serialization is largely independent of the use of the new loader)
        PRELOAD_DEPENDENCIES_IN_COOKED_EXPORTS,
        // Cooked files contain the TemplateIndex used by the event driven loader (the serialization is largely independent of the use of the new loader, i.e. this will be null if cooking for the old loader)
        TemplateIndex_IN_COOKED_EXPORTS,
        // FPropertyTag includes contained type(s) for Set and Map properties:
        PROPERTY_TAG_SET_MAP_SUPPORT,
        // Added SearchableNames to the package summary and asset registry
        ADDED_SEARCHABLE_NAMES,
        // Increased size of SerialSize and SerialOffset in export map entries to 64 bit, allow support for bigger files
        e64BIT_EXPORTMAP_SERIALSIZES,
        // Sky light stores IrradianceMap for mobile renderer.
        SKYLIGHT_MOBILE_IRRADIANCE_MAP,
        // Added flag to control sweep behavior while walking in UCharacterMovementComponent.
        ADDED_SWEEP_WHILE_WALKING_FLAG,
        // StringAssetReference changed to SoftObjectPath and swapped to serialize as a name+string instead of a string
        ADDED_SOFT_OBJECT_PATH,
        // Changed the source orientation of point lights to match spot lights (z axis)
        POINTLIGHT_SOURCE_ORIENTATION,
        // LocalizationId has been added to the package summary (editor-only)
        ADDED_PACKAGE_SUMMARY_LOCALIZATION_ID,
        // Fixed case insensitive hashes of wide strings containing character values from 128-255
        FIX_WIDE_STRING_CRC,
        // Added package owner to allow private references
        ADDED_PACKAGE_OWNER,
        // Changed the data layout for skin weight profile data
        SKINWEIGHT_PROFILE_DATA_LAYOUT_CHANGES,
        // Added import that can have package different than their outer
        NON_OUTER_PACKAGE_IMPORT,
        // Added DependencyFlags to AssetRegistry
        ASSETREGISTRY_DEPENDENCYFLAGS,
        // Fixed corrupt licensee flag in 4.26 assets
        CORRECT_LICENSEE_FLAG,

        // -----<new versions can be added before this line>-------------------------------------------------
        // this needs to be the last line (see note below)
        AUTOMATIC_VERSION_PLUS_ONE,
        AUTOMATIC_VERSION = AUTOMATIC_VERSION_PLUS_ONE - 1
    }

    public enum EUnrealEngineObjectUE3Version
    {
        Release61 = 61,
        Release62 = 62,
        Release64 = 64,
        DeprecatedHeritageTable = 68,
        PanUVRemovedFromPoly = 78,
        CompMipsDeprecated = 84,
        AddedHideCategoriesToUClass = 99,
        LightMapScaleAddedToPoly = 106,
        AddedCppTextToUStruct = 120,

        // only comments exist for these three

        // Merge in skeletal collision stuff for SVehicle support.
        temp1 = 122,
        // removed InclusiveSphereBound from FBSPNode
        temp2 = 123,
        // Added BoneCollisionSpheres, BoneCollisionBoxes & KPhysicsProps to USkeletalMesh (Add bBlockKarma to per-bone primitives.)
        temp3 = 124,
        // Added BoneCollisionBoxesModels (UModels corresponding to the BoneCollisionBoxes array) (Add bBlockNonZeroExtent/bBlockZeroExtent to per-bone primitives.)
        temp4 = 125,
        // Changed static mesh collision code (Merged k-dop static mesh collision code.)
        temp5 = 126,
        // Static mesh collision for skeletal meshes. -pv
        temp6 = 127,
        // added DetailMode to FDecorationLayer
        temp7 = 128,

        MovedFriendlyNameToUFunction = 160,
        TextureDeprecatedFromPoly = 170,
        CompactIndexDeprecated = 178,
        DeprecatedPointer = 180,
        AddedDelegateSourceToUDelegateProperty = 185,
        ClassDependenciesDeprecated = 186,
        DisplacedHideCategories = 187, // todo
        AddedStateStackToUStateFrame = 189,
        PropertyFlagsSizeExpandedTo64Bits = 195,
        AddedComponentGuid = 196,
        Use64BitFlag = 195,
        stuffig = 202,
        PackageImportsDeprecated = 208,
        shortyes = 209,
        fwefwef = 207,
        AddedComponentTemplatesToUClass = 210,
        OldformatDeprecated = 215,
        todoa = 218,
        AddedArcheType = 220,
        AddedBulkLod = 221,
        AddedComponentMapToExports, // todo
        AddedInterfacesFeature = 222,

        // lowest known documented ver

        // Removing Length, XSize, YSize and ZSize from VJointPos
        VER_REMOVE_SIZE_VJOINTPOS = 224,
        // Added BackfaceShadowTexCoord to FVert.
        VER_BACKFACESHADOWTEXCOORD = 225,
        // Added ThumbnailDistance to the natively serialized StaticMesh.
        VER_STATICMESH_THUMBNAIL_DISTANCE = 226,
        // Converted FPoly::Vertex to a TArray.
        VER_FPOLYVERTEXARRAY = 227,
        // Converted FQuantizedLightSample to use FColor instead of BYTE[4]. Needed to byte swap correctly on Xenon
        VER_QUANT_LIGHTSAMPLE_BYTE_TO_COLOR = 228,
        // Low poly collision data for terrain is serialized
        VER_TERRAIN_COLLISION = 229,
        // Added texture LOD groups. Version incremented to set default values.
        VER_TEXTURE_GROUPS = 230,
        // Converted ULightMap1D::Scale to FLOAT[3] instead of FLinearColor
        VER_LIGHTMAP_SCALE_TO_FLOAT_ARRAY = 231,
        // Added decal manager to ULevel.
        VER_ADDED_DECAL_MANAGER = 232,
        // Added ParticleModuleRequired to support LOD levels in particle emitters.
        // Changed to LOD model for emitters.
        VER_CHANGE_EMITTER_TO_LODMODEL = 233,
        // Changed streaming code around to split texture and sound streaming into separate arrays
        VER_SPLIT_SOUND_FROM_TEXTURE_STREAMING = 234,
        // Serialize terrain patch bounds
        VER_SERIALIZE_TERRAIN_PATCHBOUNDS = 235,
        // Add structures to BSP and terrain for pre-cooked Novodex collision data.
        VER_PRECOOK_PHYS_BSP_TERRAIN = 236,
        // Add structures to BrushComponent for pre-cooked Novodex collision data.
        VER_PRECOOK_PHYS_BRUSHES = 237,
        // Add pre-cooked static mesh data cache to ULevel
        VER_PRECOOK_PHYS_STATICMESH_CACHE = 238,
        // UDecalComponent's RenderData is now serialized.
        VER_DECAL_RENDERDATA = 239,
        // UDecalComponent's RenderData is now serialized as pointers.
        VER_DECAL_RENDERDATA_POINTER = 240,
        // Repairs bad centroid calculation of the collision data
        VER_REPAIR_STATICMESH_COLLISION = 241,
        // Added lighting channel support
        VER_LIGHTING_CHANNEL_SUPPORT = 242,
        // LODGroups for particle systems
        VER_PARTICLESYSTEM_LODGROUP_SUPPORT = 243,
        // Changed default volumes for MikeL, propagating to all instances
        VER_SOUNDNODEWAVE_DEFAULT_CHANGE = 244,
        // Added EngineVersion to FPackageFileSummary
        VER_PACKAGEFILESUMMARY_CHANGE = 245,
        // Particles with linear color
        VER_PARTICLESYSTEM_LINEARCOLOR_SUPPORT = 246,
        // Added ExportFlags to FObjectExport
        VER_FOBJECTEXPORT_EXPORTFLAGS = 247,
        // Removed COMPONENT_CLASS_BRIDGE code
        VER_REMOVED_COMPONENT_CLASS_BRIDGE = 248,
        // Moved ExportMap and ImportMap to beginning of file and extended FPackageFileSummary
        VER_MOVED_EXPORTIMPORTMAPS_ADDED_TOTALHEADERSIZE = 249,
        // Introduced concept of default poly flags
        VER_DEFAULTPOLYFLAGS_CHANGE = 250,
        VER_USTRUCT_SERIALIZETAGGEDPROPERTIES_BROKEN = 250,
        // Changed lazy array serialization
        VER_LAZYARRAY_SERIALIZATION_CHANGE = 251,
        // Added USoundNodeMixer::InputVolume
        VER_USOUNDNODEMIXER_INPUTVOLUME = 252,
        // Added version number for precooked physics data
        VER_SAVE_PRECOOK_PHYS_VERSION = 253,
        // Added compression support to TLazyArray
        VER_LAZYARRAY_COMPRESSION = 254,
        // Changed the UI system to use instances of UIState for tracking ui menu states, rather than child classes of UIState
        VER_CHANGED_UISTATES = 255,
        // Fixed brush polyflags defaulting to PF_DefaultFlags which should ONLY be the case for surfaces and polys but not for brushes
        VER_FIXED_BRUSH_POLYFLAGS = 256,
        // Made UIAction.ActionMap transient
        VER_MADE_ACTIONMAP_TRANSIENT = 257,
        // Safe version for UStruct serialization bug
        VER_USTRUCT_SERIALIZETAGGEDPROPERTIES_FIXED = 258,
        // Added terrain InfoFlags
        VER_TERRAIN_ADDING_INFOFLAGS = 259,
        // Added PayloadFilename to FLazyLoader
        VER_LAZYLOADER_PAYLOADFILENAME = 260,
        // Added CursorMap to UISkin
        VER_ADDED_CURSOR_MAP = 261,
        // Static mesh property fixup
        VER_STATICMESH_PROPERTY_FIXUP = 262,
        // InfoData in terrains error... wasn't copy-n-pasting correctly so saved maps are botched
        VER_TERRAIN_INFODATA_ERROR = 263,
        // NavigationPoints were having their DrawScale incorrectly changed by the editor, so saved maps need to have it reset to the default
        VER_NAVIGATIONPOINT_DRAWSCALE_FIXUP = 264,
        // Changed USequenceOp.SeqOpOutputLink.LinkAction from name to object reference
        VER_CHANGED_LINKACTION_TYPE = 265,
        // Replaced TLazyArray with FUntypedBulkData
        VER_REPLACED_LAZY_ARRAY_WITH_UNTYPED_BULK_DATA = 266,
        // Components no longer serialize TemplateName unless the component is a template
        // Fixed ObjectArchetype for components not always pointing to correct object
        VER_FIXED_COMPONENT_TEMPLATES = 267,
        // Static mesh property fixup... again
        VER_STATICMESH_PROPERTY_FIXUP_2 = 268,
        // Added folder name to UPackage
        VER_FOLDER_ADDED = 269,
        // Changed WarGame's ActorFactories to be name only and on save store a reference to the content they
        //      need to spawn.  This makes it so we don't have all possible content loaded when only needed a subset
        VER_WARFARE_FACTORIES_ONLY_REF_WHAT_THEY_NEED = 270,
        // Fixed 2D array of floats serialized by FStaticMeshTriangleBulkData::SerializeElement for UVs to match
        //		the memory layout as required by bulk serialization
        VER_FIXED_TRIANGLE_BULK_DATA_SERIALIZATION = 271,
        // Hardcoded FTerrainMaterialMask bit size
        VER_HARDCODED_TERRAIN_MATERIAL_MASK_SIZE = 272,
        // Removed UPrimitiveComponent->ComponentGuid
        VER_REMOVED_COMPONENT_GUID = 273,
        // Refactored UISkin into two classes (UISkin/UICustomSkin)
        VER_REFACTORED_UISKIN = 274,
        // Added level only lighting option, defaults to TRUE for static only lights
        VER_ADDED_LEVEL_ONLY_LIGHTING_OPTION = 275,
        // Added platform flags to UClass
        VER_ADDED_PLATFORM_FLAGS = 276,
        // Added CookedContentVersion to FPackageFileSummary
        VER_PACKAGEFILESUMMARY_CHANGE_COOK_VER_ADDED = 277,
        // Made FLinearColor serialize as a unit
        VER_CHANGED_FLINEARCOLOR_SERIALIZATION = 278,
        // Made FLinearColor serialize as a unit
        VER_MADE_IMMUTABLE_NONINHERIT = 279,
        // Added per level navigation lists for streaming level fixups
        VER_PERLEVEL_NAVLIST = 280,
        // Added SourceStyleID to UIStyle_Combo
        VER_ADDED_SOURCESTYLEID = 281,
        // Integrated FaceFX 1.5 and cleaned up FaceFX serialization
        VER_FACEFX_1_5_UPGRADE = 282,
        // Remapped SoundCueLocalized instances to SoundCue
        VER_REMAPPED_SOUNDCUELOCALIZED_TO_SOUNDCUE = 283,
        // Added USoundNodeConcatenator::InputVolume
        VER_USOUNDNODECONCATENATOR_INPUTVOLUME = 284,
        // Pre-cook physics data for per-triangle static mesh collision.
        VER_PRECOOK_PERTRI_PHYS_STATICMESH = 285,
        // Changed FInputKeyAction to have a array of UIAction instead of a single UIAction.
        VER_INPUTKEYACTION_HAS_ARRAY_OF_UIACTION = 286,
        // Added UInterpTrackSound::PostLoad for setting the volume and pitch of existing Matinee sound keys.
        VER_INTERP_TRACK_SOUND_VOLUME_PITCH = 287,
        // Changed UClass.Interfaces to be a map
        VER_CHANGED_INTERFACES_TO_MAP = 288,
        // Added animation compression.
        VER_ANIMATION_COMPRESSION = 289,
        // Rewrote simplified collision.
        VER_NEW_SIMPLE_CONVEX_COLLISION = 290,
        // Eliminated book-keeping overhead in animation compression.
        VER_ANIMATION_COMPRESSION_SINGLE_BYTESTREAM = 291,
        // Operations on UAnimSequence::CompressedByteStream are now aligned to four bytes.
        VER_ANIMATION_COMPRESSION_FOUR_BYTE_ALIGNED = 292,
        // Added inclusion/ exclusion volumes to light component
        VER_ADDED_LIGHT_VOLUME_SUPPORT = 293,
        // Added permuted data for simplified collision to use SIMD instructions
        VER_SIMD_SIMPLIFIED_COLLISION_DATA = 294,
        // Fix distributions on DistanceCrossFade
        VER_DISTANCE_CROSSFADE_DISTRIBUTIONS_RESET = 295,
        // Added NameIndexMap to USkeletalMesh to speed up MatchRefBone
        VER_ADD_SKELMESH_NAMEINDEXMAP = 296,
        // Rendering refactor
        VER_RENDERING_REFACTOR = 297,
        // Terrain material resource serialization
        VER_TERRAIN_MATERIALRESOURCE_SERIALIZE = 298,
        // Adding ScreenPositionScaleBiasParameter and SceneDepthCalcParameter
        //		parameters to FMaterialPixelShaderParameters
        VER_SCREENPOSSCALEBIAS_SCENEDEPTHCALC_PARAMETERS = 299,
        // Added TwoSidedSignParameter to FMaterialPixelShaderParameters
        VER_TWOSIDEDSIGN_PARAMETERS = 300,
        // A checkpoint to force all materials to be recompiled
        VER_MATERIAL_RECOMPILE_CHECKPOINT = 301,
        // FDistributions
        VER_FDISTRIBUTIONS = 302,
        // Encapsulated UIDockingSet
        VER_UIDOCKINGSET_CHANGED = 303,
        // Implemented FalloffExponent in Gemini
        VER_FALLOFF_EXPONENT_GEMINI = 304,
        // Remapping TextureRenderTarget to TextureRenderTarget2D
        VER_REMPAP_TEXTURE_RENDER_TARGET = 305,
        // Added compilation errors to FMaterial
        VER_FMATERIAL_COMPILATION_ERRORS = 306,
        // Added platform mask to shader cache
        VER_ADDED_PLATFORMTOSERIALIZEMASK = 307,
        // Forcing all FRawSistributions to be rebuilt
        VER_FDISTRIBUTION_FORCE_DIRTY = 308,
        // Enabled single-pass component instancing
        VER_SINGLEPASS_COMPONENT_INSTANCING = 309,
        // Added profiles to AimOffset node
        VER_AIMOFFSET_PROFILES = 310,
        // Added NumInstructions to FShader
        VER_SHADER_NUMINSTRUCTIONS = 311,
        // Rewrote decals for the new renderer.
        VER_DECAL_REFACTOR = 312,
        // Moved lowest LOD regeneration to PostLoad.
        VER_EMITTER_LOWEST_LOD_REGENERATION = 313,
        // Rotated light-map basis to allow seamless mirroring of UVs over texture X axis.
        VER_LIGHTMAP_SYMMETRIC_OVER_X = 314,
        // Remove CollisionModel from StaticMesh
        VER_REMOVE_STATICMESH_COLLISIONMODEL = 315,
        // Code to convert legacy skylight-primitive interaction semantics to light channels.
        VER_LEGACY_SKYLIGHT_CHANNELS = 316,
        // Recompile shaders for depth-bias expression fix.
        VER_DEPTHBIAS_SHADER_RECOMPILE = 317,
        // Recompile shaders for vertex light-map fix.
        VER_VERTEX_LIGHTMAP_SHADER_RECOMPILE = 318,
        // Recompile global shaders for modulated shadows
        VER_MOD_SHADOW_SHADER_RECOMPILE = 319,
        // Recompile emissive shaders for sky light lower hemisphere support.
        VER_SKYLIGHT_LOWERHEMISPHERE_SHADER_RECOMPILE = 320,
        // Serialize TTransArray owner
        VER_SERIALIZE_TTRANSARRAY_OWNER = 321,
        // Rewritten package map that can replicate objects by index without linkers
        VER_LINKERFREE_PACKAGEMAP = 322,
        // Added gamma correction to simple element pixel shader
        VER_SIMPLEELEMENTSHADER_GAMMACORRECTION = 323,
        // Fixed up PreviewLightRadius for PointLightComponents
        VER_FIXED_POINTLIGHTCOMPONENT_LIGHTRADIUS = 324,
        // Added ColorScale and OverlayColor to GammaCorrectionPixelShader
        VER_GAMMACORRECTION_SHADER_RECOMPILE = 325,
        // Added texture dependency length information to FMaterial
        VER_MATERIAL_TEXTUREDEPENDENCYLENGTH = 326,
        // Added cached cooked audio data for Xbox 360
        VER_ADDED_CACHED_COOKED_XBOX360_DATA = 327,
        // Added color saturate when doing a pow to the simple/gamma pixel shaders
        VER_SATURATE_COLOR_SHADER_RECOMPILE = 328,
        // Changed the DeviceZ to WorldZ conversion shader
        VER_DEVICEZ_CONVERT_SHADER_RECOMPILE = 329,
        // Added bIsMasked to UMaterial (affects velocity shaders)
        VER_MATERIAL_ISMASKED_FLAG = 330,
        // AimOffset Nodes used Quaterions intead of Rotators
        VER_AIMOFFSET_ROT2QUAT = 331,
        // Replaced ULightMap* with FLightMap*
        VER_LIGHTMAP_NON_UOBJECT = 332,
        // TResourceArray usage for mesh rendering data
        VER_USE_UMA_RESOURCE_ARRAY_MESH_DATA = 333,
        // Added package compression
        VER_ADDED_PACKAGE_COMPRESSION_SUPPORT = 334,
        // Changed terrain shader to only be compiled for terrain materials
        VER_SHADER_RECOMPILE_FOR_TERRAIN_MATERIALS = 335,
        // Changed the permuted planes in FConvexVolume to include repeats so that pure SIMD tests can be done
        VER_CONVEX_VOLUMES_PERMUTED_PLANES_CHANGE = 336,
        // Removed redudant enums from UIComp_AutoAlign
        VER_REMOVED_REDUNDANT_ENUMS = 337,
        // VelocityShader/MotionBlurShader recompile
        VER_MOTIONBLURSHADER_RECOMPILE = 338,
        // Added color exp bias term to simple elemnt pixel shader
        VER_SIMPLE_ELEMENT_SHADER_RECOMPILE = 339,
        // Added CompositeDynamic lighting channel
        VER_COMPOSITEDYNAMIC_LIGHTINGCHANNEL = 340,
        // Recompile modulated shadow pixel shader
        VER_MODULATESHADOWPROJECTION_SHADER_RECOMPILE = 341,
        // Upgrading to July XDK requires recompiling shaders
        VER_JULY_XDK_UPGRADE = 342,
        // FName change (splits an FName to name and number pair)
        VER_FNAME_CHANGE_NAME_SPLIT = 343,
        // Recompile translucency pixel shader.
        VER_TRANSLUCENCY_SHADER_RECOMPILE = 344,
        // Added code to fix PointLightComponents that have an invalid PreviewLightRadius [presumably] resulting from old T3D text being pasted into levels
        VER_FIXED_POINTLIGHTCOMPONENT_LIGHTRADIUS_AGAIN = 345,
        // VSM Shadow projection. Recompile shadow projection shaders
        VER_SHADER_VSM_SHADOW_PROJECTION = 346,
        // Changed how old name tables are loaded to split the name earlier to reduce extra FNames in memory from old packages
        VER_NAME_TABLE_LOADING_CHANGE = 347,
        // Added code to fix PointLightComponents that have an invalid PreviewLightRadius component (isn't the same component as the one contained in the owning actor's components array)
        VER_FIXED_PLC_LIGHTRADIUS_POST_COMPONENTFIX = 348,
        // Fix for permuting vertex data for FConvexElem and FConvexVolume
        VER_FIX_CONVEX_VERTEX_PERMUTE = 349,
        // Terrain serialize material resource guids
        VER_TERRAIN_SERIALIZE_MATRES_GUIDS = 350,
        // USeqAct_Interp now saves the transformations of actors it affects.
        VER_ADDED_SEQACT_INTERP_SAVEACTORTRANSFORMS = 351,
        // Emissive shader optimizations require recompiling shaders
        VER_EMISSIVE_OPTIMIZATIONS_SHADER_RECOMPILE = 352,
        // Only bloom positive scene color values on PC to mimic XBOX
        VER_DOFBLOOMGATHER_SHADER_RECOMPILE = 353,

        //	354,355
        // Recompile fog shader
        VER_HEIGHTFOG_SHADER_RECOMPILE = 355,
        // Recompile VSM filter depth gather shader
        VER_VSMFILTERGATHER_SHADER_RECOMPILE = 356,
        // Added a bUsesSceneColor flag to UMaterial
        VER_MATERIAL_USES_SCENECOLOR_FLAG = 357,
        // StaticMeshes now have physics cooked data saved in them at defined scales.
        VER_PRECACHE_STATICMESH_COLLISION = 358,
        // Terrain packed weight maps
        VER_TERRAIN_PACKED_WEIGHT_MAPS = 359,
        // Upgrade to August XDK
        VER_AUGUST_XDK_UPGRADE = 360,
        // Force recalculation of the peak active particles
        VER_RECALC_PEAK_ACTIVE_PARTICLES = 361,
        // Added setable max bone influences to GPU skin vertex factory and max influences to skel mesh chunks
        VER_GPUSKIN_MAX_INFLUENCES_OPTIMIZATION = 362,
        // Added an option to control the depth bias when rendering shadow depths
        VER_SHADOW_DEPTH_SHADER_RECOMPILE = 363,
        // Merge all static mesh vertex data into a single vertex buffer
        VER_STATICMESH_VERTEXBUFFER_MERGE = 364,
        // Precomputed 'force stream mips' array in ULevel
        VER_LEVEL_FORCE_STREAM_TEXTURES = 365,
        // Recompile FUberPostProcessBlendPixelShader to fix shadow color clamping
        VER_UBERPOSTPROCESSBLEND_PS_RECOMPILE = 366,
        // VelocityShader recompile
        VER_VELOCITYSHADER_RECOMPILE = 367,
        // Level streaming volume changes
        VER_ADDED_LEVELSTREAMINGVOLUME_USAGE = 368,
        // Changed LOADING_COMPRESSION_CHUNK_SIZE from 32K to 128K
        VER_CHANGED_COMPRESSION_CHUNK_SIZE_TO_128 = 369,
        // Terrain vertex factory shader has been changed to use MulMatrix
        VER_RECOMPILE_TERRAIN_SHADERS = 370,
        // RateScale has been added to AnimNodeSynch groups.
        VER_ANIMNODESYNCH_RATESCALE = 371,
        // Serialization of the animation compression bytestream now accounts for padding.
        VER_ANIMATION_COMPRESSION_PADDING_SERIALIZATION = 372,
        // RateScale has been added to AnimNodeSynch groups.
        VER_DOFBLOOMGATHER_DISTORTION_SHADER_RECOMPILE = 373,
        // Added code to fix SpotLightComponents that have an invalid PreviewInnerCone and PreviewOuterCone component (isn't the same component as the one contained in the owning actor's components array)
        VER_FIXED_SPOTLIGHTCOMPONENTS = 374,
        // Added UExporter::PreferredFormatIndex.
        VER_ADDED_UExPORTER_PREFFERED_FORMAT = 375,
        // Added cached cooked audio data for PS3
        VER_ADDED_CACHED_COOKED_PS3_DATA = 376,
        // Added property metadat to UClass
        VER_ADDED_PROPERTY_METADATA = 377,
        // Added property metadat to UClass
        VER_DECAL_STATIC_DECALS_SERIALIZED = 378,
        // Added Hardware PCF support, requires shadow filter shaders recompile
        VER_HARDWARE_PCF = 379,
        // Added cached cooked ogg vorbis data for the PC
        VER_ADDED_CACHED_COOKED_PC_DATA = 380,
        // Changed the filter buffer to be fixed point, which allows filtering on all hardware
        VER_FIXEDPOINT_FILTERBUFFER = 381,
        // Upgrade to XMA2
        VER_XMA2_UPGRADE = 382,
        // Upgrade to XMA2
        VER_ADDED_RAW_SURROUND_DATA = 383,
        // Fixed a problem that was causing curves to have incorrect tangents at end points.
        VER_FIXED_CURVE_INTERP_TANGENTS = 384,
        // Fixed versioning problem for SoundNodeWave
        VER_UPDATED_SOUND_NODE_WAVE = 385,
        // Remove RigidBodyIgnorePawns flag.
        VER_REMOVE_RB_IGNORE_PAWNS = 386,
        // Fixed a bug with CollisionType handling that caused some actors to have collision incorrectly turned off after being edited
        VER_COLLISIONTYPE_FIX = 387,
        // Fix for using wrong values when indexing into the VertexData array and on the duplicated verts at the end
        VER_FIX_BAD_INDEX_CONVEX_VERTEX_PERMUTE = 388,
        // Added lightmap texture coordinates to FDecalVertex.
        VER_DECAL_ADDED_DECAL_VERTEX_LIGHTMAP_COORD = 389,
        // Added NumChannels to SoundNodeWave
        VER_ADDED_NUM_CHANNELS = 390,
        // Changed terrain to be high-res edit
        VER_TERRAIN_HIRES_EDIT = 391,
        // Reversed order of gamma correction and rescaling of vertex light-maps to match texture light-maps.
        VER_VERTEX_LIGHTMAP_GAMMACORRECTION_FIX = 392,
        // Reversed order of gamma correction and rescaling of vertex light-maps to match texture light-maps.
        VER_ADDED_LOOP_INDEFINITELY = 393,
        // Added texture density material shaders
        VER_TEXTUREDENSITY = 394,
        // Added fallbacks for lack of floating point blending support.
        VER_FP_BLENDING_FALLBACK = 395,
        // Added CollisionType to ActorFactoryDynamicSM and deprecated collision flags in that class and subclasses
        VER_ADD_COLLISIONTYPE_TO_ACTORFACTORYDYNAMICSM = 396,
        // Forcing all FRawSistributions to be rebuilt (fixes very short in time curves, and optimizes 2 keyframe linear 'curves')
        VER_FDISTRIBUTION_FORCE_DIRTY2 = 397,
        // Added support for decals on GPU-skinned skeletal meshes.
        VER_DECAL_SKELETAL_MESHES = 398,
        // Added SP_PCD3D_SM2 shader platform
        VER_SM2_PLATFORM = 399,
        // Added support for decals on CPU-skinned skeletal meshes.
        VER_DECAL_CPU_SKELETAL_MESHES = 400,
        // Temporarily disabled support for unlit skinned decal materials for want of shader constants.
        VER_DECAL_DISABLED_UNLIT_MATERIALS_SKELETAL_MESHES = 401,
        // Removed registers from skiined decal vertex factory; reenabled unlit skinned decal materials.
        VER_DECAL_REMOVED_VELOCITY_REGISTERS = 402,
        // Added Fetch4 shadow shaders which are automatically used on supporting ATI cards.
        VER_IMPLEMENTED_FETCH4 = 403,
        // Moved GPU skinned decal and non-decal vertex factory shader code into the same file.
        VER_DECAL_MERGED_W_GPUSKIN_SHADER_CODE = 404,
        // TEXCOORD6,TEXCOORD7 used instead of POSITION1,NORMAL1 since those semantics are not supported by Cg.
        VER_REMOVED_POS1_NORM1_SHADER_CODE = 405,
        // Added support for static parameters
        VER_STATIC_MATERIAL_PARAMETERS = 406,
        // PS3 shader recompile for         pow()
        VER_PS3SHADER_RECOMPILE = 407,
        // Shader recompile for Full/Partial Motion Blur
        VER_FULLMOTIONBLUR_RECOMPILE = 408,
        // Added support for being able to rename material instance parameters.
        VER_IMPLEMENTED_MIC_PARAM_RENAMING = 409,
        // Added material fallbacks
        VER_MATERIAL_FALLBACKS = 410,
        // Added support for terrain morphing between tessellation levels.
        VER_TERRAIN_MORPHING_OPTION = 411,
        // PS3 now stores depth in the alpha channel.
        VER_PS3SHADER_RECOMPILE_ALPHADEPTH = 412,
        // Optimized motionblur shader
        VER_MOTIONBLUROPTIMZED = 413,
        // Have to force terrain shaders to recompile for PC...
        VER_TERRAIN_MORPHING_OPTION_RECOMPILE = 414,
        // Added DependsMap to ULinkerLoad
        VER_ADDED_LINKER_DEPENDENCIES = 415,
        // Auto-add keys for the lookup track for a movement track in matinee.
        VER_MATINEE_MOVEMENT_LOOKUP_TRACK_IMPLEMENTED = 416,
        // Added lighting channel support to BSP surfaces.
        VER_BSP_LIGHTING_CHANNEL_SUPPORT = 417,
        // Added shader change detection and other debug functions
        VER_SHADER_CRC_CHECKING = 418,
        // Changed emissive vertex light-map shader to swizzle VET_Color inputs on PS3 to make it compatible with FColor.
        VER_LIGHTMAP_PS3_BYTEORDER_FIX = 419,
        // Changed emissive vertex light-map shader to swizzle VET_Color inputs on PS3 to make it compatible with FColor.
        VER_CLEANUP_SOUNDNODEWAVE = 420,
        // Added a shared shader parameter for rendertarget color bias factor
        VER_SHADER_RENDERTARGETBIAS = 421,
        // Added tracking information about what components were dropped in order to get a fallback material to compile.
        VER_FALLBACK_DROPPED_COMPONENTS_TRACKING = 422,
        // Fixed a bug where Actors with bBlockActors and !CollisionComponent->bBlockActors would get set to COLLIDE_Touch instead of COLLIDE_CustomDefault,
        //		potentially causing the Actor's bBlockActors to be set to false incorrectly
        VER_FIXED_INCORRECT_COLLISIONTYPE = 423,
        // Fixed UIScrollbars but they all need to be recreated
        VER_FIXED_UISCROLLBARS = 424,
        // Fixed UIScrollbars but they all need to be recreated
        VER_DEPRECATED_FONIX_417 = 425,
        // Handle missing vertex factories during serialization
        VER_HANDLE_NOT_EXISTING_VERTEX_FACTORIES = 426,
        // Implemented fog volumes for all platforms
        VER_FOGVOLUMES_ALLPLATFORMS = 427,
        // Serialize enums by name	(bad change)
        VER_ENUM_VALUE_SERIALIZED_BY_NAME = 428,
        // Removed redundant UI behavior flag
        VER_REMOVED_DISALLOW_REPARENTING_FLAG = 429,
        // Added UWorld::ExtraReferencedObjects
        VER_ADDED_WORLD_EXTRA_REFERENCED_OBJECTS = 430,
        // Added support for translucency in fog volumes
        VER_TRANSLUCENCY_IN_FOG_VOLUMES = 431,
        // Changed the class of the Increment/Decrement buttons in UIScrollbar; need to be recreated
        VER_FIXED_UISCROLLBAR_BUTTONS = 432,
        // Updated foliage vertex factory shader
        VER_FOLIAGE_VERTEX_FACTORY_INSTANCING_SHADER = 433,
        // Added support for static mesh vertex colors
        VER_STATICMESH_VERTEXCOLOR = 434,
        // Add support for per-poly collision checks against specified rigid sections of skel meshes
        VER_SKELMESH_BONE_KDOP = 435,
        // Shared vertex shader parameters
        VER_SHARED_SHADER_PARAMS = 436,
        // April 2007 XDK upgrade requires tossing cooked audio data
        VER_APRIL_2007_XDK_UPGRADE = 437,
        // Recompile the uberpostprocess blend pixel shader
        VER_UBERPOSTPROCESSBLEND_PS_RECOMPILE_2 = 438,
        // Added support for translucency lit by light-map
        VER_TRANSLUCENCY_LIT_BY_LIGHTMAP = 439,
        // Added support for lit decals on terrain.
        VER_ADDED_LIT_TERRAIN_DECALS = 440,
        // Merge sprite and subUV particles into a single shader
        VER_PARTICLE_SPRITE_SUBUV_MERGE = 441,
        // Added SpeedTree static lighting support
        VER_SPEEDTREE_STATICLIGHTING = 442,
        // Rescale and compress particle thumbnails
        VER_RESCALE_AND_COMPRESS_PARTICLE_THUMBNAILS = 443,
        // Changed UIRoot.DockingSet.DockPadding to a UIScreenValue
        VER_CHANGED_DOCKPADDING_VARTYPE = 444,
        // Added shaders and lightmaps for simple lighting.
        VER_ADDED_SIMPLE_LIGHTING = 445,
        // Recompile GPU skin morph blending vertex factories
        VER_GPU_SKIN_MORPH_VF_RECOMPILE = 446,
        // Move particle materials to RequiredModule (to allow LODing)
        VER_PARTICLE_MATERIALS_TO_REQUIRED_MODULE = 447,
        // Integrated SpeedTree vertex shader rendering
        VER_SPEEDTREE_VERTEXSHADER_RENDERING = 448,
        // Changed enum serialization to be by name
        VER_ENUM_VALUE_SERIALIZED_BY_NAMEV2 = 449, // krowe, i added V2

        // Force distributions to be rebuilt.
        VER_FDISTRIBUTION_FORCE_DIRTY3 = 450,
        // Recompile DistortionApply and DepthOnly shaders to apply optimizations.
        VER_DISTORTION_AND_DEPTHONLY_RECOMPILE = 451,
        // Terrain-related vertex factories now control lightmap specular via ModifyCompilationEnvironment.
        VER_TERRAIN_VERTEX_FACTORIES_LIGHTMAP_SPECULAR = 452,
        // Sanity checking information for BulkSerialize
        VER_ADDED_BULKSERIALIZE_SANITY_CHECKING = 453,
        // Removed zone mask
        VER_REMOVED_ZONEMASK = 454,
        // Recompile PS3 shaders for trimming and optimizations.
        VER_PS3_SHADER_RECOMPILE = 455,
        // Resave material compile errors to remove expression references.
        VER_MATERIAL_ERROR_RESAVE = 456,
        // Changed all UIScreenValue members into UIScreenValue_Extent members
        VER_CHANGED_SCREENVALUE_VARTYPE = 457,
        // Fixed morphing terrain on PS3
        VER_PS3_MORPH_TERRAIN = 458,
        // Upgraded to the April 07 DirectX SDK
        VER_APRIL07_DXSDK_UPGRADE = 459,
        // Downgraded to Oct 06 DirectX SDK
        VER_OCT06_DXSDK_DOWNGRADE = 460,
        // Changed native serialization in UIDynamicDataProvidder
        VER_ADDED_COLLECTION_DATA = 461,
        // Fog Volumes affect particles again
        VER_REENABLED_PARTICLE_FOGGING = 462,
        // Remove cooked terrain data (using heightfield now)
        VER_REMOVE_COOKED_PHYS_TERRAIN = 463,
        // Added ComponentElementIndex to FBSPNode
        VER_ADDED_COMPONENT_ELEMENT_INDEX = 464,
        // Terrain decal tangents now correctly oriented.
        VER_TERRAIN_DECAL_TANGENTS = 465,
        // Separated static mesh positions into a separate buffer
        VER_SEPARATED_STATIC_MESH_POSITIONS = 466,
        // Removed PrimitiveComponent's transient WorldToLocal Matrix to save memory since the FPrimitiveSceneInfo's cached copy is what will be used most
        VER_REMOVED_PRIMITIVE_COMPONENT_WORLD_TO_LOCAL = 467,
        // Changed k-dop indices back to WORD
        VER_KDOP_DWORD_TO_WORD = 468,
        // Added material position transform
        VER_MATERIAL_POSITION_TRANSFORM = 469,
        // Add new BVTree structure for terrain collision
        VER_ADD_TERRAIN_BVTREE = 470,
        // Terrain patch bounds get generated 'on-demand'
        VER_TERRAIN_PATCHBOUNDS_ONDEMAND = 471,
        // ===

        MovedColorFromUVItem = 472,
        AddedCastShadow = 473,
        AddedFullPrecisionUV = 474,
        AddedPackageFlags = 475,
        AddedRemovedNormal = 477,
        AddedPackageSource = 482,

        // === versions missing (LMK if you find it)

        // Min version for content resave
        VER_CONTENT_RESAVE_AUGUST_2007_QA_BUILD = 491,
        // Static mesh version bump, package version bumped to ease resaving
        VER_STATICMESH_VERSION_16 = 492,
        // Used 16 bit float UVs for skeletal meshes
        VER_USE_FLOAT16_SKELETAL_MESH_UVS = 493,
        // Store two tangent basis vectors instead of three to save memory (skeletal mesh vertex buffers)
        VER_SKELETAL_MESH_REMOVE_BINORMAL_TANGENT_VECTOR = 494,
        // Terrain collision data stored in world space.
        VER_TERRAIN_COLLISION_WORLD_SPACE = 495,
        // Removed DecalManager ref from UWorld
        VER_REMOVED_DECAL_MANAGER_FROM_UWORLD = 496,
        // Modified SpeedTree vertex factory shader parameters.
        VER_SPEEDTREE_SHADER_CHANGE	= 497,
        // Fix height-fog pixel shader 4-layer
        VER_HEIGHTFOG_PIXELSHADER_START_DIST_FIX = 498,
        // MotionBlurShader recompile (added clamping to render target extents)
        VER_MOTIONBLURSHADER_RECOMPILE_VER2 = 499,
        // Separate pass for LDR BLEND_Modulate transparency mode
        // Modulate preserves dest alpha (depth)
        VER_SM2_BLENDING_SHADER_FIXES = 500,
        // Terrain material fallback support
        VER_ADDED_TERRAIN_MATERIAL_FALLBACK = 501,
        // Added support for multi-column collections to UIDynamicFieldProvider
        VER_ADDED_MULTICOLUMN_SUPPORT = 503,
        // Serialize cached displacement values for terrain
        VER_TERRAIN_SERIALIZE_DISPLACEMENTS = 504,
        // Fixed bug which allowed multiple instances of a UIState class get added to style data maps
        VER_REMOVED_PREFAB_STYLE_DATA = 505,
        // Exposed separate horizontal and vertical texture scale for material texture lookups
        //  Various font changes that affected serialization
        VER_FONT_FORMAT_AND_UV_TILING_CHANGES = 506,
        // Changed UTVehicleFactory to use a string for class reference in its defaults
        VER_UTVEHICLEFACTORY_USE_STRING_CLASS = 507,
        // Fixed the special 0.0f value in the velocity buffer that is used to select between background velocity or dynamic velocity
        VER_BACKGROUNDVELOCITYVALUE = 508,
        // Reset vehicle usage flags on some NavigationPoints that had been incorrectly set
        VER_FIXED_NAV_VEHICLE_USAGE_FLAGS = 509,
        // Changed Texture2DComposite to inherit from Texture instead of Texture2D.
        VER_TEXTURE2DCOMPOSITE_BASE_CHANGE = 510,
        // Fixed fonts serializing all members twice.
        VER_FIXED_FONTS_SERIALIZATION = 511,
        // -
        VER_STATICMESH_FRAGMENTINDEX = 514,
        // Added Draw SkelTree Manager. Added FColor to FMeshBone serialization.
        VER_SKELMESH_DRAWSKELTREEMANAGER = 515,
        // Added AdditionalPackagesToCook to FPackageFileSummary
        VER_ADDITIONAL_COOK_PACKAGE_SUMMARY = 516,
        // Add neighbor info to FFragmentInfo
        VER_FRAGMENT_NEIGHBOUR_INFO = 517,
        // Added interior fragment index
        VER_FRAGMENT_INTERIOR_INDEX = 518,
        // Added bCanBeDestroyed and bRootFragment
        VER_FRAGMENT_DESTROY_FLAGS = 519,
        // Add exterior surface normal and neighbor area info to FFragmentInfo
        VER_FRAGMENT_EXT_NORMAL_NEIGH_DIM = 520,
        // Add core mesh 3d offset and scale
        VER_FRACTURE_CORE_SCALE_OFFSET = 521,
        // Moved particle SpawnRate and Burst info into their own module.
        VER_PARTICLE_SPAWN_AND_BURST_MOVE = 523,
        // Share modules across particle LOD levels where possible.
        VER_PARTICLE_LOD_MODULE_SHARE = 524,
        // Fixing up TypeData modules not getting pushed into lower LODs
        VER_PARTICLE_LOD_MODULE_TYPEDATA_FIXUP = 525,
        // Save off PlaneBias with FSM
        VER_FRACTURE_SAVE_PLANEBIAS = 526,
        // Fixing up LOD distributions... (incorrect archetypes caused during Spawn conversion)
        VER_PARTICLE_LOD_DIST_FIXUP = 527,
        // Changed default DiffusePower value
        VER_DIFFUSEPOWER_DEFAULT = 529,
        // Allow for '0' in the particle burst list CountLow slot...
        VER_PARTICLE_BURST_LIST_ZERO = 530,
        // Added AttenAllowedParameter to FModShadowMeshPixelShader
        VER_MODSHADOWMESHPIXELSHADER_ATTENALLOWED = 531,
        // Support for mesh simplification tool.  Static mesh version bump (added named reference to high res source mesh.)
        VER_STATICMESH_VERSION_18 = 532,
        // Added automatic fog volume components to simplify workflow
        VER_AUTOMATIC_FOGVOLUME_COMPONENT = 533,
        // Added an optional array of skeletal mesh weights/bones for instancing
        VER_ADDED_EXTRA_SKELMESH_VERTEX_INFLUENCES = 534,
        // Added an optional array of skeletal mesh weights/bones for instancing
        VER_UNIFORM_DISTRIBUTION_BAKING_UPDATE = 535,
        // Replaced classes for sequences associated with PrefabInstances
        VER_FIXED_PREFAB_SEQUENCES = 536,
        // Changed FInputKeyAction's list of sequence actions to a list of sequence output links
        VER_MADE_INPUTKEYACTION_OUTPUT_LINKS = 537,
        // Moved global shaders from UShaderCache to a single global shader cache file.
        VER_GLOBAL_SHADER_FILE = 538,
        // Using MSEnc to encode mp3s rather than MP3Enc
        VER_MP3ENC_TO_MSENC = 539,
        // Added optional external specification of static vertex normals.
        VER_STATICMESH_EXTERNAL_VERTEX_NORMALS = 541,
        // Removed 2x2 normal transform for decal materials
        VER_DECAL_MATERIAL_IDENDITY_NORMAL_XFORM = 542,
        // Removed FObjectExport::ComponentMap
        VER_REMOVED_COMPONENT_MAP = 543,
        // Fixed back uniform distributions with lock flags set to something other than NONE
        VER_LOCKED_UNIFORM_DISTRIBUTION_BAKING = 544,
        // Fixed Kismet sequences with illegal names
        VER_FIXED_KISMET_SEQUENCE_NAMES = 545,
        // Added fluid lightmap support
        VER_ADDED_FLUID_LIGHTMAPS = 546,
        // Fixing up LODValidity and spawn module outers...
        VER_EMITTER_LODVALIDITY_FIX2 = 547,
        // Add FSM core rotation and 'no physics' flag on chunks
        VER_FRACTURE_CORE_ROTATION_PERCHUNKPHYS = 549,
        // New curve auto-tangent calculations; Clamped auto tangent support
        VER_NEW_CURVE_AUTO_TANGENTS = 550,
        // Removed 2x2 normal transform from decal vertices
        VER_DECAL_REMOVED_2X2_NORMAL_TRANSFORM = 551,
        // Updated decal vertex factories
        VER_DECAL_VERTEX_FACTORY_VER1 = 552,
        // Updated decal vertex factories
        VER_DECAL_VERTEX_FACTORY_VER2 = 554,
        // Updated the fluid detail normalmap
        VER_FLUID_DETAIL_UPDATE = 555,
        // Fixup particle systems with incorrect distance arrays...
        VER_PARTICLE_LOD_DISTANCE_FIXUP = 556,
        // Added FSM build version
        VER_FRACTURE_NONCRITICAL_BUILD_VERSION = 557,
        // Added DynamicParameter support for particles
        VER_DYNAMICPARAMETERS_ADDED = 558,
        // Added travelspeed parameter to the fluid detail normalmap
        VER_FLUID_DETAIL_UPDATE2 = 559,
        // /** replaced bAcceptsDecals,bAcceptsDecalsDuringGameplay with bAcceptsStaticDecals,bAcceptsDynamicDecals */
        VER_UPDATED_DECAL_USAGE_FLAGS = 560,
        // Made bOverrideNormal override the full tangent basis.
        VER_OVERRIDETANGENTBASIS = 563,
        // Made LightComponent bounced lighting settings multiplicative with direct lighting.
        VER_BOUNCEDLIGHTING_DIRECTMODULATION = 564,
        // Reduced FStateFrame::LatentAction to WORD
        VER_REDUCED_STATEFRAME_LATENTACTION_SIZE = 566,
        // Added GUIDs for updating texture file cache
        VER_ADDED_TEXTURE_FILECACHE_GUIDS = 567,
        // Fixed scene color and scene depth usage
        VER_FIXED_SCENECOLOR_USAGE = 568,
        // Renamed UPrimitiveComponent::CullDistance to MaxDrawDistance
        VER_RENAMED_CULLDISTANCE = 569,
        // Fixing up InterpolationMethod mismatches in emitter LOD levels...
        VER_EMITTER_INTERPOLATIONMETHOD_FIXUP = 570,
        // Fixing up LensFlare ScreenPercentageMaps
        VER_LENSFLARE_SCREENPERCENTAGEMAP_FIXUP = 571,
        // Reimplemented particle LOD check distance time
        VER_PARTICLE_LOD_CHECK_DISTANCE_TIME_FIX = 573,
        // Decal physical material entry fixups
        VER_DECAL_PHYS_MATERIAL_ENTRY_FIXUP = 574,
        // Added persisitent FaceFXAnimSet to the world...
        VER_WORLD_PERSISTENT_FACEFXANIMSET = 575,
        // depcreated redundant editor window position
        // Delete var - SkelControlBase: ControlPosX, ControlPosY, MaterialExpression: EditorX, EditorY
        VER_DEPRECATED_EDITOR_POSITION = 576,
        // moved RawAnimData serialization to native
        VER_NATIVE_RAWANIMDATA_SERIALIZATION = 577,
        // deprecated sound attenuation ranges
        VER_DEPRECATE_SOUND_RANGES = 578,
        // new format stored in the XMA2 file to avoid runtime calcs
        VER_XAUDIO2_FORMAT_UPDATE = 581,
        // flip the normal for meshes with negative non-uniform scaling
        VER_VERTEX_FACTORY_LOCALTOWORLD_FLIP = 582,
        // add additional sort flags to sprite/subuv particle emitters
        VER_NEW_PARTICLE_SORT_MODES = 583,
        // added asset thumbnails to packages
        VER_ASSET_THUMBNAILS_IN_PACKAGES = 584,
        // Added Pylon list to Ulevel
        VER_PYLONLIST_IN_ULEVEL = 585,
        // Added local object version number to ULevel and NavMesh
        VER_NAVMESH_COVERREF = 586,
        // poly height var added to polygons in navmesh
        VER_NAVMESH_POLYHEIGHT = 588,
        // simple element shader recompile
        VER_SIMPLE_ELEMENT_SHADER_VER0 = 589,
        // added rectangular thumbnail support
        VER_RECTANGULAR_THUMBNAILS_IN_PACKAGES = 590,
        // changed default for SkeletalMeshActor.bCollideActors to FALSE
        VER_REMOVED_DEFAULT_SKELETALMESHACTOR_COLLISION = 591,
        // added skeletalmesh position compression saving 8 bytes
        VER_SKELETAL_MESH_SUPPORT_PACKED_POSITION = 592,
        // removed content tags from objects (obsolete by new asset database system)
        VER_REMOVED_LEGACY_CONTENT_TAGS = 593,
        // added back refs for SplineActors
        VER_ADDED_SPLINEACTOR_BACK_REFS = 594,
        // Changed the format of the base pose for additive animations.
        VER_NEW_BASE_POSE_ADDITIVE_ANIM_FORMAT = 595,
        // Fix up 'Bake and Prune' animations where their num frames doesn't match NumKeys.
        VER_FIX_BAKEANDPRUNE_NUMFRAMES = 596,
        // added full names to package thumbnails
        VER_CONTENT_BROWSER_FULL_NAMES = 597,
        // added profiling system to AnimTree previewing
        VER_ANIMTREE_PREVIEW_PROFILES = 598,
        // added triangle sorting options to skeletal meshes
        VER_SKELETAL_MESH_SORTING_OPTIONS = 599,
        // Lightmass serialization changes
        VER_INTEGRATED_LIGHTMASS = 600,
        // added BoneAtom quaternion math support and convert vars from Matrix
        VER_FBONEATOM_QUATERNION_TRANSFORM_SUPPORT = 601,
        // deprecate distributions from sound nodes
        VER_DEPRECATE_SOUND_DISTRIBUTIONS = 602,
        // added DontSortCategories option to classes
        VER_DONTSORTCATEGORIES_ADDED = 603,
        // Reintroduced lossless compression of Raw Data, and removed redundant KeyTimes array.
        VER_RAW_ANIMDATA_REDUX = 604,
        // Fixed bad additive animation base pose data
        VER_FIXED_BAD_ADDITIVE_DATA = 605,
        // Add per-poly procbuilding ruleset pointer
        VER_ADD_FPOLY_PBRULESET_POINTER = 606,
        // Added precomputed lighting volume to each level
        VER_GI_CHARACTER_LIGHTING = 607,
        // SkeletalMesh Compose now done in 3 passes as opposed to 2.
        VER_THREE_PASS_SKELMESH_COMPOSE = 608,
        // Added bone influence mapping data per bone break
        VER_ADDED_EXTRA_SKELMESH_VERTEX_INFLUENCE_MAPPING = 609,
        // Fix bad AnimSequences.
        VER_REMOVE_BAD_ANIMSEQ = 610,
        // added editor data to sound classes
        VER_SOUND_CLASS_SERIALISATION_UPDATE = 613,
        // older maps may have improper ProcBuilding textures
        VER_NEED_TO_CLEANUP_OLD_BUILDING_TEXTURES = 614,
        // Mesh paint system
        VER_MESH_PAINT_SYSTEM = 615,
        VER_MESH_PAINT_SYSTEM_ENUM = VER_MESH_PAINT_SYSTEM,
        // Added ULightMapTexture2D::bSimpleLightmap
        VER_LIGHTMAPTEXTURE_VARIABLE = 616,
        // Normal shadows on the dominant light
        VER_DOMINANTLIGHT_NORMALSHADOWS = 617,
        // Added PlatformMeshData to mesh elements (for PS3 Edge Geometry support)
        VER_ADDED_PLATFORMMESHDATA = 618,
        // changed makeup of FPolyReference
        VER_FPOLYREF_CHANGE = 620,
        // Added bsp element index to the serialized static receiver data for decals
        VER_DECAL_SERIALIZE_BSP_ELEMENT = 621,
        // Added support for automatic, safe cross-level references
        VER_ADDED_CROSSLEVEL_REFERENCES = 623,
        // Changed lightmap encoding to only use two DXT1 textures for directional lightmaps
        VER_MAXCOMPONENT_LIGHTMAP_ENCODING = 624,
        // Added instanced rendering to localvertexfactory
        VER_XBOXINSTANCING = 625,
        // Fixing up emitter editor color issue.
        VER_FIXING_PARTICLE_EMITTEREDITORCOLOR = 626,
        // Added OriginalSizeX/Y to Texture2D
        VER_ADDED_TEXTURE_ORIGINAL_SIZE = 627,
        // Added options to generate particle normals from simple shapes
        VER_ANALYTICAL_PARTICLE_NORMALS = 628,
        // Fixup references to removed deprecated ParticleEmitter.SpawnRate
        VER_REMOVED_EMITTER_SPAWNRATE = 630,
        // Add support for static normal parameters
        VER_ADD_NORMAL_PARAMETERS = 631,
        // Changed UParticleSystem::bLit to be per-LOD
        VER_PARTICLE_LIT_PERLOD = 632,
        // Changed byte property serialization to include the enum the property uses (if any)
        VER_BYTEPROP_SERIALIZE_ENUM = 633,
        // Added InternalFormatLODBias
        VER_ADDED_TEXTURE_INTERNALFORMATLODBIAS = 634,
        // Added an explicit emissive light radius
        VER_ADDDED_EXPLICIT_EMISSIVE_LIGHT_RADIUS = 636,
        // Enabled Custom Thumbnails for shared thumbnail asset types
        VER_ENABLED_CUSTOM_THUMBNAILS_FOR_SHARED_TYPES = 637,
        // Added AnimMetaData system to AnimSequence, auto conversion of BoneControlModifiers to that new system.
        // Fixed FQuatError, automatic animation recompression when needed.
        VER_ADDED_ANIM_METADATA_FIXED_QUATERROR = 638,
        // Changed UStruct serialization to include both on-disk and in-memory bytecode size
        VER_USTRUCT_SERIALIZE_ONDISK_SCRIPTSIZE = 639,
        // Added support for spline mesh offsetting
        VER_ADDED_SPLINE_MESH_OFFSET = 642,
        // Speedtree 5.0 integration
        VER_SPEEDTREE_5_INTEGRATION = 643,
        // Added selected object coloring to Lightmap Density rendering mode
        VER_LIGHTMAP_DENSITY_SELECTED_OBJECT = 644,
        // Added LightmapUVs expression
        VER_MATEXP_LIGHTMAPUVS_ADDED = 645,
        // Switched AnimMetadata_SkelControl to using a list.
        VER_SKELCONTROL_ANIMMETADATA_LIST = 646,
        // Added material vertex shader parameters
        VER_MATERIAL_EDITOR_VERTEX_SHADER = 647,
        // Fixed hit proxy material parameters not getting serialized
        VER_FIXED_HIT_PROXY_VERTEX_OFFSET = 650,
        // Added general OcclusionPercentage material expression
        VER_ADDDED_OCCLUSION_PERCENTAGE_EXPRESSION = 651,
        // Added the ability to shadow indirect only in Lightmass
        VER_SHADOW_INDIRECT_ONLY_OPTION = 652,
        // Changed mesh emitter camera facing options...
        VER_MESH_EMITTER_CAMERA_FACING_OPTIONS = 653,
        // Replaced bSimpleLightmap with LightmapFlags in ULightMapTexture2D
        VER_LIGHTMAPFLAGS = 654,
        // Added the ability for script to bind DLL functions
        VER_SCRIPT_BIND_DLL_FUNCTIONS = 655,
        // Moved uniform expressions from being stored in the UMaterial package to the shader cache
        VER_UNIFORM_EXPRESSIONS_IN_SHADER_CACHE = 656,
        // Added dynamic parameter support and second uv set to beams and trails
        VER_BEAM_TRAIL_DYNAMIC_PARAMETER = 657,
        // Allow random overrides per-section in ProcBuilding meshes
        VER_PROCBUILDING_MATERIAL_OPTIONS = 659,
        // Changed uniform expressions to reference textures by index instead of name
        VER_UNIFORMEXPRESSION_TEXTUREINDEX = 660,
        // Regenerate texture array for old materials, so they match the shadercache.
        VER_UNIFORMEXPRESSION_POSTLOADFIXUP = 661,
        // Separated DOF and Bloom, invalidate shadercache.
        VER_SEPARATE_DOF_BLOOM = 662,
        // Change AnimNotify_Trails to use SamplesPerSecond
        VER_ANIMNOTIFY_TRAIL_SAMPLEFRAMERATE = 664,
        // Support for attaching static decals to instanced static meshes
        VER_STATIC_DECAL_INSTANCE_INDEX = 665,
        // Added support for precomputed shadowmaps to lit decals
        // Teh Forbidden= ?,
        VER_DECAL_SHADOWMAPS = 666,
        // Fixed malformed raw anim data
        VER_FIXED_MALFORMED_RAW_ANIM_DATA = 667,
        // Removed unused velocity values from AnimNotify_Trail sampled data
        VER_ANIMNOTIFY_TRAILS_REMOVED_VELOCITY = 668,
        // Added SpawnRate support to Ribbon emitters
        VER_RIBBON_EMITTERS_SPAWNRATE = 669,
        // Remove ruleset from FPoly and add 'variation name' instead
        VER_FPOLY_RULESET_VARIATIONNAME = 670,
        // Added PreViewTranslationParameter in FParticleInstancedMeshVertexFactoryShaderParameters
        VER_ADDED_PRE_VIEW_TRANSLATION_PARAMETER = 671,
        // Added shader compression functionality
        VER_SHADER_COMPRESSION = 672,
        // Optimized FPropertyTag to store bool properties with 1 byte on disk instead of 4
        VER_PROPERTYTAG_BOOL_OPTIMIZATION = 673,
        // Added iPhone cached data (PVRTC textures)
        VER_ADDED_CACHED_IPHONE_DATA = 674,
        // Fixup for ForceFeedbackSerialization
        VER_FORCEFEEDBACKWAVERFORM_NOEXPORT_CHANGE = 677,
        // Changed type OverrideVertexColors from TArray<FColor> to FColorVertexBuffer *
        VER_OVERWRITE_VERTEX_COLORS_MEM_OPTIMIZED = 678,
        // Changed the default usage to be SVB_LoadingAndVisibility for level streaming volumes.
        VER_STREAMINGVOLUME_USAGE_DEFAULT = 679,
        // Added support to serialize clothing asset properties.
        VER_APEX_CLOTHING = 680,
        // Added support to serialize destruction cached data
        VER_APEX_DESTRUCTION = 681,
        // Added spotlight dominant shadow transition handling
        VER_SPOTLIGHT_DOMINANTSHADOW_TRANSITION = 682,
        // Added support for preshadows on translucency
        VER_TRANSLUCENT_PRESHADOWS = 685,
        // Removed shadow volume support
        VER_REMOVED_SHADOW_VOLUMES = 686,
        // Bulk serialize instance data
        VER_BULKSERIALIZE_INSTANCE_DATA = 688,
        // Added TerrainVertexFactory TerrainLayerCoordinateOffset Parameter
        VER_ADDED_TERRAINLAYERCOORDINATEOFFSET_PARAM = 689,
        // Added CachedPhysConvexBSPData in ULevel for Convex BSP
        VER_CONVEX_BSP = 690,
        // Reduced ProbeMask in UState/FStateFrame to DWORD and removed IgnoreMask
        VER_REDUCED_PROBEMASK_REMOVED_IGNOREMASK = 691,
        // Changed way material references are stored/handled for Matinee material parameter tracks
        VER_CHANGED_MATPARAMTRACK_MATERIAL_REFERENCES = 693,
        // Added bone influence mapping option per bone break
        VER_ADDED_EXTRA_SKELMESH_VERTEX_INFLUENCE_CUSTOM_MAPPING = 694,
        // Changed GDO lighting defaults to be cheap
        VER_CHANGED_GDO_LIGHTING_DEFAULTS2 = 696,
        // Added chunks/sections when swapping to a vertex influence using IWU_FullSwap
        VER_ADDED_CHUNKS_SECTIONS_VERTEX_INFLUENCE = 700,
        // Half scene depth parameter got serialized
        VER_HALFSCENE_DEPTH_PARAM = 705,
        // introduced VisualizeTexture shader
        VER_VISUALIZETEXTURE = 706,
        // updated bink shader serialization
        VER_BINK_SHADER_SERIALIZATION_CHANGE = 707,
        // Added RequiredBones array to extra vertex influence structure
        VER_ADDED_REQUIRED_BONES_VERTEX_INFLUENCE = 708,
        // Added multiple UV channels to skeletal meshes
        VER_ADDED_MULTIPLE_UVS_TO_SKELETAL_MESH = 709,
        // Added ability to render and import skeletal meshes with vertex colors
        VER_ADDED_SKELETAL_MESH_VERTEX_COLORS = 710,
        // Removed SM2 support
        VER_REMOVED_SHADER_MODEL_2 = 711,
        // Removed terrain displacement mapping
        VER_TERRAIN_REMOVED_DISPLACEMENTS = 713,
        // Added FStaticTerrainLayerWeightParameter
        VER_ADD_TERRAINLAYERWEIGHT_PARAMETERS = 714,
        // Added usage specification to vertex influences
        VER_ADDED_USAGE_VERTEX_INFLUENCE = 715,
        // Added support for camera offset particles
        VER_PARTICLE_ADDED_CAMERA_OFFSET = 716,
        // Resolution independent light shafts
        VER_RES_INDEPENDENT_LIGHTSHAFTS = 720,
        // Lightmaps on GDOs
        VER_GDO_LIGHTMAPS = 721,
        // Explicit normal support for static meshes
        VER_STATIC_MESH_EXPLICIT_NORMALS = 723,
        // Reverted HalfRes MotionBlur&DOF for now
        VER_HALFRES_MOTIONBLURDOF4 = 727,
        // MotionBlurSeperatePass back in again
        VER_HALFRES_MOTIONBLURDOF5 = 729,
        // bump the version to prevent error message
        VER_REMOVED_SEPARATEBLOOM2 = 731,
        // Fixed GDO FLightmapRef handling
        VER_FIXED_GDO_LIGHTMAP_REFCOUNTING = 732,
        // Precomputed Visibility
        VER_PRECOMPUTED_VISIBILITY = 734,
        // sets the StartTime on MITVs to -1 when they were created with that var being transient
        VER_MITV_START_TIME_FIX_UP = 735,
        // Add lightmap to LandscapeComponent
        VER_LANDSCAPECOMPONENT_LIGHTMAPS = 737,
        // Non uniform precomputed visibility
        VER_NONUNIFORM_PRECOMPUTED_VISIBILITY = 739,
        // Object based Motion Blur scale fix
        VER_IMPROVED_MOTIONBLUR2 = 740,
        // Object based Motion Blur scale fix
        VER_HITMASK_MIRRORING_SUPPORT = 741,
        // Fixed RadialBlur look
        VER_RADIALBLUR_FIX = 743,
        // Add Landscape vertex factory LodBias Parameter
        VER_LANDSCAPEVERTEXFACTORY_ADD_LODBIAS_PARAM = 744,
        // Optimized AngleBasedSSAO, better quality
        VER_IMPROVED_ANGLEBASEDSSAO = 746,
        // Optimized AngleBasedSSAO
        VER_IMPROVED_ANGLEBASEDSSAO2 = 747,
        // New character indirect lighting controls
        VER_CHARACTER_INDIRECT_CONTROLS = 748,
        // Add force script defined ordering per class
        VER_FORCE_SCRIPT_DEFINED_ORDER_PER_CLASS = 749,
        // Optimized SSAO SmartBlur making 2 pass
        VER_OPTIMIZEDSSAO = 750,
        // One pass approximate lighting for translucency
        VER_ONEPASS_TRANSLUCENCY_LIGHTING = 754,
        // Moved UField::SuperField to UStruct
        VER_MOVED_SUPERFIELD_TO_USTRUCT = 756,
        // Support AnimNodeSlot dynamic sequence node allocation on demand
        VER_ADDED_ANIMNODESLOTPOOL = 760,
        // Optimized UAnimSequence storage
        VER_OPTIMIZED_ANIMSEQ = 761,
        // removed Direction from cover reference
        VER_REMOVED_DIR_COVERREF = 763,
        // Fixed GDO's getting lighting unbuilt when Undestroyed
        VER_GDO_LIGHTING_HANDLE_UNDESTROY = 764,
        // Added option for per bone motion blur, made pow() for non PS3 platforms unclamped
        VER_PERBONEMOTIONBLUR = 766,
        // Added async texture pre-allocation to level streaming
        VER_TEXTURE_PREALLOCATION = 767,
        // Added property to specify bone to use for TRISORT_CustomLeftRight
        VER_ADDED_SKELETAL_MESH_SORTING_LEFTRIGHT_BONE = 768,
        // Added new feature: SoftEdge MotionBlur
        VER_SOFTEDGEMOTIONBLUR = 769,
        // Compact kDop trees for static meshes
        VER_COMPACTKDOPSTATICMESH = 770,
        // Refactoring UberPostProcess, removed unused parameters
        VER_UBERPOST_REFACTOR2 = 773,
        // Added XY offset parameters to Landscape vertex factory
        VER_LANDSCAPEVERTEXFACTORY_ADD_XYOFFSET_PARAMS = 774,
        // Replaced tonemapper checkbox by combobox
        VER_TONEMAPPER_ENUM = 779,
        // Fix distortion effect wrong color leaking in
        VER_DISTORTIONEFFECT2 = 780,
        // Fixed translucent preshadow filtering
        VER_FIXED_TRANSLUCENT_SHADOW_FILTERING = 783,
        // Added vfetch sprite and subuv particle support on 360
        VER_SPRITE_SUBUV_VFETCH_SUPPORT = 784,
        // fixed warning with MotionBlurSkinning
        VER_MOTIONBLURSKINNING = 787,
        // adjustable kernel for ReferenceDOF
        VER_POSTPROCESSUPDATE = 788,
        // Added class group names for grouping in the editor
        VER_ADDED_CLASS_GROUPS = 789,
        // Bloom after motionblur for better quality
        VER_BLOOM_AFTER_MOTIONBLUR = 790,
        // MotionBlurSoftEdge fix bias on NV 7800 cards
        VER_IMPROVED_MOTIONBLUR6 = 792,
        // MotionBlur optimizations
        VER_IMPROVED_MOTIONBLUR7 = 793,
        // Removed unused parameter
        VER_REMOVE_MAXBONEINFLUENCE = 794,
        // Fixed automatic shader versioning
        VER_FIXED_AUTO_SHADER_VERSIONING = 796,
        // Added texture instances for non-static actors in ULevel::BuildStreamingData().
        VER_DYNAMICTEXTUREINSTANCES = 797,
        // Moved Guids previously stored in CoverLink (with many dups) into ULevel
        VER_COVERGUIDREFS_IN_ULEVEL = 798,
        // Fix content that lost the flag because of wrong serialization
        VER_COLORGRADING2 = 800,
        // Added code to preserve static mesh component override vertex colors when source verts change
        VER_PRESERVE_SMC_VERT_COLORS = 801,
        // Added shadowing for image based reflections
        VER_IMAGE_REFLECTION_SHADOWING = 802,
        // Added ability to keep degenerate triangles when building static mesh
        VER_KEEP_STATIC_MESH_DEGENERATES = 804,
        // Added shader cache priority
        VER_SHADER_CACHE_PRIORITY = 805,
        // Added support for 32 bit vertex indices on skeletal meshes
        VER_DWORD_SKELETAL_MESH_INDICES = 806,
        // Introduced DepthOfFieldType
        VER_DEPTHOFFIELD_TYPE = 807,
        // Fixed some serialization issues with 32 bit indices
        VER_DWORD_SKELETAL_MESH_INDICES_FIXUP = 808,
        // Changed material parameter allocation for landscape
        VER_CHANGED_LANDSCAPE_MATERIAL_PARAMS = 810,
        // fix blue rendering
        VER_INVALIDATE_SHADERCACHE1 = 812,
        // fixup estimate max particle counts
        VER_RECALCULATE_MAXACTIVEPARTICLE = 813,
        // serialize raw data info for morph target
        VER_SERIALIZE_MORPHTARGETRAWVERTSINDICES = 814,
        // fix specular on old terrain on consoles
        VER_TERRAIN_SPECULAR_FIX = 815,
        // Changed ScenColor texture format
        VER_INVALIDATE_SHADERCACHE2 = 816,
        // Added support for VertexFactoryParameters in pixel shader
        VER_INVALIDATE_SHADERCACHE3 = 817,
        // Fixup empty emitter particle systems
        VER_PARTICLE_EMPTY_EMITTERS_FIXUP = 818,
        // Renamed old actor groups to layers
        VER_RENAMED_GROUPS_TO_LAYERS = 819,
        // Deprecated some doubly serialised data
        VER_DEPRECATE_DOUBLY_SERIALISED_SMC = 820,
        // changed screendoor texture to be pixel perfect and 64x64
        VER_INVALIDATE_SHADERCACHE4 = 821,
        // Fixup the references to MobileGame package which no longer exists after the UDK/Mobile merge
        VER_FIXUP_MOBILEGAME_REFS = 822,
        // Source mesh data is saved before modification.
        VER_STATIC_MESH_SOURCE_DATA_COPY = 823,
        // Landscape Decal Factory
        VER_LANDSCAPEDECALVERTEXFACTORY = 824,
        // Remove generic ActorFactory support from GDO, only support spawning rigid body
        VER_GDO_REMOVE_ACTORFACTORY = 825,
        // Fix for static mesh components affected by a copy/paste bug with override vertex colors.
        VER_FIX_OVERRIDEVERTEXCOLORS_COPYPASTE = 826,
        // Renamed MobileGame to SimpleGame
        VER_RENAME_MOBILEGAME_TO_SIMPLEGAME = 827,
        // Fixup archetypes of distributions in auto-coverted seeded modules
        VER_FIXUP_SEEDED_MODULE_DISTRIBUTIONS = 828,
        // Expose and store more mesh optimization settings via the editor.
        VER_STORE_MESH_OPTIMIZATION_SETTINGS = 829,
        // Added extra editor data saved per foliage instance
        VER_FOLIAGE_INSTANCE_SAVE_EDITOR_DATA = 830,
        // Removed unused lighting properties
        VER_REMOVE_UNUSED_LIGHTING_PROPERTIES = 829,
        // Fixing up version as VER_REMOVE_UNUSED_LIGHTING_PROPERTIES is less than VER_FOLIAGE_INSTANCE_SAVE_EDITOR_DATA and not unique.
        VER_FIXED_UP_VERSION = 831,
        // SphereMask material expression hardness was defined wrong
        VER_SPHEREMASK_HARDNESS = 832,
        // Added UI data saved with InstancedFoliageActor
        VER_FOLIAGE_SAVE_UI_DATA = 833,
        // Support simplification of skeletal meshes.
        VER_SKELETAL_MESH_SIMPLIFICATION = 834,
        // Support physical materials on landscape.
        VER_LANDSCAPE_PHYS_MATERIALS = 835,
        // Added support for compressed pixel shaders on Playstation 3
        VER_INVALIDATE_SHADERCACHE5 = 836,
        // SphereMask serialization fix
        VER_SPHEREMASK_HARDNESS1 = 837,
        // kdop edge case fix
        VER_KDOP_ONE_NODE_FIX = 838,
        // Whether, or not, translation is included in animation sequences is now tracked
        VER_ANIM_SEQ_TRANSLATION_STATE = 839,
        // SphereMask serialization fix
        VER_SPHEREMASK_HARDNESS2 = 840,
        // Crack-free displacement support for static and skeletal meshes.
        VER_CRACK_FREE_DISPLACEMENT_SUPPORT = 841,
        // Fix crash when serializing bogus static mesh color vertex buffers.
        VER_FIX_BROKEN_COLOR_VERTEX_BUFFERS = 842,
        // Cleaning up APEX destruction variables
        VER_CLEANUP_APEX_DESTRUCTION_VARIABLES = 843,
        // Per-instance foliage selection and editing
        VER_FOLIAGE_INSTANCE_SELECTION = 844,
        // WiiU support for compressed sounds
        VER_WIIU_COMPRESSED_SOUNDS = 845,
        // Flash support for compressed sounds
        VER_FLASH_COMPRESSED_SOUNDS_DEPRECATED = 846,
        // Fixups for foliage LOD
        VER_FOLIAGE_LOD = 847,
        // Support for per-LOD lightmaps in InstancedStaticMeshComponents
        VER_INSTANCED_STATIC_MESH_PER_LOD_STATIC_LIGHTING = 848,
        // Flash branch integration
        VER_FLASH_MOBILE_FEATURES_INTEGRATION = 849,
        // Added Z offset to foliage
        VER_FOLIAGE_ADDED_Z_OFFSET = 850,
        // Added cached compressed IPhone audio
        VER_IPHONE_COMPRESSED_SOUNDS = 851,
        // Switched IPhone compressed sounds to MS-ADPCM, need to reconvert any converted sounds
        VER_IPHONE_COMPRESSED_SOUNDS_MS_ADPCM = 852,
        // Fix for Material Blend Mode override
        VER_MATERIAL_BLEND_OVERRIDE = 853,
        // THe proper version for flash audio after merge to main
        VER_FLASH_MERGE_TO_MAIN = 854,
        // Renamed all mobile material parameters so the start with 'Mobile'
        VER_MOBILE_MATERIAL_PARAMETER_RENAME = 855,
        // Allow decoupling particle image flipping from ScreenAlignment square
        VER_PARTICLE_SQUARE_IMAGE_FLIPPING = 856,
        // A missed code fix in the flash merge was making flash textures not get saved
        VER_VERSION_NUMBER_FIX_FOR_FLASH_TEXTURES = 857,
        // Replacing the SM2/SM3 material resource array with high/low quality level
        VER_ADDED_MATERIAL_QUALITY_LEVEL = 858,
        // Tag mesh proxies as such.
        VER_TAG_MESH_PROXIES = 859,
        // Put DBAVars to global vertex/pixel shader registers
        VER_REALD_DBAVARS_TO_SHADER_REGISTERS = 860,
        // Changed Flash texture caching
        VER_FLASH_DXT5_TEXTURE_SUPPORT = 861,
        // IPhone - stereo sounds decompress blocks as they are played
        VER_IPHONE_STEREO_STAYS_ADPCM_COMPRESSED = 862,
        // Added additional settings to static and skeletal optimization structures (FStaticMeshOptimizationSettings & FSkeletalMeshOptimizationSettings )
        VER_ADDED_EXTRA_MESH_OPTIMIZATION_SETTINGS = 863,
        // separate out ETC cooking from PVRTC cooking
        VER_ANDROID_ETC_SEPARATED = 864,
        // Bug in compression where left channel was being overwritten by right channel
        VER_IPHONE_STEREO_ADPCM_COMPRRESION_BUG_FIX = 865,
        // Added undo support to Substance
        VER_ALG_SBS_INPUT_INDEX = 866,

        //-IPhone adpcm compression now has a variable block size based on the quality setting in SoundNodeWave
        VER_IPHONE_AUDIO_VARIABLE_BLOCK_SIZE_COMPRESSION = 867,
        // -----<new versions can be added before this line>-------------------------------------------------

        // this needs to be the last line (see note below)
        VER_AUTOMATIC_VERSION_PLUS_ONE,
        VER_AUTOMATIC_VERSION = VER_AUTOMATIC_VERSION_PLUS_ONE - 1
    }

    public enum EUnrealEngineObjectLicenseeUEVersion
    {
        VER_LIC_NONE = 0,
        // this needs to be the last line (see note below)
        VER_LIC_AUTOMATIC_VERSION_PLUS_ONE,
        VER_LIC_AUTOMATIC_VERSION = VER_LIC_AUTOMATIC_VERSION_PLUS_ONE - 1
    }

    /// <summary>
    /// This object combines all of our version enums into a single easy to use structure
    /// which allows us to update older version numbers independently of the newer version numbers.
    /// </summary>
    public struct FPackageFileVersion :
        IComparable<EUnrealEngineObjectUE3Version>,
        IComparable<EUnrealEngineObjectUE4Version>,
        IComparable<EUnrealEngineObjectUE5Version>
    {
        /// UE3 file version
        public int FileVersionUE3;

        /// UE4 file version
        public int FileVersionUE4;

        /// UE5 file version
        public int FileVersionUE5;

        /// Set all versions to the default state
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reset()
        {
            FileVersionUE3 = 0;
            FileVersionUE4 = 0;
            FileVersionUE5 = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FPackageFileVersion(int ue3Version, int ue4Version, int ue5Version)
        {
            FileVersionUE3 = ue3Version;
            FileVersionUE4 = ue4Version;
            FileVersionUE5 = ue5Version;
        }

        /// Creates and returns a FPackageFileVersion based on a single UE3 version, UE4 version, or UE5 version
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FPackageFileVersion CreateUE3Version(int version) => new(version, 0, 0);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FPackageFileVersion CreateUE3Version(EUnrealEngineObjectUE3Version version) => new((int)version, 0, 0);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FPackageFileVersion CreateUE4Version(int version) => new(0, version, 0);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FPackageFileVersion CreateUE4Version(EUnrealEngineObjectUE4Version version) => new(0, (int)version, 0);

        public int Value
        {
            get
            {
                if (FileVersionUE5 >= (int)EUnrealEngineObjectUE5Version.INITIAL_VERSION)
                    return FileVersionUE5;
                if (FileVersionUE4 > (int)EUnrealEngineObjectUE4Version.DETERMINE_BY_GAME)
                    return FileVersionUE4;
                return FileVersionUE3;
            }
            set
            {
                if (value >= (int)EUnrealEngineObjectUE5Version.INITIAL_VERSION)
                    FileVersionUE5 = value;
                else if (value > (int)EUnrealEngineObjectUE4Version.DETERMINE_BY_GAME)
                    FileVersionUE4 = value;
                else
                    FileVersionUE3 = value;
            }
        }

        /// UE3 version comparisons
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(FPackageFileVersion a, EUnrealEngineObjectUE3Version b) => a.FileVersionUE3 == (int)b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(FPackageFileVersion a, EUnrealEngineObjectUE3Version b) => a.FileVersionUE3 != (int)b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(FPackageFileVersion a, EUnrealEngineObjectUE3Version b) => a.FileVersionUE3 < (int)b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(FPackageFileVersion a, EUnrealEngineObjectUE3Version b) => a.FileVersionUE3 > (int)b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(FPackageFileVersion a, EUnrealEngineObjectUE3Version b) => a.FileVersionUE3 <= (int)b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(FPackageFileVersion a, EUnrealEngineObjectUE3Version b) => a.FileVersionUE3 >= (int)b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(EUnrealEngineObjectUE3Version other) => FileVersionUE3.CompareTo(other);

        /// UE4 version comparisons
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(FPackageFileVersion a, EUnrealEngineObjectUE4Version b) => a.FileVersionUE4 == (int)b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(FPackageFileVersion a, EUnrealEngineObjectUE4Version b) => a.FileVersionUE4 != (int)b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(FPackageFileVersion a, EUnrealEngineObjectUE4Version b) => a.FileVersionUE4 < (int)b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(FPackageFileVersion a, EUnrealEngineObjectUE4Version b) => a.FileVersionUE4 > (int)b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(FPackageFileVersion a, EUnrealEngineObjectUE4Version b) => a.FileVersionUE4 <= (int)b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(FPackageFileVersion a, EUnrealEngineObjectUE4Version b) => a.FileVersionUE4 >= (int)b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(EUnrealEngineObjectUE4Version other) => FileVersionUE4.CompareTo(other);

        /// UE5 version comparisons
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(FPackageFileVersion a, EUnrealEngineObjectUE5Version b) => a.FileVersionUE5 == (int)b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(FPackageFileVersion a, EUnrealEngineObjectUE5Version b) => a.FileVersionUE5 != (int)b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(FPackageFileVersion a, EUnrealEngineObjectUE5Version b) => a.FileVersionUE5 < (int)b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(FPackageFileVersion a, EUnrealEngineObjectUE5Version b) => a.FileVersionUE5 > (int)b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(FPackageFileVersion a, EUnrealEngineObjectUE5Version b) => a.FileVersionUE5 <= (int)b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(FPackageFileVersion a, EUnrealEngineObjectUE5Version b) => a.FileVersionUE5 >= (int)b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(EUnrealEngineObjectUE5Version other) => FileVersionUE5.CompareTo(other);

        /// <summary>
        /// Returns true if this object is compatible with the FPackageFileVersion passed in as the parameter.
        /// This means that  all version numbers for the current object are equal or greater than the
        /// corresponding version numbers of the other structure.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsCompatible(FPackageFileVersion other) => FileVersionUE3 >= other.FileVersionUE3 && FileVersionUE4 >= other.FileVersionUE4 && FileVersionUE5 >= other.FileVersionUE5;

        /// FPackageFileVersion comparisons
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(FPackageFileVersion a, FPackageFileVersion b) => a.FileVersionUE3 == b.FileVersionUE3 && a.FileVersionUE4 == b.FileVersionUE4 && a.FileVersionUE5 == b.FileVersionUE5;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(FPackageFileVersion a, FPackageFileVersion b) => !(a == b);

        public override bool Equals(object? obj) => obj is FPackageFileVersion other && this == other;
        public override int GetHashCode() => HashCode.Combine(FileVersionUE3, FileVersionUE4, FileVersionUE5);

        public override string ToString()
            => FileVersionUE5 >= (int)EUnrealEngineObjectUE5Version.INITIAL_VERSION
                ? ((EUnrealEngineObjectUE5Version)FileVersionUE5).ToString()
                : FileVersionUE4 > (int)EUnrealEngineObjectUE4Version.DETERMINE_BY_GAME
                    ? ((EUnrealEngineObjectUE4Version)FileVersionUE4).ToString()
                    : ((EUnrealEngineObjectUE3Version)FileVersionUE3).ToString();
    }
}
