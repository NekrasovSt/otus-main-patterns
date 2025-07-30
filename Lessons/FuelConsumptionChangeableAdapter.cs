namespace Lessons;

public class FuelConsumptionChangeableAdapter : IFuelConsumptionChangeable
{
    private readonly UObject _universalObject;

    public FuelConsumptionChangeableAdapter(UObject universalObject)
    {
        _universalObject = universalObject;
    }

    public int FuelConsumption
    {
        set => _universalObject[nameof(FuelConsumption)] = value;
    }
}