using ExamplePipelines.Handlers;

namespace GenericAotPipelines.Generators.IntegrationTests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            Type decoratorType = GetByIdRequestHandler.GetDecoratorType();
            Assert.Equal("GetByIdRequestHandlerDecorator", decoratorType.Name);

        }
    }
}
