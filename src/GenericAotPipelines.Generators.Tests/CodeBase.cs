namespace GenericAotPipelines.Generators.Tests;

internal static class CodeBase
{
    public const string CommonCode = """
#nullable enable

using System;
using System.Threading;
using System.Threading.Tasks;

namespace GenericAotPipelines
{
    public interface IHandler<in TRequest, TResponse>
    {
        ValueTask<TResponse> HandleAsync(TRequest request, CancellationToken ct = default);
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class UsePipelineAttribute<TPipelineType> : Attribute;

    public delegate ValueTask<TResponse> NextMiddlewareDelegate<in TRequest, TResponse>(TRequest request, CancellationToken ct);

    public interface IMiddleware<TRequest, TResponse>
    {
        ValueTask<TResponse> InvokeAsync(TRequest request, NextMiddlewareDelegate<TRequest, TResponse> next, CancellationToken ct);
    }

    public interface IPipeline<TRequest, TResponse>
    {
        ValueTask<TResponse> InvokeAsync(IHandler<TRequest, TResponse> handler, TRequest request, CancellationToken token);
    }

    public class Pipeline<TRequest, TResponse> : IPipeline<TRequest, TResponse>
    {
        public Pipeline(params IMiddleware<TRequest, TResponse>[] middlewares)
        {
        }

        public async ValueTask<TResponse> InvokeAsync(IHandler<TRequest, TResponse> handler, TRequest request, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }

    public class PipelineDecorator<TRequest, TResponse>(
        IPipeline<TRequest, TResponse> pipeline,
        IHandler<TRequest, TResponse> handler)
        : IHandler<TRequest, TResponse>
    {
        public ValueTask<TResponse> HandleAsync(TRequest request, CancellationToken ct = default) =>
            pipeline.InvokeAsync(handler, request, ct);
    }
}
""";
}