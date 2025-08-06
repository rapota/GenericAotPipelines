using GenericAotPipelines;

namespace Tests.IntegrationTests;

internal sealed class DefaultEventPipeline<TRequest> : Pipeline<TRequest>
{
    public DefaultEventPipeline(EmptyEvetMiddleware<TRequest> emptyEvetMiddleware)
        : base(emptyEvetMiddleware)
    {
    }
}

internal sealed class EmptyEvetMiddleware<TRequest> : IMiddleware<TRequest>
{
    public ValueTask InvokeAsync(TRequest request, NextVoidMiddlewareDelegate<TRequest> next, CancellationToken ct)
    {
        return next(request, ct);
    }
}