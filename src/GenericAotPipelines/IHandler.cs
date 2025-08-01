namespace GenericAotPipelines;

public interface IHandler<in TRequest, TResponse>
{
    ValueTask<TResponse> HandleAsync(TRequest request, CancellationToken ct = default);
}