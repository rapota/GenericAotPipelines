using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace GenericAotPipelines.Generators;

internal static class InterfaceAnalyzers
{
    private const string HandlerInterfaceNamespace = "GenericAotPipelines";
    private const string HandlerInterfaceName = "IHandler";

    public static InterfaceMetadata? TryGetInterfaceMetadata(INamedTypeSymbol handlerSymbol)
    {
        foreach (INamedTypeSymbol interfaceSymbol in handlerSymbol.Interfaces)
        {
            RequestResponseMetadata? requestResponseTypes = TryGetRequestResponseTypesRecursive(interfaceSymbol);
            if (requestResponseTypes != null)
            {
                return new InterfaceMetadata(
                    Mappings.ToNamedTypeMetadata(interfaceSymbol),
                    (RequestResponseMetadata)requestResponseTypes);
            }
        }

        return null;
    }

    private static bool IsIHandler(INamedTypeSymbol interfaceSymbol) =>
        interfaceSymbol.ContainingNamespace.Name == HandlerInterfaceNamespace
        && interfaceSymbol.Name == HandlerInterfaceName;

    private static RequestResponseMetadata? TryGetRequestResponseTypes(INamedTypeSymbol interfaceSymbol)
    {
        if (!IsIHandler(interfaceSymbol))
        {
            return null;
        }

        ImmutableArray<ITypeSymbol> arguments = interfaceSymbol.TypeArguments;
        if (arguments.Length != 2)
        {
            return null;
        }

        return new RequestResponseMetadata(
            Mappings.ToTypeMetadata(arguments[0]),
            Mappings.ToTypeMetadata(arguments[1]));
    }

    private static RequestResponseMetadata? TryGetRequestResponseTypesRecursive(INamedTypeSymbol interfaceSymbol)
    {
        foreach (INamedTypeSymbol symbol in interfaceSymbol.Interfaces)
        {
            RequestResponseMetadata? requestResponseTypes = TryGetRequestResponseTypesRecursive(symbol);
            if (requestResponseTypes != null)
            {
                return requestResponseTypes;
            }
        }

        return TryGetRequestResponseTypes(interfaceSymbol);
    }
}