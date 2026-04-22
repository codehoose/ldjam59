using HackThePlanet.Models;

namespace HackThePlanet.Commands.Gameplay
{
    internal class AddUnitToBoardCommand : BaseCommand
    {
        private int _x;
        private int _y;
        private bool _isGhost;
        private UnitType _type;
        private IUnit _createdUnit;

        public AddUnitToBoardCommand(int x, int y, bool isGhost, UnitType type)
        {
            _x = x;
            _y = y;
            _isGhost = isGhost;
            _type = type;
        }

        public override void Execute()
        {
            _createdUnit = GameState.Instance.DeployUnit(_x, _y, _isGhost, _type);
        }

        public override void Undo()
        {
            GameState.Instance.RemoveUnit(_createdUnit);
        }
    }
}
