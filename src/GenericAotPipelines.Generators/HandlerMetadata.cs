namespace GenericAotPipelines.Generators;

internal record struct HandlerMetadata(TypeMetadata HandlerType, InterfaceMetadata InterfaceMetadata, AttributeMetadata AttributeMetadata);

internal record struct AttributeMetadata(TypeMetadata PipelineType);

internal record struct TypeMetadata(string TypeName, string Namespace, string FullName);

internal record struct InterfaceMetadata(TypeMetadata InterfaceType, RequestResponseMetadata RequestResponseTypes);

internal record struct RequestResponseMetadata(TypeMetadata RequestType, TypeMetadata ResponseType);
