using Serilog;
using Serilog.Events;
using Serilog.Sinks.SpectreConsole;

namespace ExonBreak.Server;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        using var log = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.SpectreConsole(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u4}] {Message:lj}{NewLine}{Exception}", minLevel: LogEventLevel.Verbose)
            .CreateLogger();

        Log.Logger = log;
        var server = new DedicatedServer(false);
        server.Run();
    }
}
