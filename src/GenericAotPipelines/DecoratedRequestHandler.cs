namespace GenericAotPipelines;

public class DecoratedRequestHandler<TRequest> : IRequestHandler<TRequest>
{
    private readonly IRequestHandler<TRequest> _requestHandler;

    public DecoratedRequestHandler(Pipeline<TRequest> pipeline, IRequestHandler<TRequest> requestHandler)
    {
        _requestHandler = pipeline.DecorateHandler(requestHandler);
    }

    public ValueTask HandleAsync(TRequest request, CancellationToken ct = default) =>
        _requestHandler.HandleAsync(request, ct);
}

public class DecoratedRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
{
    private readonly IRequestHandler<TRequest, TResponse> _requestHandler;

    public DecoratedRequestHandler(Pipeline<TRequest, TResponse> pipeline, IRequestHandler<TRequest, TResponse> requestHandler)
    {
        _requestHandler = pipeline.DecorateHandler(requestHandler);
    }

    public ValueTask<TResponse> HandleAsync(TRequest request, CancellationToken ct = default) =>
        _requestHandler.HandleAsync(request, ct);
}