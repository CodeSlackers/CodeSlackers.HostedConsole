namespace sample;

public interface IStateService<T>
{
    T GetState();
}