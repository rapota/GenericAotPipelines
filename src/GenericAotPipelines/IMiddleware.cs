namespace GenericAotPipelines;

public delegate ValueTask NextVoidMiddlewareDelegate<in TRequest>(TRequest request, CancellationToken ct);

public delegate ValueTask<TResponse> NextMiddlewareDelegate<in TRequest, TResponse>(TRequest request, CancellationToken ct);

public interface IMiddleware<TRequest>
{
    ValueTask InvokeAsync(TRequest request, NextVoidMiddlewareDelegate<TRequest> next, CancellationToken ct);
}

public interface IMiddleware<TRequest, TResponse>
{
    ValueTask<TResponse> InvokeAsync(TRequest request, NextMiddlewareDelegate<TRequest, TResponse> next, CancellationToken ct);
}