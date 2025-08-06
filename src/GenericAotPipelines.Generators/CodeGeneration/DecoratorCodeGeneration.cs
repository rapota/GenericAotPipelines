using System.CodeDom.Compiler;

namespace GenericAotPipelines.Generators.CodeGeneration;

internal static class DecoratorCodeGeneration
{
    public static string GenerateDecorator(HandlerMetadata handlerMetadata)
    {
        return FileGenerator.GenerateFile(code => GenerateFile(code, handlerMetadata));
    }

    private static void GenerateFile(IndentedTextWriter source, HandlerMetadata handlerMetadata)
    {
        //source.WriteLine("using System;");
        //source.WriteLine("using System.Threading;");
        //source.WriteLine("using System.Threading.Tasks;");
        source.WriteLine("using Microsoft.Extensions.DependencyInjection;");
        source.WriteLine();

        source.WriteLine("namespace GenericAotPipelines.Generated.Decorators");
        source.WriteLine("{");
        source.Indent++;

        GenerateClass(source, handlerMetadata);

        source.Indent--;
        source.WriteLine("}");
    }

    private static void GenerateClass(IndentedTextWriter source, HandlerMetadata handlerMetadata)
    {
        var className = typeof(DecoratorGenerator).FullName;
        var assemblyVersion = typeof(DecoratorGenerator).Assembly.GetName().Version.ToString();

        source.WriteLine("[global::System.CodeDom.Compiler.GeneratedCode(\"{0}\", \"{1}\")]", className, assemblyVersion);
        source.WriteLine("[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]");
        GenerateDecorator(source, handlerMetadata);
    }

    private static void GenerateDecorator(IndentedTextWriter source, HandlerMetadata handler)
    {
        string decoratorName = handler.Type.FullName.Replace('.', '_') + "_Decorator";

        source.WriteLine($"internal sealed class {decoratorName}");

        source.Indent++;
        source.WriteLine(
            ": DecoratedRequestHandler<{0}>",
            Mappings.Format(handler.Interface.RequestResponseTypes));
        source.WriteLine($", {handler.Interface.Type}");
        source.Indent--;

        source.WriteLine("{");
        source.Indent++;

        GenerateConstructor(source, handler, decoratorName);
        source.WriteLine();
        GenerateRegistrationMethod(source, handler, decoratorName);

        source.Indent--;
        source.WriteLine("}");
    }

    private static void GenerateConstructor(IndentedTextWriter source, HandlerMetadata handler, string decoratorName)
    {
        source.WriteLine($"public {decoratorName}(");
        source.Indent++;

        source.WriteLine($"{handler.AttributeMetadata.PipelineType} pipeline,");

        source.WriteLine($"{handler.Type.FullName} handler)");
        source.WriteLine(": base(pipeline, handler)");

        source.Indent--;
        source.WriteLine("{");
        source.WriteLine("}");
    }

    private static void GenerateRegistrationMethod(IndentedTextWriter source, HandlerMetadata handler, string decoratorName)
    {
        source.WriteLine("public static void RegisterDecoratedHandler(IServiceCollection services)");
        source.WriteLine("{");
        source.Indent++;

        source.WriteLine("services");
        source.Indent++;
        source.WriteLine($".AddTransient<{handler.Type.FullName}>()");
        source.WriteLine($".AddTransient<{handler.Interface.Type}, {decoratorName}>();");
        source.Indent--;

        source.Indent--;
        source.WriteLine("}");
    }
}