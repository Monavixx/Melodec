using melodec_tool.Models;

namespace melodec_tool;

public static class Synchronizer
{
    public static async Task SyncAsync(Config config)
    {
        var tracks = Directory.GetFiles(config.MusicPath, "*.mp3")
            .Select(f => Path.GetFileNameWithoutExtension(f));
        var tracksToDownload = config.Tracks
            .Where(track => !tracks.Any(t => track.EvaluateFilename() == t))
            .ToArray();
        var playlists = Directory.GetDirectories(config.MusicPath);
        var playlistsToDownload = config.Playlists
            .Where(playlist => !playlists.Any(p => p == playlist.EvaluateDirectoryName()))
            .ToArray();

        var downloader = new Downloader(config);

        await Task.WhenAll(downloader.DownloadAsync(tracksToDownload),
            downloader.DownloadAsync(playlistsToDownload));
    }
}
