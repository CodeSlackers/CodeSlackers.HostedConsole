namespace CodeSlackers.HostedConsole;

public interface IFlow<T>
{
    string FlowName { get; }

    string NextFlow { get; set; }

    Task Run();

}