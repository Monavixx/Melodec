using melodec_tool.Models;

namespace melodec_tool;

public class Synchronizer(Config config)
{
    public async Task SyncAsync(bool dontAsk, bool redownloadAll)
    {
        switch ((config.KeepUndeclared, redownloadAll))
        {
            case (true, true):
                RemoveDeclared(dontAsk);
                break;
            case (true, false):
                break;
            case (false, true):
                RemoveAll(dontAsk);
                break;
            case (false, false):
                RemoveUndeclared(dontAsk);
                break;
        }
        var tracks = Directory.GetFiles(config.MusicPath, "*.mp3");
        var tracksToDownload = config.Tracks
            .Where(track => !tracks.Any(t => track.EvaluateFilename() == Path.GetFileNameWithoutExtension(t)))
            .ToArray();
        var playlists = Directory.GetDirectories(config.MusicPath);
        var playlistsToDownload = config.Playlists
            .Where(playlist => !playlists.Any(p => Path.GetFileName(Path.TrimEndingDirectorySeparator(p)) == playlist.EvaluateDirectoryName()))
            .ToArray();
        var downloader = new Downloader(config);

        await downloader.DownloadAsync(tracksToDownload);
        await downloader.DownloadAsync(playlistsToDownload);
    }

    private void RemoveAll(bool dontAsk)
    {
        RemoveFilesAndDirectories(
                Directory.GetFiles(config.MusicPath),
                Directory.GetDirectories(config.MusicPath),
                dontAsk
                );
    }

    private void RemoveDeclared(bool dontAsk)
    {
        var filesToRemove = config.Tracks.Select(t => Path.Combine(config.MusicPath, t.EvaluateFilename() + ".mp3"))
            .Where(f => File.Exists(f))
            .ToArray();
        var directoriesToRemove = config.Playlists.Select(p => Path.Combine(config.MusicPath, p.EvaluateDirectoryName()))
            .Where(d => Directory.Exists(d))
            .ToArray();
        RemoveFilesAndDirectories(filesToRemove, directoriesToRemove, dontAsk);
    }

    private void RemoveUndeclared(bool dontAsk)
    {
        var filesToRemove = Directory.EnumerateFiles(config.MusicPath)
            .Where(f => !config.Tracks.Any(track => track.EvaluateFilename() == Path.GetFileNameWithoutExtension(f)))
            .ToArray();
        var directoriesToRemove = Directory.EnumerateDirectories(config.MusicPath)
            .Where(d => !config.Playlists.Any(playlist => playlist.EvaluateDirectoryName() ==
                         Path.GetFileName(Path.TrimEndingDirectorySeparator(d))))
            .ToArray();

        RemoveFilesAndDirectories(filesToRemove, directoriesToRemove, dontAsk);
    }
    private static void RemoveFilesAndDirectories(string[] files, string[] directories, bool dontAsk)
    {
        bool allowed = true;
        if (!dontAsk)
        {
            Console.WriteLine("The next files and directories will be removed:");
            foreach (var entryToRemove in files.Concat(directories))
                Console.WriteLine($"- {entryToRemove}");
            Console.WriteLine("Are you sure you want to delete these files? (y/n)");
            allowed = Console.ReadLine()?.Equals("y", StringComparison.InvariantCultureIgnoreCase) ?? false;
        }
        if (allowed)
        {
            foreach (var f in files)
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
            foreach (var d in directories)
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
