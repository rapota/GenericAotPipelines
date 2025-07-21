namespace GenericAotPipelines;

[AttributeUsage(AttributeTargets.Class)]
public class UsePipelineAttribute(Type pipelineType) : Attribute
{
    public Type PipelineType { get; } = pipelineType;
}