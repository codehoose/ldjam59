namespace HackThePlanet.FSM.Gameplay
{
    using HackThePlanet.Models;

    internal class InitializeGameState : BaseState<InitializeGameState>
    {
        public override void Enter(IStateManager stateManager)
        {
            GameState.Instance.Init();
            stateManager.ChangeState(HandoffScreenState.Instance);
        }
    }
}
