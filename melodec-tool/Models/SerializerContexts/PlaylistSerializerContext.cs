using System.Text.Json.Serialization;

namespace melodec_tool.Models.SerializerContexts;

[JsonSerializable(typeof(Playlist))]
[JsonSerializable(typeof(Playlist[]))]
internal partial class PlaylistSerializerContext : JsonSerializerContext
{
}
