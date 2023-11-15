using CodeSlackers.HostedConsole;
using Microsoft.Extensions.Options;

namespace sample;

public class StateService : IStateService<SolutionBuilderState>
{
    
    private readonly SolutionBuilderState _state = new();

    public SolutionBuilderState GetState()
    {
        return _state;
    }
}