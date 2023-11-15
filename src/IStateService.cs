namespace CodeSlackers.HostedConsole;

public interface IStateService<T>
{
    T GetState();
}