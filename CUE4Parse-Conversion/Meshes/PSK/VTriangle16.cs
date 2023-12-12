﻿using CUE4Parse.UE4.Writers;

namespace CUE4Parse_Conversion.Meshes.PSK;

public class VTriangle16 : ISerializable
{
    public readonly ushort[] WedgeIndex;
    public readonly byte MatIndex;
    public readonly byte AuxMatIndex;
    public readonly uint SmoothingGroups;

    public VTriangle16(ushort[] wedgeIndex, byte matIndex, byte auxMatIndex, uint smoothingGroups)
    {
        WedgeIndex = wedgeIndex;
        MatIndex = matIndex;
        AuxMatIndex = auxMatIndex;
        SmoothingGroups = smoothingGroups;
    }

    public void Serialize(FArchiveWriter Ar)
    {
        Ar.Write(WedgeIndex[0]);
        Ar.Write(WedgeIndex[1]);
        Ar.Write(WedgeIndex[2]);
        Ar.Write(MatIndex);
        Ar.Write(AuxMatIndex);
        Ar.Write(SmoothingGroups);
    }
}