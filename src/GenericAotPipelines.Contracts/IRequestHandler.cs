namespace GenericAotPipelines;

public interface IRequestHandler<in TRequest>
{
    ValueTask HandleAsync(TRequest request, CancellationToken ct = default);
}

public interface IRequestHandler<in TRequest, TResponse>
{
    ValueTask<TResponse> HandleAsync(TRequest request, CancellationToken ct = default);
}
