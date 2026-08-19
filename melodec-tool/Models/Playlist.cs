using System.Text.Json.Serialization;

namespace melodec_tool.Models;

public class Playlist
{
    [JsonPropertyName("url")]
    public string Url { get; set; } = null!;
    [JsonPropertyName("directory_name")]
    public string? DirectoryName { get; set; }
    [JsonPropertyName("author")]
    public string? Author { get; set; }
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonIgnore]
    private string? _cachedName = null;
    public string EvaluateDirectoryName()
    {
        if (_cachedName is not null) return _cachedName;
        if (!string.IsNullOrWhiteSpace(DirectoryName))
            return DirectoryName;
        string author = "unknown", title = "untitled";
        if (!string.IsNullOrWhiteSpace(Author))
            author = Author;
        if (!string.IsNullOrWhiteSpace(Title))
            title = Title;
        return _cachedName = $"{author} - {title}";
    }
}
