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
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine($"yt-dlp returned code {process.ExitCode} while downloading '{track.EvaluateFilename()}' [{track.Url}]");
                Console.ResetColor();

                var filename = Path.Combine(directory, track.EvaluateFilename() + ".mp3");
                if (File.Exists(filename))
                {
                    File.Delete(filename);
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"'{track.EvaluateFilename()}' has been downloaded");
                Console.ResetColor();
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine($"Downloading '{track.EvaluateFilename()}' [{track.Url}] failed with message: {ex.Message}");
            Console.ResetColor();
            var filename = Path.Combine(directory, track.EvaluateFilename() + ".mp3");
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
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
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine($"yt-dlp returned code {process.ExitCode} while downloading '{playlist.EvaluateDirectoryName()}' [{playlist.Url}]");
                Console.ResetColor();
                if (Directory.Exists(playlistDirectory))
                {
                    Directory.Delete(playlistDirectory, true);
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"'{playlist.EvaluateDirectoryName()}' has been downloaded");
                Console.ResetColor();
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine($"Downloading '{playlist.EvaluateDirectoryName()}' [{playlist.Url}] failed with message: {ex.Message}");
            Console.ResetColor();
            if (Directory.Exists(playlistDirectory))
            {
                Directory.Delete(playlistDirectory, true);
            }
        }

    }

    public async Task DownloadAsync(Track[] tracks)
    {
        foreach (var track in tracks)
        {
            await DownloadAsync(track);
        }
    }

    public async Task DownloadAsync(Playlist[] playlists)
    {
        foreach (var playlist in playlists)
        {
            await DownloadAsync(playlist);
        }
    }
}
