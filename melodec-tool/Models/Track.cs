using System.Text.Json.Serialization;

namespace melodec_tool.Models;

public class Track
{
    [JsonPropertyName("url")]
    public string Url { get; set; } = null!;
    [JsonPropertyName("filename")]
    public string? Filename { get; set; }
    [JsonPropertyName("author")]
    public string? Author { get; set; }
    [JsonPropertyName("title")]
    public string? Title { get; set; }
    [JsonPropertyName("album")]
    public string? Album { get; set; }

    [JsonIgnore]
    private string? _cachedName = null;
    public string EvaluateFilename()
    {
        if (_cachedName is not null) return _cachedName;
        if (!string.IsNullOrWhiteSpace(Filename))
            return Filename;
        string author = "unknown", title = "untitled";
        if (!string.IsNullOrWhiteSpace(Author))
            author = Author;
        if (!string.IsNullOrWhiteSpace(Title))
            title = Title;
        return _cachedName = $"{author} - {title}";
    }
}
