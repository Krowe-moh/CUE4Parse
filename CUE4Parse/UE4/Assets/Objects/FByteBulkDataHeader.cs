using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.IO.Objects;
using CUE4Parse.UE4.Versions;
using Newtonsoft.Json;
using static CUE4Parse.UE4.Assets.Objects.EBulkDataFlags;

namespace CUE4Parse.UE4.Assets.Objects
{
    [JsonConverter(typeof(FByteBulkDataHeaderConverter))]
    public readonly struct FByteBulkDataHeader
    {
        public readonly EBulkDataFlags BulkDataFlags;
        public readonly int ElementCount;
        public readonly uint SizeOnDisk;
        public readonly long OffsetInFile;
        public readonly FBulkDataCookedIndex CookedIndex;

        public FByteBulkDataHeader(FAssetArchive Ar)
        {
            CookedIndex = FBulkDataCookedIndex.Default;

            BulkDataFlags = Ar.Read<EBulkDataFlags>();
            ElementCount = Ar.Read<int>();
            SizeOnDisk = Ar.Read<uint>();
            OffsetInFile = Ar.Read<int>();
                OffsetInFile += Ar.Owner.Summary.BulkDataStartOffset;

        }
    }
}
