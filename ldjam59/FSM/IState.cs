namespace HackThePlanet.FSM
{
    internal interface IState
    {
        void Enter(StateManager stateManager);
        void Exit(StateManager stateManager);
        void Tick(float deltaTime);
    }
}
