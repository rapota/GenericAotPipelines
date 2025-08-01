using GenericAotPipelines;

namespace ExamplePipelines.Handlers;

public record GetByIdRequest(Guid Id);

public interface IGetByIdRequestHandler : IHandler<GetByIdRequest, string>;

[UsePipeline<DefaultPipeline<GetByIdRequest, string>>]
internal sealed partial class GetByIdRequestHandler : IGetByIdRequestHandler
{
    public ValueTask<string> HandleAsync(GetByIdRequest request, CancellationToken ct = default)
    {
        var result = request.ToString();
        return ValueTask.FromResult(result);
    }
}
