using HackThePlanet.Models;

namespace HackThePlanet.FSM.Gameplay
{
    internal class EndPlayerTurnState : BaseState<EndPlayerTurnState>
    {
        public override void Enter(StateManager stateManager)
        {
            base.Enter(stateManager);

            var result = GameState.EndCurrentPlayerTurn();
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
