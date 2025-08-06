namespace GenericAotPipelines;

[AttributeUsage(AttributeTargets.Class)]
public sealed class UsePipelineAttribute<TPipelineType> : Attribute;