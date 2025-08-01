namespace GenericAotPipelines;

public interface IPipeline<TRequest, TResponse>
{
    ValueTask<TResponse> InvokeAsync(IHandler<TRequest, TResponse> handler, TRequest request, CancellationToken token);
}