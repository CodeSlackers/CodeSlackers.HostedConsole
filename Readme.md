# CodeSlackers.HostedConsole
Probably a bad name, but this sets up all the niceties you would get with a hosted app: DI and Logging, along with a way to split up a Console

## Key Concepts
### ConsoleScreenAppBuilder: 
CreateConfigureConsoleScreenApplication to bootstrap the console app 
```csharp
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

```

### IConsoleScreen: 
This is the main view of any flow set.  and the apps primary entry point to the user
```csharp
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
```
### IStateService
Manages the state from flow to flow. Just inject as a singleton and you are good
```chsarp
public interface IStateService<T>
{
    T GetState();
}
```
### IFlow
This is where all the work happens. IO to the screen and, based on user selection set the next state.

```csharp
public class SolutionFlow(
        IFlowIoService flowIoService, 
        IStateService<SolutionBuilderState> stateService,
        ILogger<SolutionFlow> logger) 
    : IFlow<SolutionBuilderState>
{
    public string FlowName => SolutionBuilderFlows.SolutionFlow;
    public string NextFlow { get; set; } = SolutionBuilderFlows.Quit;
    public async Task Run()
    {
        var state = stateService.GetState();
        flowIoService.Writeline(what do you want?)
        //Removed for brevite---
        NextFlow = SolutionBuilderFlows.ProjectFlow;
    }
}```
### Menu Manager: 
simple wrapper around a console menu tool [ConsoleMenu](https://github.com/lechu445/ConsoleMenu)
```csharp
    // Simple yes or now
        var menuQuestions = MenuManager.AnswerQuestion(_questions, (menu, question) =>
        {
            projectType = question;
            menu.CloseMenu();
        }, "Please Select a Project Type");

        menuQuestions.Show();

    // multiple choise
    private readonly List<string> _questions = new() { "webapi", "worker", "blazor" };

    var menuQuestions = MenuManager.AnswerQuestion(_questions, (menu, question) =>
    {
        projectType = question;
        menu.CloseMenu();
    }, "Please Select a Project Type");


```
### FAQ
**Is this too complicated?**

Probably, but it does make testing a lot easier, later I can show code coverage and the long term goal is to have a library of flows that can be chained togeter.


