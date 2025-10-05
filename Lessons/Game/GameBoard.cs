using Lessons.Adapters;

namespace Lessons.Game;

public class GameBoard : ICollisionFinder
{
    public const int SIZE = 10;
    public const int MAX_COORDINATE = 100;
    private GameBlock[,] _gameBlocks = new GameBlock[SIZE, SIZE];

    public GameBoard()
    {
        for (var x = 0; x < SIZE; x++)
        {
            for (var y = 0; y < SIZE; y++)
            {
                _gameBlocks[x, y] = new GameBlock(x, y);
            }
        }
    }

    public void AddOrUpdateToBoard(UObject gameObject)
    {
        var adapter = new MovableAdapter(gameObject);
        if (adapter.Location.X < 0 || adapter.Location.X > MAX_COORDINATE)
        {
            throw new ArgumentException("Недопустимое значение координат", nameof(adapter.Location.X));
        }

        if (adapter.Location.Y < 0 || adapter.Location.Y > MAX_COORDINATE)
        {
            throw new ArgumentException("Недопустимое значение координат", nameof(adapter.Location.Y));
        }
        var positionAdapter = new GameBoardPositionAdapter(gameObject);
        foreach (var position in positionAdapter.Positions)
        {
            RemoveObjects(position.X, position.Y, gameObject);
        }

        positionAdapter.Positions = [];
        
        var areaX = GetAreas(adapter.Location.X);
        var areaY = GetAreas(adapter.Location.Y);
        var positions = new List<Position>();
        foreach (var x in areaX)
        {
            foreach (var y in areaY)
            {
                _gameBlocks[x, x].AddObject(gameObject);
                positions.Add(new Position(x, y));
            }
        }
        
        positionAdapter.Positions = positions;
    }

    private void RemoveObjects(int x, int y, UObject obj)
    {
        _gameBlocks[x, y].Remove(obj);
    }
    private IEnumerable<int> GetAreas(int location)
    {
        var position = location / SIZE;
        var areas = new List<int>() { position };
        if (location % SIZE == 0 && position > 0)
        {
            areas.Add(position - 1);
        }

        return areas;
    }

    public IEnumerable<Collision> Exam()
    {
        var result = new List<Collision>();
        foreach (var gameBlock in _gameBlocks)
        {
            result.AddRange(gameBlock.Exam());
        }

        return result;
    }
}