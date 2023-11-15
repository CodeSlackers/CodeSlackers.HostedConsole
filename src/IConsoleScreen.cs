namespace CodeSlackers.HostedConsole;

public interface IConsoleScreen
{
    string Title { get; }
    Task Show();

}