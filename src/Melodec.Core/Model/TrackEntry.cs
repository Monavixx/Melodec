namespace Melodec.Core.Model;

public sealed record TrackEntry : Entry
{
    public string? Artist { get; }
    public string? Title { get; }
    public Uri? PinnedUrl { get; }
    public string? OverrideFilename { get; }
    public string? OverrideMusicPath { get; }

    public TrackEntry(
        string? artist,
        string? title,
        Uri? pinnedUrl,
        string? overrideFilename = null,
        string? overrideMusicPath = null)
    {
        if ((artist is null && title is not null) || (artist is not null && title is null))
            throw new ArgumentException("Artist and Title must be both either set or null");
        if (artist is null && pinnedUrl is null)
            throw new ArgumentException("At least one of the following should be set: [PinnedUrl] or [Artist,Title]");
        Artist = artist;
        Title = title;
        PinnedUrl = pinnedUrl;
        OverrideFilename = overrideFilename;
        OverrideMusicPath = overrideMusicPath;
    }

    public override string Key => Artist is not null && Title is not null ?
        $"track:{EntryKeys.Normalize(Artist)}|{EntryKeys.Normalize(Title)}"
        : $"track:url:{EntryKeys.HashUrl(PinnedUrl!)}";

    public override string Display => Artist is not null && Title is not null
        ? $"{Artist} - {Title}"
        : PinnedUrl!.ToString();
}
