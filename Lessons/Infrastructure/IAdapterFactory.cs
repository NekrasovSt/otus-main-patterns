namespace Lessons.Infrastructure;

public interface IAdapterFactory
{
    object Create(UObject uObject);
    void RegisterDependencies();
}