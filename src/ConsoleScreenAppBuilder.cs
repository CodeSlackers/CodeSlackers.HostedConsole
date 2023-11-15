using CodeSlackers.HostedConsole.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CodeSlackers.HostedConsole;

public static class ConsoleScreenAppBuilder
{

    /// <summary>
    /// Creates a hosted console application and returns main console
    /// </summary>
    /// <param name="configurationFile">name of json file</param>
    /// <param name="mainScreenTitle">FlowName of main screen</param>
    /// <param name="addServices">action to add services</param>
    /// <returns><see cref="IConsoleScreen"/></returns>
    public static IConsoleScreen CreateConfigureConsoleScreenApplication(
        string mainScreenTitle,
        Action<HostBuilderContext, IServiceCollection> addServices)
    {
        var builder = GetHostBuilder(addServices);

        builder.AddLogging();
        var host = builder.Build();
        var consoleScreen = GetConsoleScreen(mainScreenTitle, host);
        return consoleScreen;
    }

    private static IConsoleScreen GetConsoleScreen(string mainScreenTitle, IHost host)
    {
        using IServiceScope serviceScope = host.Services.CreateScope();
        IServiceProvider provider = serviceScope.ServiceProvider;
        var mainScreen = provider.GetServices<IConsoleScreen>().Where(s => s.Title == mainScreenTitle);

        var consoleScreens = mainScreen as IConsoleScreen[] ?? mainScreen.ToArray();
        if (!consoleScreens.Any() || consoleScreens.Count() > 1)
        {
            Console.WriteLine("Invalid Configuration");
            throw new InvalidOperationException("Invalid Configuration");
        }

        return consoleScreens[0];
    }

    private static IHostBuilder GetHostBuilder(Action<HostBuilderContext, IServiceCollection> addServices)
    {
        var configurationBuilder = new ConfigurationBuilder();
        IHostBuilder builder = Host.CreateDefaultBuilder()
            .ConfigureServices(addServices);
        return builder;
    }
}