using GenericAotPipelines;

namespace ExamplePipelines;

public sealed class DefaultPipeline<TRequest, TResponse> : Pipeline<TRequest, TResponse>
{
    public DefaultPipeline() : base()
    {
    }
}