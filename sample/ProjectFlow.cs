using CodeSlackers.HostedConsole;
using CodeSlackers.HostedConsole.Logging;
using Microsoft.Extensions.Logging;

namespace sample;

public class ProjectFlow(IFlowIoService flowIoService, IStateService<SolutionBuilderState> stateService, ILogger<ProjectFlow> logger) : IFlow
{
    private readonly List<string> _questions = new() { "webapi", "worker", "blazor" };
    
    public string FlowName => SolutionBuilderFlows.ProjectFlow;
    public string NextFlow { get; set; } = SolutionBuilderFlows.Quit;
    public async Task Run()
    {
        var state = stateService.GetState();
        var projectName = string.Empty;
        var projectType = string.Empty;

        var menuQuestions = MenuManager.AnswerQuestion(_questions, (menu, question) =>
        {
            projectType = question;
            menu.CloseMenu();
        }, "Please Select a Project Type");

        menuQuestions.Show();
        projectName = GetProjectName(state);
        await AddProject(state, projectName, projectType);
        logger.LogEvent($"Added project {projectName} of {projectType} ", state);
        var menu = MenuManager.YesNo(
            () => { NextFlow = SolutionBuilderFlows.ProjectFlow; }, () =>
            {
                logger.LogEvent("Solution Built", state);
                NextFlow = SolutionBuilderFlows.Quit;
            },
            "Do you wish to add another project?");
        menu.Show();
    }


    private async Task AddProject(SolutionBuilderState state, string projectName, string projectType)
    {
        var projectTemplate = string.Empty;
        switch (projectType)
        {
            case "webapi":
                projectTemplate = "webapi -minimal false";
                break;
            case "worker":
                projectTemplate = "worker";
                break;
            case "blazor":
                projectTemplate = "blazorwasm";
                break;
        }

        var resultOne = await CliWrap.Cli.Wrap("dotnet").WithArguments($"new {projectTemplate} -n {projectName}").WithWorkingDirectory($"{state.WorkingDirectory}/{state.SolutionName}").ExecuteAsync();
        logger.LogInformation(resultOne.RunTime.ToString());
        var resultTwo = await CliWrap.Cli.Wrap("dotnet").WithArguments(@$"dotnet sln add .\{projectName}").WithWorkingDirectory($"{state.WorkingDirectory}/{state.SolutionName}").ExecuteAsync();
        logger.LogInformation(resultTwo.RunTime.ToString());
    }


    private string GetProjectName(SolutionBuilderState state)
    {
        var projectName = string.Empty;
        flowIoService.WriteLine("Please provide project name");
        bool validName = false;
        while (validName == false)
        {
            projectName = flowIoService.ReadLine();
            if (string.IsNullOrEmpty(projectName) || projectName.ToLower() == "quit")
            {
                flowIoService.WriteLine("Please enter the name of your solution or type quit to exit");
                projectName = flowIoService.ReadLine();
            }

            if (projectName.ToLower() == "quit")
            {
                return projectName;
            }

            if (state.ProjectNames.Contains(projectName))
            {
                flowIoService.WriteLine("Project name already exists, please enter a new name");
            }
            else
            {
                state.ProjectNames.Add(projectName);

                validName = true;
            }
        }

        return projectName;
    }


}