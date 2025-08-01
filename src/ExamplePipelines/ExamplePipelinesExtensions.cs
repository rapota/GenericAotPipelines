using ExamplePipelines.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace ExamplePipelines;

public static class ExamplePipelinesExtensions
{
    public static IServiceCollection AddHandlers(this IServiceCollection services) =>
        services
            .AddTransient(typeof(DefaultPipeline<,>))
            .AddTransient<IGetAllQueryHandler, GetAllQueryHandler>()
            .AddTransient<IGetByIdRequestHandler, GetByIdRequestHandler>();
}