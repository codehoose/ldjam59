using HackThePlanet.Models;

namespace HackThePlanet.Commands.Gameplay
{
    internal class KillUnitCommand : BaseCommand
    {
        private readonly IUnit _attacking;
        private readonly IUnit _defending;

        public KillUnitCommand(IUnit attacking, IUnit defending)
        {
            _attacking = attacking;
            _defending = defending;
        }

        public override void Execute()
        {
            _attacking.HasActed = true;
            GameState.Instance.KillProcess(_defending);
        }

        public override void Undo()
        {
            _attacking.HasActed = false;
            var (x, y) = GameState.Instance.GetUnitGridPosition(_defending);
            GameState.Instance.MoveUnit(_defending, x, y);

        }
    }
}
