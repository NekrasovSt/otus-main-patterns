namespace Lessons;

public class FuelConsumptionAdapter : IFuelConsumption
{
    private readonly UObject _universalObject;

    public FuelConsumptionAdapter(UObject universalObject)
    {
        _universalObject = universalObject;
    }

    public int FuelAmount => (int)_universalObject[nameof(FuelAmount)];

    public int FuelConsumption => (int)_universalObject[nameof(FuelConsumption)];
    public void Burn(int currentVelocity)
    {
        _universalObject[nameof(FuelAmount)] = FuelAmount - currentVelocity * FuelConsumption;
    }
}