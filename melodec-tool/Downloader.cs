using System.Diagnostics;
using melodec_tool.Models;

namespace melodec_tool;

public class Downloader(Config config)
{
    public async Task DownloadAsync(Track track)
    {
        var processStartInfo = new ProcessStartInfo
        {
            FileName = "yt-dlp",
            Arguments = $"--cookies-from-browser chrome -x --audio-format mp3 -P \"{config.MusicPath}\" -o \"{track.EvaluateFilename()}.%(ext)s\" \"{track.Url}\"",
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardError = true,
            RedirectStandardInput = true,
            RedirectStandardOutput = true
        };
        using var process = new Process { StartInfo = processStartInfo };

        try
        {
            process.Start();
            await process.WaitForExitAsync();
            if (process.ExitCode != 0)
                Console.Error.WriteLine($"yt-dlp returned code {process.ExitCode} while downloading '{track.EvaluateFilename()}' [{track.Url}]");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Downloading '{track.EvaluateFilename()}' [{track.Url}] failed with message: {ex.Message}");
        }
    }

    public async Task DownloadAsync(Playlist playlist)
    {
        string playlistDirectory = Path.Combine(config.MusicPath, playlist.EvaluateDirectoryName());
        var processStartInfo = new ProcessStartInfo
        {
            FileName = "yt-dlp",
            Arguments = $"-N 6 --cookies-from-browser chrome -x --audio-format mp3 -P \"{playlistDirectory}\" -o \"%(playlist_index)s - %(artist,uploader)s - %(track,title)s.%(ext)s\" \"{playlist.Url}\"",
            UseShellExecute = false,
            CreateNoWindow = true,
            // RedirectStandardError = true,
            // RedirectStandardInput = true,
            // RedirectStandardOutput = true
        };
        using var process = new Process { StartInfo = processStartInfo };

        try
        {
            process.Start();
            await process.WaitForExitAsync();
            if (process.ExitCode != 0)
                Console.Error.WriteLine($"yt-dlp returned code {process.ExitCode} while downloading '{playlist.EvaluateDirectoryName()}' [{playlist.Url}]");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Downloading '{playlist.EvaluateDirectoryName()}' [{playlist.Url}] failed with message: {ex.Message}");
        }

    }

    public Task DownloadAsync(Track[] tracks)
    {
        return Task.WhenAll(tracks.Select(DownloadAsync));
    }

    public Task DownloadAsync(Playlist[] playlists)
    {
        return Task.WhenAll(playlists.Select(DownloadAsync));
    }
}
