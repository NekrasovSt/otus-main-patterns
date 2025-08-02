using Lessons.Exceptions;
using Lessons.Infrastructure;

namespace Lessons.Commands;

public class CheckFuelCommand: ICommand
{
    private readonly IFuelConsumption _fuelConsumption;
    private readonly IMovableObject _movableObject;

    public CheckFuelCommand(IFuelConsumption fuelConsumption, IMovableObject movableObject)
    {
        _fuelConsumption = fuelConsumption;
        _movableObject = movableObject;
    }

    public void Execute()
    {
        if (_movableObject.Velocity.Length * _fuelConsumption.FuelConsumption > _fuelConsumption.FuelAmount)
        {
            throw new CommandException();
        }
    }
}