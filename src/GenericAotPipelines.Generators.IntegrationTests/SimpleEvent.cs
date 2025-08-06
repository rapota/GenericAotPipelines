using GenericAotPipelines;

namespace Tests.IntegrationTests;

public record SimpleEvent;

public interface ISimpleEventHandler : IRequestHandler<SimpleEvent>;

[UsePipeline<DefaultEventPipeline<SimpleEvent>>]
internal sealed class SimpleEventHandler : ISimpleEventHandler
{
    public ValueTask HandleAsync(SimpleEvent request, CancellationToken ct = default)
    {
        return ValueTask.CompletedTask;
    }
}