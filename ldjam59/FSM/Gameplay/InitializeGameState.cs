namespace HackThePlanet.FSM.Gameplay
{
    internal class InitializeGameState : BaseState<InitializeGameState>
    {
        public override void Enter(StateManager stateManager)
        {
            stateManager.Game.State.Init();
            stateManager.ChangeState(HandoffScreenState.Instance);
        }
    }
}
