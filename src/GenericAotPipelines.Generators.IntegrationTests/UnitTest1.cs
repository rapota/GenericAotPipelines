using Microsoft.Extensions.DependencyInjection;
using GenericAotPipelinesExtensions;

namespace GenericAotPipelines.Generators.IntegrationTests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        Type decoratorType = GetByIdRequestHandler.GetDecoratorType();
        Assert.Equal("GetByIdRequestHandlerDecorator", decoratorType.Name);
    }

    [Fact]
    public void TestDi()
    {
        IServiceCollection services = new ServiceCollection();

        services.AddTransient(typeof(DefaultPipeline<,>));
        services.RegisterDecoratedHandlers();

        using ServiceProvider provider = services.BuildServiceProvider();
        IGetByIdRequestHandler handler = provider.GetRequiredService<IGetByIdRequestHandler>();

        Assert.Equal("GetByIdRequestHandlerDecorator", handler.GetType().Name);
    }
}