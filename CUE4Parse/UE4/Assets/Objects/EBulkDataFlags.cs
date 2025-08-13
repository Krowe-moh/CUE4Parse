using System;

namespace CUE4Parse.UE4.Assets.Objects
{
    [Flags]
    public enum EBulkDataFlags : uint
    {
        BULKDATA_StoreInSeparateFile = 0x01, // bulk stored in different file (otherwise it's "inline")
        BULKDATA_CompressedZlib = 0x02, // corresponds to BULKDATA_SerializeCompressedZLIB (UE4)
        BULKDATA_CompressedLzo = 0x10, // unknown name
        BULKDATA_Unused = 0x20, // empty bulk block
        BULKDATA_SeparateData = 0x40, // bulk stored in a different place in the same file
        BULKDATA_CompressedLzx = 0x80 // unknown name
    }
}