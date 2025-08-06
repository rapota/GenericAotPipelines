using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Diagnostics;
using System.Text;
using GenericAotPipelines.Generators.CodeGeneration;
using GenericAotPipelines.Generators.Parsing;

namespace GenericAotPipelines.Generators;

[Generator(LanguageNames.CSharp)]
public class DecoratorGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
#if DEBUG
        //if (!Debugger.IsAttached)
        //{
        //    Debugger.Launch();
        //}
#endif

        IncrementalValuesProvider<HandlerMetadata?> handlers = context.SyntaxProvider.ForAttributeWithMetadataName(
            fullyQualifiedMetadataName: "GenericAotPipelines.UsePipelineAttribute`1",
            predicate: (syntaxNode, _) => syntaxNode is ClassDeclarationSyntax,
            transform: (syntaxContext, _) => HandlerAnalyzers.GetMetadata((INamedTypeSymbol)syntaxContext.TargetSymbol)
        )
        .Where(x => x != null);

        context.RegisterSourceOutput(handlers, AddGeneratedCode);

        IncrementalValueProvider<ImmutableArray<HandlerMetadata?>> collectedHandlers = handlers.Collect();
        context.RegisterSourceOutput(collectedHandlers, AddRegistrationExtensions);
    }

    private static void AddGeneratedCode(SourceProductionContext context, HandlerMetadata? handlerMetadata)
    {
        if (handlerMetadata == null)
        {
            return;
        }

        HandlerMetadata metadata = (HandlerMetadata)handlerMetadata;

        string code = DecoratorCodeGeneration.GenerateDecorator(metadata);
        SourceText sourceText = SourceText.From(code, Encoding.UTF8);

        string fileName = $"{metadata.HandlerType.TypeName}.g.cs";
        context.AddSource(fileName, sourceText);
    }

    private void AddRegistrationExtensions(SourceProductionContext context, ImmutableArray<HandlerMetadata?> handlers)
    {
        string code = RegistrationCodeGeneration.GenerateDecorator(handlers);
        SourceText sourceText = SourceText.From(code, Encoding.UTF8);

        string fileName = "GenericAotPipelinesExtensions.g.cs";
        context.AddSource(fileName, sourceText);
    }
}