using HackThePlanet.Models;

namespace HackThePlanet.Commands.Gameplay
{
    internal class MoveUnitCommand : BaseCommand
    {
        private readonly IUnit _unit;
        private readonly int _x;
        private readonly int _y;
        private readonly int _previousX;
        private readonly int _previousY;

        public MoveUnitCommand(IUnit unit, int x, int y)
        {
            (_previousX, _previousY) = GameState.Instance.GetUnitGridPosition(unit);
            _x = x;
            _y = y;
            _unit = unit;
        }

        public override void Execute()
        {
            _unit.HasActed = true;
            GameState.Instance.MoveUnit(_unit, _x, _y);
        }

        public override void Undo()
        {
            _unit.HasActed = false;
            GameState.Instance.MoveUnit(_unit, _previousX, _previousY);
        }
    }
}
