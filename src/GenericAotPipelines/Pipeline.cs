namespace GenericAotPipelines;

public class Pipeline<TRequest, TResponse> : IPipeline<TRequest, TResponse>
{
    private readonly List<IMiddleware<TRequest, TResponse>> _middlewares;

    public Pipeline(params IMiddleware<TRequest, TResponse>[] middlewares)
    {
        _middlewares = middlewares.ToList();
        _middlewares.Reverse();
    }

    public async ValueTask<TResponse> InvokeAsync(IHandler<TRequest, TResponse> handler, TRequest request, CancellationToken token)
    {
        IChainLink root = BuildChain(handler);
        return await root.InvokeAsync(request, token);
    }

    private IChainLink BuildChain(IHandler<TRequest, TResponse> handler)
    {
        IChainLink target = new TargetLink(handler);
        foreach (IMiddleware<TRequest, TResponse> middleware in _middlewares)
        {
            target = new MiddlewareLink(middleware, target);
        }

        return target;
    }

    private interface IChainLink
    {
        ValueTask<TResponse> InvokeAsync(TRequest request, CancellationToken token);
    }

    private sealed class TargetLink(IHandler<TRequest, TResponse> handler)
        : IChainLink
    {
        public ValueTask<TResponse> InvokeAsync(TRequest request, CancellationToken token) =>
            handler.HandleAsync(request, token);

        public override string? ToString() => handler.ToString();
    }

    private sealed class MiddlewareLink(IMiddleware<TRequest, TResponse> middleware, IChainLink nextLink)
        : IChainLink
    {
        public ValueTask<TResponse> InvokeAsync(TRequest request, CancellationToken token) =>
            middleware.InvokeAsync(request, nextLink.InvokeAsync, token);

        public override string? ToString() => middleware.ToString();
    }
}