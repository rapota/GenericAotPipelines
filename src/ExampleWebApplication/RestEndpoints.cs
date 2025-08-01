using ExamplePipelines.Handlers;

namespace ExampleWebApplication;

public static class RestEndpoints
{
    public static ValueTask<Todo[]> GetAllAsync(IGetAllQueryHandler handler, CancellationToken ct) =>
        handler.HandleAsync(true, ct);

    //public static async Task<IResult> GetByIdAsync(int id, IGetByIdRequestHandler handler, CancellationToken ct)
    //{
    //    Todo? todo = await handler.HandleAsync(id, ct);
    //    return todo != null
    //        ? Results.Ok(todo)
    //        : Results.NotFound();
    //}
}