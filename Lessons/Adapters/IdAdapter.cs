using Lessons.Infrastructure;

namespace Lessons.Adapters;

public class IdAdapter : IId
{
    private readonly UObject _uObject;

    public IdAdapter(UObject uObject)
    {
        _uObject = uObject;
    }

    public Guid Id
    {
        get
        {
            if (!_uObject.ContainsKey(nameof(Id)))
            {
                return Guid.Empty;
            }

            if (_uObject[nameof(Id)] is Guid id)
            {
                return id;
            }

            if (Guid.TryParse(_uObject[nameof(Id)].ToString(), out id))
            {
                return id;
            }

            return Guid.Empty;
        }
    }
}