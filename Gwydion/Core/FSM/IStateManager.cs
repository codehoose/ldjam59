namespace Gwydion.Core.FSM
{
    using Microsoft.Xna.Framework;

    public interface IStateManager
    {
        Game Game { get; }

        void ChangeState(IState newState);
        void Tick(float deltaTime);
    }

    public interface IStateManager<T> : IStateManager where T : Game
    {
        new T Game { get; }
    }
}
