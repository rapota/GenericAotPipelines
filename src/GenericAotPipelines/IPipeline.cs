namespace GenericAotPipelines;

public interface IPipeline<TRequest, TResponse>
{
    Task<TResponse> InvokeAsync(IHandler<TRequest, TResponse> handler, TRequest request, CancellationToken token);
}