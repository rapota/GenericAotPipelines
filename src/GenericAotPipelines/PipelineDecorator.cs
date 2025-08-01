namespace GenericAotPipelines;

public class PipelineDecorator<TRequest, TResponse>(
    IPipeline<TRequest, TResponse> pipeline,
    IHandler<TRequest, TResponse> handler)
    : IHandler<TRequest, TResponse>
{
    public ValueTask<TResponse> HandleAsync(TRequest request, CancellationToken ct = default) =>
        pipeline.InvokeAsync(handler, request, ct);
}