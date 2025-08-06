using GenericAotPipelines;

namespace Tests.IntegrationTests;

public record Todo(int Id, string? Title, DateOnly? DueBy = null, bool IsComplete = false);

public record GetByIdRequest(Guid Id);

public interface IGetByIdRequestHandler : IRequestHandler<GetByIdRequest, Todo>;

[UsePipeline<DefaultPipeline<GetByIdRequest, Todo>>]
internal sealed class GetByIdRequestHandler : IGetByIdRequestHandler
{
    public ValueTask<Todo> HandleAsync(GetByIdRequest request, CancellationToken ct = default)
    {
        Todo result = new(1, "Walk the dog");
        return ValueTask.FromResult(result);
    }
}
