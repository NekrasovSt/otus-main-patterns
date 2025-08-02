namespace Lessons.Helpers;

public static class StarshipBuilder
{
    public static UObject CreateObject()
    {
        return new UObject();
    }

    public static UObject SetVelocity(this UObject universalObject, int x, int y)
    {
        var velocityChangeableAdapter = new VelocityChangeableAdapter(universalObject)
        {
            Velocity = new Vector()
            {
                X = x, Y = y
            }
        };
        return universalObject;
    }
    public static UObject SetLocation(this UObject universalObject, int x, int y)
    {
        var velocityChangeableAdapter = new MovableAdapter(universalObject)
        {
            Location = new Vector()
            {
                X = x, Y = y
            }
        };
        return universalObject;
    }
    public static UObject SetFuelConsumption(this UObject universalObject, int fuelConsumption)
    {
        var fuelConsumptionChangeableAdapter = new FuelConsumptionChangeableAdapter(universalObject)
        {
            FuelConsumption = fuelConsumption
        };
        return universalObject;
    }
    public static UObject SetFuelAmount(this UObject universalObject, int fuelAmount)
    {
        var refuelableAdapter = new RefuelableAdapter(universalObject);
        refuelableAdapter.AddFuelAmount(fuelAmount);
        return universalObject;
    }

    public static UObject SetRotation(this UObject universalObject, int angular, int angularVelocity)
    {
        var rotatableAdapter = new RotatableAdapter(universalObject)
        {
            Angular = angular
        };
        var angularVelocityChangeable = new AngularVelocityChangeableAdapter(universalObject)
        {
            AngularVelocity = angularVelocity
        };
        return universalObject;
    }
}