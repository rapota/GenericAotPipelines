using Microsoft.CodeAnalysis;

namespace GenericAotPipelines.Generators.Parsing;

internal static class HandlerAnalyzers
{
    public static HandlerMetadata? GetMetadata(INamedTypeSymbol handlerSymbol)
    {
        InterfaceMetadata? interfaceMetadata = InterfaceAnalyzers.TryGetInterfaceMetadata(handlerSymbol);
        if (interfaceMetadata == null)
        {
            return null;
        }

        AttributeMetadata? attributeMetadata = AttributeAnalyzers.GetAttributeMetadata(handlerSymbol);
        if (attributeMetadata == null)
        {
            return null;
        }

        return new HandlerMetadata(
            Mappings.ToNamedTypeMetadata(handlerSymbol),
            (InterfaceMetadata)interfaceMetadata,
            (AttributeMetadata)attributeMetadata
        );
    }
}