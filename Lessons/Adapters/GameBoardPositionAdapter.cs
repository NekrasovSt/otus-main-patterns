namespace Lessons.Adapters;

public class GameBoardPositionAdapter : IGameBoardPosition
{
    private readonly UObject _universalObject;

    public GameBoardPositionAdapter(UObject universalObject)
    {
        _universalObject = universalObject;
    }

    public List<Position> Positions
    {
        get => (List<Position>)_universalObject.GetValueOrDefault(nameof(Positions), new List<Position>());
        set => _universalObject[nameof(Positions)] = value;
    }
}