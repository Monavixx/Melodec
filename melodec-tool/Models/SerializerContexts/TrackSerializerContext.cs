using System.Text.Json.Serialization;

namespace melodec_tool.Models.SerializerContexts;

[JsonSerializable(typeof(Track))]
[JsonSerializable(typeof(Track[]))]
internal partial class TrackSerializerContext : JsonSerializerContext
{
}
