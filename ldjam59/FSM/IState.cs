namespace HackThePlanet.FSM
{
    public interface IState
    {
        void Enter(IStateManager stateManager);
        void Exit(IStateManager stateManager);
        void Tick(float deltaTime);
    }
}
