using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Core.Math;

namespace CUE4Parse.UE4.Assets.Exports.Material;

public class FMaterialUniformExpressionAbs : FMaterialUniformExpressionPeriodic
{
    public FMaterialUniformExpressionAbs(FAssetArchive Ar)
        : base(Ar)
    {
    }
}

public class FMaterialUniformExpressionCeil : FMaterialUniformExpressionPeriodic
{
    public FMaterialUniformExpressionCeil(FAssetArchive Ar)
        : base(Ar)
    {
    }
}
public class FMaterialUniformExpressionSquareRoot : FMaterialUniformExpressionPeriodic
{
    public FMaterialUniformExpressionSquareRoot(FAssetArchive Ar)
        : base(Ar)
    {
    }
}

public class FMaterialUniformExpressionPeriodic : IUStruct
{
    public UniformExpression x { get; private set; }

    public FMaterialUniformExpressionPeriodic(FAssetArchive Ar)
    {
        x = new UniformExpression(Ar);
    }
}

public class FMaterialUniformExpressionSine : IUStruct
{
    public UniformExpression x { get; private set; }
    public bool bIsCosine { get; private set; }

    public FMaterialUniformExpressionSine(FAssetArchive Ar)
    {
        x = new UniformExpression(Ar);
        bIsCosine = Ar.ReadBoolean();
    }
}

public class FMaterialUniformExpressionClamp : IUStruct
{
    public UniformExpression Input { get; private set; }
    public UniformExpression Min { get; private set; }
    public UniformExpression Max { get; private set; }

    public FMaterialUniformExpressionClamp(FAssetArchive Ar)
    {
        Input = new UniformExpression(Ar);
        Min = new UniformExpression(Ar);
        Max = new UniformExpression(Ar);
    }
}

public class FMaterialUniformExpressionFoldedMath : IUStruct
{
    public UniformExpression A { get; private set; }
    public UniformExpression B { get; private set; }
    public byte Op { get; private set; }

    public FMaterialUniformExpressionFoldedMath(FAssetArchive Ar)
    {
        A = new UniformExpression(Ar);
        B = new UniformExpression(Ar);
        Op = Ar.Read<byte>();
    }
}

public class FMaterialUniformExpressionMin : FMaterialUniformExpressionMax
{
    public FMaterialUniformExpressionMin(FAssetArchive Ar)
        : base(Ar)
    {
    }
}

public class FMaterialUniformExpressionMax : IUStruct
{
    public UniformExpression A { get; private set; }
    public UniformExpression B { get; private set; }

    public FMaterialUniformExpressionMax(FAssetArchive Ar)
    {
        A = new UniformExpression(Ar);
        B = new UniformExpression(Ar);
    }
}

public class FMaterialUniformExpressionAppendVector : IUStruct
{
    public UniformExpression A { get; private set; }
    public UniformExpression B { get; private set; }
    public int NumComponentsA { get; private set; }

    public FMaterialUniformExpressionAppendVector(FAssetArchive Ar)
    {
        A = new UniformExpression(Ar);
        B = new UniformExpression(Ar);
        NumComponentsA = Ar.Read<int>();
    }
}

public class FMaterialUniformExpressionConstant : IUStruct
{
    public FLinearColor Value { get; private set; }
    public byte ValueType { get; private set; }

    public FMaterialUniformExpressionConstant(FAssetArchive Ar)
    {
        Value = Ar.Read<FLinearColor>();
        ValueType = Ar.Read<byte>();
    }
}
