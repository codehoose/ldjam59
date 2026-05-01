namespace HackThePlanet.FSM
{
    using Gwydion.Core.FSM;
    using HackThePlanet.Models;

    internal class EndPlayerTurnState : BaseState<EndPlayerTurnState>
    {
        public override void Enter(IStateManager stateManager)
        {
            base.Enter(stateManager);

            var result = GameState.Instance.EndCurrentPlayerTurn();
            if (result == EndTurnCondition.GameOn)
            {
                stateManager.ChangeState(HandoffScreenState.Instance);
            }
            else
            {
                // TODO: Game Over - Player won!
            }
        }
    }
}
