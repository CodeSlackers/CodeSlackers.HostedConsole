﻿using CodeSlackers.HostedConsole;

namespace sample;

public class SolutionBuilderScreen : IConsoleScreen
{
    
    private readonly IEnumerable<IFlow<SolutionBuilderState>> _flows;
    public SolutionBuilderScreen(
        IEnumerable<IFlow<SolutionBuilderState>> flows)
    {
        _flows = flows;

    }
    public string Title => SolutionBuilderFlows.SolutionBuilderScreen;
    public async Task Show()
    {
        Console.WriteLine("Welcome to the .net solution builder");
        var nextFlow = SolutionBuilderFlows.SolutionFlow;
        while (nextFlow != SolutionBuilderFlows.Quit)
        {
            var flow = _flows.FirstOrDefault(f => f.FlowName == nextFlow);

            if (flow == null)
            {
                Console.WriteLine("No solution flow found");
                return;
            }

            await flow.Run();
            nextFlow = flow.NextFlow;
        }
        
    }
}