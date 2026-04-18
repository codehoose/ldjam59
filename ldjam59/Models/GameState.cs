using System;
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
        public bool UnitToDeployIsGhost { get; set; }

        private IUnit[] Units => _board.GetAllUnits();

        public IEnumerable<Agent> GetAgents() => _players.Select(p => p.Agent);
        public IEnumerable<IUnit> GetUnits() => _board.GetAllUnits();

        public (int, int) GetAgentGridPosition(IUnit agent) => _board.GetAgentGridPosition(agent);
        public List<int> GetFreeSquaresAround(IUnit agent) => _board.GetFreeSquaresAround(agent);
        public int GetTileIndex(int x, int y) => _board.GetTileIndex(x, y);
        public bool IsOccupied(int x, int y) => _board.IsOccupied(x, y);

        public void Init()
        {
            _players[0] = _board.AddPlayer("Abercromby", 1, 1);
            _players[1] = _board.AddPlayer("Bertram", 8, 8);
            _currentPlayer = _players[_current];
            _cycles = DEFAULT_CYCLES;
        }


        internal void DeployUnit(int x, int y)
        {
            if (UnitToDeploy == UnitType.None)
            {
                return;
            }

            _cycles -= (UnitToDeploy == UnitType.Crawler ? 1 : 2);

            _ = _board.AddUnit(_currentPlayer, UnitToDeploy, x, y, UnitToDeployIsGhost);
            UnitToDeploy = UnitType.None;
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
