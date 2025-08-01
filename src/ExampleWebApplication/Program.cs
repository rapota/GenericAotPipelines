using ExampleWebApplication;
using ExamplePipelines;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

builder.Services.AddHandlers();

var app = builder.Build();

var todosApi = app.MapGroup("/todos");
todosApi.MapGet("/", RestEndpoints.GetAllAsync);
//todosApi.MapGet("/{id}", RestEndpoints.GetByIdAsync);

app.Run();