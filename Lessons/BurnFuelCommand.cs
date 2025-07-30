using Lesson4.Infrastructure;

namespace Lesson4;

public class BurnFuelCommand: ICommand
{
    private readonly IFuelConsumption _fuelConsumption;
    private readonly IMovableObject _movableObject;

    public BurnFuelCommand(IFuelConsumption fuelConsumption, IMovableObject movableObject)
    {
        _fuelConsumption = fuelConsumption;
        _movableObject = movableObject;
    }

    public void Execute()
    {
        _fuelConsumption.Burn(_movableObject.Velocity.Length);
    }
}