namespace HackThePlanet.Commands.Gameplay
{
    using HackThePlanet.Models;

    internal class RemoveUnitFromBoardCommand : BaseCommand
    {
        private readonly IUnit _attacking;
        private readonly IUnit _defending;

        public RemoveUnitFromBoardCommand(IUnit attacking, IUnit defending)
        {
            _attacking = attacking;
            _defending = defending;
        }

        public override void Execute()
        {
            _attacking.HasActed = true;
            GameState.Instance.RemoveUnit(_defending);
        }

        public override void Undo()
        {
            _attacking.HasActed = false;
            var (x, y) = GameState.Instance.GetUnitGridPosition(_defending);
            GameState.Instance.MoveUnit(_defending, x, y);
        }
    }
}
