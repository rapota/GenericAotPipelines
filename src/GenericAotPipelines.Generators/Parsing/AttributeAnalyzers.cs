using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace GenericAotPipelines.Generators.Parsing;

internal static class AttributeAnalyzers
{
    private const string AttributeNamespace = "GenericAotPipelines";
    private const string AttributeName = "UsePipelineAttribute";

    public static AttributeMetadata? GetAttributeMetadata(INamedTypeSymbol handlerSymbol)
    {
        ImmutableArray<AttributeData> attributes = handlerSymbol.GetAttributes();
        AttributeData? usePipelineAttribute = attributes.FirstOrDefault(IsUsePipelineAttribute);
        if (usePipelineAttribute == null)
        {
            return null;
        }

        ITypeSymbol? piplineTypeSymbol = usePipelineAttribute.AttributeClass?.TypeArguments[0];
        if (piplineTypeSymbol == null)
        {
            return null;
        }

        TypeMetadata genericPipelineType = Mappings.ToTypeMetadata(piplineTypeSymbol);

        return new AttributeMetadata(genericPipelineType);
    }

    private static bool IsUsePipelineAttribute(AttributeData attributeData)
    {
        INamedTypeSymbol? symbol = attributeData.AttributeClass;
        if (symbol == null)
        {
            return false;
        }

        return symbol.ContainingNamespace.Name == AttributeNamespace
            && symbol.Name == AttributeName;
    }

    private static INamedTypeSymbol GetAttributeArgument(AttributeData attribute)
    {
        TypedConstant first = attribute.ConstructorArguments.First();
        return (INamedTypeSymbol)first.Value!;
    }
}
