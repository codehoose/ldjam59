namespace HackThePlanet.Models
{
    internal class GameState
    {
        private readonly Board _board = new Board();
        private readonly Player[] _players = new Player[2];
        private Player _currentPlayer;
        private Player _winner;
        private int _mana;

        private IUnit[] Units => _board.GetAllUnits();
    }
}
