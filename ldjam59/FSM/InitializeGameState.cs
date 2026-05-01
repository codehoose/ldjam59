namespace HackThePlanet.FSM
{
    using Gwydion.Core.FSM;
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
