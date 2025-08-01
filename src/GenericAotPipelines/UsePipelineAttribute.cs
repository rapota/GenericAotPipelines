namespace GenericAotPipelines;

[AttributeUsage(AttributeTargets.Class)]
public class UsePipelineAttribute<TPipelineType> : Attribute;