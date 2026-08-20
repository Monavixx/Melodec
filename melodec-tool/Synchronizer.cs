using melodec_tool.Models;

namespace melodec_tool;

public class Synchronizer(Config config)
{
    public async Task SyncAsync(bool dontAsk = false)
    {
        if (!config.KeepUndeclared)
        {
            RemoveUndeclaredAsync(dontAsk);
        }
        var tracks = Directory.GetFiles(config.MusicPath, "*.mp3");
        var tracksToDownload = config.Tracks
            .Where(track => !tracks.Any(t => track.EvaluateFilename() == Path.GetFileNameWithoutExtension(t)))
            .ToArray();
        var playlists = Directory.GetDirectories(config.MusicPath);
        var playlistsToDownload = config.Playlists
            .Where(playlist => !playlists.Any(p => p == playlist.EvaluateDirectoryName()))
            .ToArray();
        var downloader = new Downloader(config);

        await Task.WhenAll(downloader.DownloadAsync(tracksToDownload),
            downloader.DownloadAsync(playlistsToDownload));
    }

    public void RemoveUndeclaredAsync(bool dontAsk = false)
    {
        var filesToRemove = Directory.EnumerateFiles(config.MusicPath)
            .Where(f => !config.Tracks.Any(track => track.EvaluateFilename() == Path.GetFileNameWithoutExtension(f)))
            .ToArray();
        var directoriesToRemove = Directory.EnumerateDirectories(config.MusicPath)
            .Where(d => !config.Playlists.Any(playlist => playlist.EvaluateDirectoryName() == d))
            .ToArray();

        bool allowed = true;
        if (!dontAsk)
        {
            Console.WriteLine("The next files and directories will be removed:");
            foreach (var entryToRemove in filesToRemove.Concat(directoriesToRemove))
                Console.WriteLine($"- {entryToRemove}");
            Console.WriteLine("Are you sure you want to delete these files? (y/n)");
            allowed = Console.ReadLine()?.Equals("y", StringComparison.InvariantCultureIgnoreCase) ?? false;
        }
        if (allowed)
        {
            foreach (var f in filesToRemove)
            {
                try
                {
                    File.Delete(f);
                }
                catch
                {
                    Console.Error.WriteLine($"Can't remove '{f}'");
                }
            }
            foreach (var d in directoriesToRemove)
            {
                try
                {
                    Directory.Delete(d, true);
                }
                catch
                {
                    Console.Error.WriteLine($"Can't remove '{d}'");
                }
            }
        }
    }
}
