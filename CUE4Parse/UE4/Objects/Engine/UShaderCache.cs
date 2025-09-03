using CUE4Parse.UE4.Assets.Exports.Material;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Objects.Engine
{
    public class UShaderCache : Assets.Exports.UObject
    {
        public override void Deserialize(FAssetArchive Ar, long validPos)
        {
            base.Deserialize(Ar, validPos);
            var Platform = 0;
            if (Ar.Ver >= EUnrealEngineObjectUE3Version.VER_GLOBAL_SHADER_FILE)
            {
                Platform = Ar.Read<byte>();
                if (Ar.Ver < EUnrealEngineObjectUE3Version.VER_FIXED_AUTO_SHADER_VERSIONING)
                {
                    var dummy = Ar.ReadMap(
                        () => new FPackageIndex(Ar),
                        () => Ar.Read<uint>()
                    );
                }
            }
            var ShaderTypeCompressedShaderCode = Ar.ReadMap(
                () => new FPackageIndex(),               // Key: FShaderType*
                () => Ar.Read<byte>()  // Value: compressed shader code struct
            );

        }
    }
}