namespace CUE4Parse.UE4.Assets.Exports.Actor;

public class UComponent : UObject;

public class UDistributionFloat : UComponent;
public class UDistributionVector : UComponent;

public class UDistributionFloatConstant : UDistributionFloat;
public class UDistributionFloatConstantCurve : UDistributionFloat;
public class UDistributionFloatParameterBase : UDistributionFloatConstant;
public class UDistributionFloatParticleParameter : UDistributionFloatParameterBase;
public class UDistributionFloatSoundParameter : UDistributionFloatParameterBase;
public class UDistributionFloatUniform : UDistributionFloat;
public class UDistributionFloatUniformCurve : UDistributionFloat;
public class UDistributionFloatUniformRange : UDistributionFloat;
public class UDistributionVectorConstant : UDistributionVector;
public class UDistributionVectorConstantCurve : UDistributionVector;
public class UDistributionVectorParameterBase : UDistributionVector;
public class UDistributionVectorParticleParameter : UDistributionVector;
public class UDistributionVectorUniform : UDistributionVector;
public class UDistributionVectorUniformCurve : UDistributionVector;
public class UDistributionVectorUniformRange : UDistributionVector;