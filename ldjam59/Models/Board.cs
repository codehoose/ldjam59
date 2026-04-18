using System.Linq;

namespace HackThePlanet.Models
{
    internal class Board
    {
        public readonly IUnit[] _board = new IUnit[100];

        public IUnit[] GetAllUnits() => _board.Where(b => b != null).ToArray();
    }
}
