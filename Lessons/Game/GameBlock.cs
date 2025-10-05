using Lessons.Adapters;

namespace Lessons.Game;

public class GameBlock : ICollisionFinder
{
    private readonly HashSet<UObject> _objects = new HashSet<UObject>();
    private int PositionX;
    private int PositionY;

    public GameBlock(int positionY, int positionX)
    {
        PositionY = positionY;
        PositionX = positionX;
    }

    public void AddObject(UObject gameObject)
    {
        _objects.Add(gameObject);
    }

    public void Remove(UObject gameObject)
    {
        _objects.Remove(gameObject);
    }

    public IEnumerable<Collision> Exam()
    {
        var array = _objects.ToArray();

        var result = new List<Collision>();
        for (int i = 0; i < array.Length; i++)
        {
            for (int j = i + 1; j < array.Length; j++)
            {
                if (i != j)
                {
                    var obj1 = new MovableAdapter(array[i]);
                    var obj2 = new MovableAdapter(array[j]);
                    if (obj1.Location.Equals(obj2.Location))
                    {
                        result.Add(new Collision(array[i], array[j]));
                    }
                }
            }
        }

        return result;
    }
}