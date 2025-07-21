namespace GenericAotPipelines;

public class PipelineDecorator<TRequest, TResponse>(
    IPipeline<TRequest, TResponse> pipeline,
    IHandler<TRequest, TResponse> handler)
    : IHandler<TRequest, TResponse>
{
    public Task<TResponse> HandleAsync(TRequest request, CancellationToken ct = default) =>
        pipeline.InvokeAsync(handler, request, ct);
}