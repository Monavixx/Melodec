using System.Text.Json.Serialization;

namespace melodec_tool.Models.SerializerContexts;

[JsonSerializable(typeof(Config))]
internal partial class ConfigSerializerContext : JsonSerializerContext
{
}
