namespace GenericAotPipelines;

public class Pipeline<TRequest>
{
    private readonly List<IMiddleware<TRequest>> _middlewares;

    public Pipeline(params IMiddleware<TRequest>[] middlewares)
    {
        _middlewares = middlewares.ToList();
        _middlewares.Reverse();
    }

    public IRequestHandler<TRequest> DecorateHandler(IRequestHandler<TRequest> requestHandler)
    {
        if (_middlewares.Count == 0)
        {
            return requestHandler;
        }

        IChainLink target = new TargetLink(requestHandler);
        foreach (IMiddleware<TRequest> middleware in _middlewares)
        {
            target = new MiddlewareLink(middleware, target);
        }

        return target;
    }

    private interface IChainLink : IRequestHandler<TRequest>;

    private sealed class TargetLink(IRequestHandler<TRequest> requestHandler) : IChainLink
    {
        public ValueTask HandleAsync(TRequest request, CancellationToken token) =>
            requestHandler.HandleAsync(request, token);

        public override string? ToString() => requestHandler.ToString();
    }

    private sealed class MiddlewareLink(IMiddleware<TRequest> middleware, IChainLink nextLink) : IChainLink
    {
        public ValueTask HandleAsync(TRequest request, CancellationToken token) =>
            middleware.InvokeAsync(request, nextLink.HandleAsync, token);

        public override string? ToString() => middleware.ToString();
    }
}

public class Pipeline<TRequest, TResponse>
{
    private readonly List<IMiddleware<TRequest, TResponse>> _middlewares;

    public Pipeline(params IMiddleware<TRequest, TResponse>[] middlewares)
    {
        _middlewares = middlewares.ToList();
        _middlewares.Reverse();
    }

    public IRequestHandler<TRequest, TResponse> DecorateHandler(IRequestHandler<TRequest, TResponse> requestHandler)
    {
        if (_middlewares.Count == 0)
        {
            return requestHandler;
        }

        IChainLink target = new TargetLink(requestHandler);
        foreach (IMiddleware<TRequest, TResponse> middleware in _middlewares)
        {
            target = new MiddlewareLink(middleware, target);
        }

        return target;
    }

    private interface IChainLink : IRequestHandler<TRequest, TResponse>;

    private sealed class TargetLink(IRequestHandler<TRequest, TResponse> requestHandler) : IChainLink
    {
        public ValueTask<TResponse> HandleAsync(TRequest request, CancellationToken token) =>
            requestHandler.HandleAsync(request, token);

        public override string? ToString() => requestHandler.ToString();
    }

    private sealed class MiddlewareLink(IMiddleware<TRequest, TResponse> middleware, IChainLink nextLink) : IChainLink
    {
        public ValueTask<TResponse> HandleAsync(TRequest request, CancellationToken token) =>
            middleware.InvokeAsync(request, nextLink.HandleAsync, token);

        public override string? ToString() => middleware.ToString();
    }
}