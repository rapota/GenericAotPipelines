using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace GenericAotPipelines.Generators;

internal static class Mappings
{
    public static TypeMetadata ToNamedTypeMetadata(INamedTypeSymbol namedTypeSymbol) =>
        new TypeMetadata(namedTypeSymbol.Name, namedTypeSymbol.ContainingNamespace.ToString(), namedTypeSymbol.ToString());

    public static TypeMetadata ToTypeMetadata(ITypeSymbol typeSymbol) =>
        new TypeMetadata(typeSymbol.Name, typeSymbol.ContainingNamespace?.ToString() ?? "", typeSymbol.ToString());

    public static string Format(TypeMetadata typeMetadata)
    {
        return typeMetadata.FullName;
        //string code = typeMetadata.Namespace + "." + typeMetadata.TypeName;
        //return typeMetadata.IsGlobalNamespace
        //    ? "global::" + code
        //    : code;
    }

    public static string Format(RequestResponseMetadata requestResponseMetadata) =>
        Format(requestResponseMetadata.RequestType) + ", " + Format(requestResponseMetadata.ResponseType);
}
