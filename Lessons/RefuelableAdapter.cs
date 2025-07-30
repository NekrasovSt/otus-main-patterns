using DefaultNamespace;

namespace Lesson4;

public class RefuelableAdapter : IRefuelable
{
    private readonly UObject _universalObject;

    public RefuelableAdapter(UObject universalObject)
    {
        _universalObject = universalObject;
    }
    public void AddFuelAmount(int fuel)
    {
        if (fuel < 0)
        {
            throw new ArgumentException(nameof(fuel));
        }

        _universalObject.TryAdd("FuelAmount", 0);
        _universalObject["FuelAmount"] = (int)_universalObject["FuelAmount"] + fuel;
    }
}