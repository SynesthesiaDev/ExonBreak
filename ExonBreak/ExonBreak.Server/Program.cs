using Serilog;
using Serilog.Sinks.SpectreConsole;

namespace ExonBreak.Server;

internal class Program
{
    private static void Main(string[] args)
    {
        using var log = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.SpectreConsole(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u4}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();

        Log.Logger = log;
        var server = new DedicatedServer(false);
        server.Run();
    }
}
