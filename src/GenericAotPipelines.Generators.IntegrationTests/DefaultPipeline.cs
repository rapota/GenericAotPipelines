using GenericAotPipelines;

namespace GenericAotPipelines.Generators.IntegrationTests;

public sealed class DefaultPipeline<TRequest, TResponse> : Pipeline<TRequest, TResponse>
{
    public DefaultPipeline() : base()
    {
    }
}