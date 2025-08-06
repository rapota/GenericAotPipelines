namespace GenericAotPipelines.Generators.IntegrationTests;

public record Todo(int Id, string? Title, DateOnly? DueBy = null, bool IsComplete = false);

public record GetByIdRequest(Guid Id);

public interface IGetByIdRequestHandler : IHandler<GetByIdRequest, Todo>;

[UsePipeline<DefaultPipeline<GetByIdRequest, Todo>>]
internal sealed partial class GetByIdRequestHandler : IGetByIdRequestHandler
{
    public ValueTask<Todo> HandleAsync(GetByIdRequest request, CancellationToken ct = default)
    {
        Todo result = new(1, "Walk the dog");
        return ValueTask.FromResult(result);
    }

    public static Type GetDecoratorType()
    {
        return typeof(GetByIdRequestHandlerDecorator);
    }
}
