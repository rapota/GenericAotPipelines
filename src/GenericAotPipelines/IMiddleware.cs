namespace GenericAotPipelines;

public delegate ValueTask<TResponse> NextMiddlewareDelegate<in TRequest, TResponse>(TRequest request, CancellationToken ct);

public interface IMiddleware<TRequest, TResponse>
{
    ValueTask<TResponse> InvokeAsync(TRequest request, NextMiddlewareDelegate<TRequest, TResponse> next, CancellationToken ct);
}