using System.Diagnostics;
using melodec_tool.Models;

namespace melodec_tool;

public class Downloader(Config config)
{
    public async Task DownloadAsync(Track track)
    {
        string directory = config.MusicPath;
        if (!string.IsNullOrWhiteSpace(track.Album))
            directory = Path.Combine(directory, $"{track.Author} - {track.Album}");
        var processStartInfo = new ProcessStartInfo
        {
            FileName = "yt-dlp",
            Arguments = $"--cookies-from-browser chrome " +
                "--no-progress " +
                $"-N 6 -x --audio-format mp3 " +
                $"-P \"{directory}\" -o \"{track.EvaluateFilename()}.%(ext)s\" " +
                $"--postprocessor-args \"ExtractAudio:-metadata title='{track.Title}' -metadata artist='{track.Author}' -metadata album='{track.Album}'\" " +
                $"\"{track.Url}\"",
            UseShellExecute = false,
            CreateNoWindow = true,
            // RedirectStandardError = false,
            // RedirectStandardInput = true,
            // RedirectStandardOutput = true
        };
        using var process = new Process { StartInfo = processStartInfo };

        try
        {
            process.Start();
            await process.WaitForExitAsync();
            if (process.ExitCode != 0)
                Console.Error.WriteLine($"yt-dlp returned code {process.ExitCode} while downloading '{track.EvaluateFilename()}' [{track.Url}]");
            Console.WriteLine($"'{track.EvaluateFilename()}' has been downloaded");
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
            Arguments = $"-N 6 --no-progress --cookies-from-browser chrome -x --audio-format mp3 -P \"{playlistDirectory}\" -o \"%(playlist_autonumber)s - {playlist.Author} - %(track,title)s.%(ext)s\" \"{playlist.Url}\"",
            UseShellExecute = false,
            CreateNoWindow = true,
            // RedirectStandardError = false,
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
            Console.WriteLine($"'{playlist.EvaluateDirectoryName()}' has been downloaded");
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
