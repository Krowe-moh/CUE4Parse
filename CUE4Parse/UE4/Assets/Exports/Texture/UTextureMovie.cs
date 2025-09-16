using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Assets.Readers;

namespace CUE4Parse.UE4.Assets.Exports.Texture;

public class UTextureMovie : UTexture
{
    public override void Deserialize(FAssetArchive Ar, long validPos)
    {
        base.Deserialize(Ar, validPos);

        // Data is https://www.radgametools.com/bnkdown.htm encoding
        new FByteBulkData(Ar); // RawData
    }
}