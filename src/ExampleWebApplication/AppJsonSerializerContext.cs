using System.Text.Json.Serialization;
using ExamplePipelines.Handlers;

[JsonSerializable(typeof(Todo[]))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}