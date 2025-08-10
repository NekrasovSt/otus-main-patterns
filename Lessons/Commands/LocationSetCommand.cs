using Lessons.Infrastructure;

namespace Lessons.Commands;

public class LocationSetCommand: ICommand
{
    private readonly UObject _universalObject;
    private readonly Vector _location;

    public LocationSetCommand(UObject universalObject, Vector location)
    {
        _universalObject = universalObject;
        _location = location;
    }

    public void Execute()
    {
        _universalObject["Location"] = _location;
    }
}