using CodeSlackers.HostedConsole;
using Microsoft.Extensions.DependencyInjection;
using sample;


var screen = ConsoleScreenAppBuilder.CreateConfigureConsoleScreenApplication(
    SolutionBuilderFlows.SolutionBuilderScreen,
    (context, collection) =>
    {
        collection.AddSingleton<IFlowIoService, FlowIoService>();
        collection.AddSingleton<IStateService<SolutionBuilderState>, StateService>();
        collection.AddTransient<IFlow<SolutionBuilderState>, SolutionFlow>();
        collection.AddTransient<IFlow<SolutionBuilderState>, ProjectFlow>();
        collection.AddSingleton<IConsoleScreen, SolutionBuilderScreen>();
    });

await screen.Show();

/*
using CliWrap;
var result = await Cli.Wrap("dotnet")
       .WithArguments("new webapi --name Foobar")
       .WithWorkingDirectory(@"d:\code\Scratchpad")
       .ExecuteAsync();

*/