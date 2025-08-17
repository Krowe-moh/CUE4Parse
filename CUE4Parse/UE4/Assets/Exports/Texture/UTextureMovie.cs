using System.IO;
using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Assets.Readers;

namespace CUE4Parse.UE4.Assets.Exports.Texture;

public class UTextureMovie : UTexture
{
    public override void Deserialize(FAssetArchive Ar, long validPos)
    {
        base.Deserialize(Ar, validPos);

        var bulkData = new FByteBulkData(Ar);
        // allow users to download video? works fine with https://www.radgametools.com/bnkdown.htm
    }
}