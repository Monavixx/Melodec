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
    /// <param name="config">-c, Filepath to the config (Default is ~/.config/melodec/config.json)</param>
    /// <param name="forceDelete">-f, If set, it won't ask permission to delete undeclared file; otherwise, it will</param>
    /// <param name="redownloadAll">-r, If set, it will redownload all the entries</param>
    [Command("")]
    public async Task Root([HideDefaultValue] string config = "", bool forceDelete = false, bool redownloadAll = false)
    {
        if (string.IsNullOrWhiteSpace(config))
            config = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "melodec", "config.json");
        if (!File.Exists(config)) Environment.FailFast($"File '{config}' does not exist");
        Config? conf = JsonSerializer.Deserialize(await File.ReadAllTextAsync(config), ConfigSerializerContext.Default.Config);
        if (conf is null) Environment.FailFast($"Cannot parse the config file: {config}");
        var synchronizer = new Synchronizer(conf);
        await synchronizer.SyncAsync(forceDelete, redownloadAll);
    }
}
