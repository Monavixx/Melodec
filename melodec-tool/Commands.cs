using ConsoleAppFramework;
namespace melodec_tool;

public class Commands
{
    /// <summary>
    /// Download missing tracks/playlists
    /// </summary>
    /// <param name="config">Filepath to the config</param>
    [Command("")]
    public void Root(string config = "")
    {
        if (string.IsNullOrWhiteSpace(config))
            config = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "/melodec/config.json");
    }
}
