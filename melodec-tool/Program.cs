
using ConsoleAppFramework;
using melodec_tool;

var app = ConsoleApp.Create();

app.Add<Commands>();
await app.RunAsync(args);
