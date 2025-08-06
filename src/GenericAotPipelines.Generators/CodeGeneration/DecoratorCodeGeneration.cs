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

        source.WriteLine($"namespace {handlerMetadata.HandlerType.Namespace}");
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
        source.WriteLine($"partial class {handlerMetadata.HandlerType.TypeName}");
        source.WriteLine("{");
        source.Indent++;

        string decoratorName = GenerateDecorator(source, handlerMetadata);
        source.WriteLine();
        GenerateRegistrationMethod(source, handlerMetadata, decoratorName);

        source.Indent--;
        source.WriteLine("}");
    }

    private static void GenerateRegistrationMethod(IndentedTextWriter source, HandlerMetadata handlerMetadata, string decoratorName)
    {
        source.WriteLine("internal static void RegisterDecoratedHandler(IServiceCollection services)");
        source.WriteLine("{");
        source.Indent++;

        source.WriteLine("services");
        source.Indent++;
        source.WriteLine($".AddTransient<{handlerMetadata.HandlerType.TypeName}>()");
        source.WriteLine($".AddTransient<{Mappings.Format(handlerMetadata.InterfaceMetadata.InterfaceType)}, {decoratorName}>();");
        source.Indent--;
        
        source.Indent--;
        source.WriteLine("}");
    }

    private static string GenerateDecorator(IndentedTextWriter source, HandlerMetadata handlerMetadata)
    {
        string decoratorName = handlerMetadata.HandlerType.TypeName + "Decorator";

        source.WriteLine($"private sealed class {decoratorName}");

        source.Indent++;
        source.WriteLine(
            ": global::GenericAotPipelines.PipelineDecorator<{0}>",
            Mappings.Format(handlerMetadata.InterfaceMetadata.RequestResponseTypes));
        source.WriteLine(
            ", {0}",
            Mappings.Format(handlerMetadata.InterfaceMetadata.InterfaceType));
        source.Indent--;

        source.WriteLine("{");
        source.Indent++;

        GenerateConstuctor(source, handlerMetadata, decoratorName);

        source.Indent--;
        source.WriteLine("}");

        return decoratorName;
    }

    private static void GenerateConstuctor(IndentedTextWriter source, HandlerMetadata handlerMetadata, string decoratorName)
    {
        source.WriteLine($"public {decoratorName}(");
        source.Indent++;

        source.WriteLine("{0} pipeline,", Mappings.Format(handlerMetadata.AttributeMetadata.PipelineType));

        source.WriteLine($"{handlerMetadata.HandlerType.TypeName} handler)");
        source.WriteLine(": base(pipeline, handler)");

        source.Indent--;
        source.WriteLine("{");
        source.WriteLine("}");        
    }
}