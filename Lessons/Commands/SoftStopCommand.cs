using Lessons.Infrastructure;

namespace Lessons.Commands;

public class SoftStopCommand(ServerThread st) : ICommand
{
    public void Execute()
    {
        Action oldBehaviour = st.Behaviour;
        st.Behaviour = () =>
        {
            if (st.Length > 0)
            {
                oldBehaviour();
            }
            else
            {
                st.Stop();
            }
        };
    }
}