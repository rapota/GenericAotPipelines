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
    public interface IRequestHandler<in TRequest>
    {
        ValueTask HandleAsync(TRequest request, CancellationToken ct = default);
    }

    public interface IRequestHandler<in TRequest, TResponse>
    {
        ValueTask<TResponse> HandleAsync(TRequest request, CancellationToken ct = default);
    }

    
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class UsePipelineAttribute<TPipelineType> : Attribute;


    public delegate ValueTask NextVoidMiddlewareDelegate<in TRequest>(TRequest request, CancellationToken ct);

    public delegate ValueTask<TResponse> NextMiddlewareDelegate<in TRequest, TResponse>(TRequest request, CancellationToken ct);

    public interface IMiddleware<TRequest>
    {
        ValueTask InvokeAsync(TRequest request, NextVoidMiddlewareDelegate<TRequest> next, CancellationToken ct);
    }

    public interface IMiddleware<TRequest, TResponse>
    {
        ValueTask<TResponse> InvokeAsync(TRequest request, NextMiddlewareDelegate<TRequest, TResponse> next, CancellationToken ct);
    }


    public class Pipeline<TRequest>
    {
        public Pipeline(params IMiddleware<TRequest>[] middlewares)
        {
        }

        public IRequestHandler<TRequest> DecorateHandler(IRequestHandler<TRequest> requestHandler)
        {
            throw new NotImplementedException();
        }
    }

    public class Pipeline<TRequest, TResponse>
    {
        public Pipeline(params IMiddleware<TRequest, TResponse>[] middlewares)
        {
        }

        public IRequestHandler<TRequest, TResponse> DecorateHandler(IRequestHandler<TRequest, TResponse> requestHandler)
        {
            throw new NotImplementedException();
        }
    }


    public class DecoratedRequestHandler<TRequest> : IRequestHandler<TRequest>
    {
        private readonly IRequestHandler<TRequest> _requestHandler;

        public DecoratedRequestHandler(Pipeline<TRequest> pipeline, IRequestHandler<TRequest> requestHandler)
        {
            _requestHandler = pipeline.DecorateHandler(requestHandler);
        }

        public ValueTask HandleAsync(TRequest request, CancellationToken ct = default) =>
            _requestHandler.HandleAsync(request, ct);
    }

    public class DecoratedRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    {
        private readonly IRequestHandler<TRequest, TResponse> _requestHandler;

        public DecoratedRequestHandler(Pipeline<TRequest, TResponse> pipeline, IRequestHandler<TRequest, TResponse> requestHandler)
        {
            _requestHandler = pipeline.DecorateHandler(requestHandler);
        }

        public ValueTask<TResponse> HandleAsync(TRequest request, CancellationToken ct = default) =>
            _requestHandler.HandleAsync(request, ct);
    }
}
""";
}