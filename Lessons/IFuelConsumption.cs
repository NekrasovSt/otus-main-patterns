namespace Lessons;

public interface IFuelConsumption
{
    int FuelAmount { get; }
    int FuelConsumption { get; }

    void Burn(int currentVelocity);
}