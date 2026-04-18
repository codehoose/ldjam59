using System.Collections.Generic;
using System.Linq;

namespace HackThePlanet.Models
{
    internal class GameState
    {
        internal static int DEFAULT_CYCLES = 4;

        private readonly Board _board = new Board();
        private readonly Player[] _players = new Player[2];
        private Player _currentPlayer;
        private Player _winner;
        private int _current;
        private int _cycles;

        public Player CurrentPlayer => _currentPlayer;
        public int CurrentPlayerIndex => _current;
        public Player Winner => _winner;
        public int Cycles => _cycles;
        public UnitType UnitToDeploy { get; set; } = UnitType.None;

        private IUnit[] Units => _board.GetAllUnits();

        public IEnumerable<Agent> GetAgents() => _players.Select(p => p.Agent);

        public (int, int) GetAgentGridPosition(Agent agent) => _board.GetAgentGridPosition(agent);
        public List<int> GetFreeSquaresAround(Agent agent) => _board.GetFreeSquaresAround(agent);
        public int GetTileIndex(int x, int y) => _board.GetTileIndex(x, y);

        public void Init()
        {
            _players[0] = _board.AddPlayer("Abercromby", 1, 1);
            _players[1] = _board.AddPlayer("Bertram", 8, 8);
            _currentPlayer = _players[_current];
            _cycles = DEFAULT_CYCLES;
        }

        public EndTurnCondition EndCurrentPlayerTurn()
        {
            var result = EndTurnCondition.GameOn;

            var opponent = (_current + 1) % 2;
            if (!_players[opponent].Agent.IsAlive)
            {
                _winner = _players[_current];
                result = EndTurnCondition.OpponentDead;
            }
            else
            {
                _current = (_current + 1) % 2;
                _currentPlayer = _players[_current];
                _cycles = DEFAULT_CYCLES;
            }

            return result;
        }
    }
}
