using CodeSlackers.HostedConsole;
using CodeSlackers.HostedConsole.Logging;
using Microsoft.Extensions.Logging;

namespace sample;

public class SolutionFlow(
        IFlowIoService flowIoService, 
        IStateService<SolutionBuilderState> stateService,
        ILogger<SolutionFlow> logger) 
    : IFlow
{
    public string FlowName => SolutionBuilderFlows.SolutionFlow;
    public string NextFlow { get; set; } = SolutionBuilderFlows.Quit;
    public async Task Run()
    {
        var state = stateService.GetState();
        flowIoService.WriteLine("Please Provide a Working Directory or hit enter to use current directory");
        state.WorkingDirectory = flowIoService.ReadLine();
        if (string.IsNullOrEmpty(state.WorkingDirectory))
        {
           state.WorkingDirectory = Directory.GetCurrentDirectory();
        }

        flowIoService.WriteLine("Please enter the name of your solution or type quit to exit");
        var solutionName = flowIoService.ReadLine();
        var diretory = $"{state.WorkingDirectory}/{state.SolutionName}";
        while (string.IsNullOrEmpty(solutionName) || solutionName.ToLower() == "quit" || !Directory.Exists(diretory) )
        {
            if (Directory.Exists(diretory))
            {
                flowIoService.WriteLine("Directory already exists. Please enter the name of your solution or type quit to exit");
            }
            else
            {
                flowIoService.WriteLine("Please enter the name of your solution or type quit to exit");
            }

            solutionName = flowIoService.ReadLine();
        }

        if (solutionName.ToLower() == "quit")
        {
            return;
        }

        state.SolutionName = solutionName;
        logger.LogEvent($"Creating solution {solutionName}", state );
        
        
        Directory.CreateDirectory($"{state.WorkingDirectory}/{state.SolutionName}");
        await CliWrap.Cli.Wrap("dotnet").WithArguments($"new sln -n {solutionName}").WithWorkingDirectory($"{state.WorkingDirectory}/{state.SolutionName}").ExecuteAsync();
        NextFlow = SolutionBuilderFlows.ProjectFlow;
    }
}