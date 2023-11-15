namespace CodeSlackers.HostedConsole;

public interface IFlow
{
    string FlowName { get; }

    string NextFlow { get; set; }

    Task Run();

}