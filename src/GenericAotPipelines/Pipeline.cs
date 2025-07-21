namespace GenericAotPipelines;

public class Pipeline<TRequest, TResponse> : IPipeline<TRequest, TResponse>
{
    private readonly List<IMiddleware<TRequest, TResponse>> _middlewares;

    public Pipeline(params IMiddleware<TRequest, TResponse>[] middlewares)
    {
        _middlewares = middlewares.ToList();
        _middlewares.Reverse();
    }

    public Task<TResponse> InvokeAsync(IHandler<TRequest, TResponse> handler, TRequest request, CancellationToken token)
    {
        IChainLink target = new TargetLink(handler);
        foreach (IMiddleware<TRequest, TResponse> middleware in _middlewares)
        {
            target = new MiddlewareLink(middleware, target);
        }

        return target.InvokeAsync(request, token);
    }

    private interface IChainLink
    {
        Task<TResponse> InvokeAsync(TRequest request, CancellationToken token);
    }

    private sealed class TargetLink(IHandler<TRequest, TResponse> handler)
        : IChainLink
    {
        public Task<TResponse> InvokeAsync(TRequest request, CancellationToken token) =>
            handler.HandleAsync(request, token);

        public override string? ToString() => handler.ToString();
    }

    private sealed class MiddlewareLink(IMiddleware<TRequest, TResponse> middleware, IChainLink nextLink)
        : IChainLink
    {
        public Task<TResponse> InvokeAsync(TRequest request, CancellationToken token) =>
            middleware.InvokeAsync(request, nextLink.InvokeAsync, token);

        public override string? ToString() => middleware.ToString();
    }
}