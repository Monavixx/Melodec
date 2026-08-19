using System.Text.Json.Serialization;

namespace melodec_tool.Models;

public class Config
{
    [JsonPropertyName("music_path")]
    public string MusicPath { get; set; } = null!;
    [JsonPropertyName("tracks")]
    public Track[] Tracks { get; set; } = [];
    [JsonPropertyName("playlists")]
    public Playlist[] Playlists { get; set; } = [];
}
