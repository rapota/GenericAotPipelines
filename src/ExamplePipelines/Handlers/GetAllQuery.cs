using GenericAotPipelines;
using System.CodeDom.Compiler;

namespace ExamplePipelines.Handlers;

public record Todo(int Id, string? Title, DateOnly? DueBy = null, bool IsComplete = false);

public interface IGetAllQueryHandler : IHandler<bool, Todo[]>;

[UsePipeline<DefaultPipeline<bool, Todo[]>>]
internal sealed partial class GetAllQueryHandler : IGetAllQueryHandler
{
    private static readonly Todo[] SampleTodos =
    [
        new(1, "Walk the dog"),
        new(2, "Do the dishes", DateOnly.FromDateTime(DateTime.Now)),
        new(3, "Do the laundry", DateOnly.FromDateTime(DateTime.Now.AddDays(1))),
        new(4, "Clean the bathroom"),
        new(5, "Clean the car", DateOnly.FromDateTime(DateTime.Now.AddDays(2)))
    ];

    public ValueTask<Todo[]> HandleAsync(bool request, CancellationToken ct = default)
    {
        return ValueTask.FromResult(SampleTodos);
    }
}
