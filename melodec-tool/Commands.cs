using System.Text.Json;
using ConsoleAppFramework;
using melodec_tool.Models;
using melodec_tool.Models.SerializerContexts;
namespace melodec_tool;

public class Commands
{
    /// <summary>
    /// Download missing tracks/playlists
    /// </summary>
    /// <param name="config">Filepath to the config</param>
    [Command("")]
    public async Task Root(string config = "")
    {
        if (string.IsNullOrWhiteSpace(config))
            config = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "melodec", "config.json");
        if (!File.Exists(config)) Environment.FailFast($"File '{config}' does not exist");
        Config? conf = JsonSerializer.Deserialize(await File.ReadAllTextAsync(config), ConfigSerializerContext.Default.Config);
        if (conf is null) Environment.FailFast($"Cannot parse the config file: {config}");
        await Synchronizer.SyncAsync(conf);
    }
}
