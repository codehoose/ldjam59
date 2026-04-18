namespace HackThePlanet.FSM
{
    internal interface IState
    {
        void Enter(StateManager stateManager);
        void Exit();
        void Tick(float deltaTime);
    }
}
