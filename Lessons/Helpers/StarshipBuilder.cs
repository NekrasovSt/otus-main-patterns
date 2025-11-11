using Lessons.Adapters;

namespace Lessons.Helpers;

public static class StarshipBuilder
{
    public static UObject CreateObject()
    {
        return new UObject();
    }

    public static UObject SetId(this UObject universalObject, Guid id)
    {
        universalObject.Add("Id", id);
        return universalObject;
    }

    public static UObject SetPlayerId(this UObject universalObject, Guid playerId)
    {
        var adapter = new PlayerSetterAdapter(universalObject)
        {
            PlayerId = playerId
        };
        return universalObject;
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

    public static UObject GetFromDictionary(Dictionary<string, object> dictionary)
    {
        var uobject = new UObject();
        foreach (var pair in dictionary)
        {
            var valueDictionary = pair.Value as Dictionary<string, object>;
            switch (pair.Key)
            {
                case "Velocity":
                    uobject.Add("Velocity", new Vector(){X = Convert.ToInt32(valueDictionary["x"]), Y = Convert.ToInt32(valueDictionary["y"])});
                    break;
                case "Location":
                    uobject.Add("Location", new Vector(){X = Convert.ToInt32(valueDictionary["x"]), Y = Convert.ToInt32(valueDictionary["y"])});
                    break;
                case "Id":
                    uobject.Add("Id", Guid.Parse(pair.Value.ToString()));
                    break;
            }
        }

        return uobject;
    }
}