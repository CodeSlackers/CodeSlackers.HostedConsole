using Microsoft.Extensions.Hosting;
using Serilog.Formatting.Json;
using Serilog;
using System.Reflection;

namespace CodeSlackers.HostedConsole.Logging;

public static class LoggingSetupExtension
{
    // <summary>
    /// Helper method to extend IHostBuilder to ddd structured logging.
    /// <summary>
    /// </summary>
    /// <param name="builder"><see cref="IHostBuilder"/></param>
    public static void AddLogging(this IHostBuilder builder)
    {
        var name = string.Empty;
        var s = Assembly.GetCallingAssembly().GetName().Name;
        if (s != null) name = s.ToLower();

        builder.UseSerilog((ctx, lc) =>
        {
            lc.WriteTo.Console();
            var path = $"{Environment.CurrentDirectory}//{s}.json";
            lc.WriteTo.File(new JsonFormatter(), path, rollingInterval: RollingInterval.Year);
        });
    }
}