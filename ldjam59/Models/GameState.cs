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

        private IUnit[] Units => _board.GetAllUnits();

        public IEnumerable<Agent> GetAgents() => _players.Select(p => p.Agent);

        public (int, int) GetAgentGridPosition(Agent agent)
        {
            var x = agent.TileIndex % 10;
            var y = agent.TileIndex / 10;
            return (x, y);
        }

        public int GetTileIndex(int x, int y)
        {
            var cx = Math.Clamp(x, 0, 9);
            var cy = Math.Clamp(y, 0, 9);

            return cy * 10 + cx;
        }

        public void Init()
        {
            _players[0] = new Player(GetTileIndex(1, 1)) { Name = "Abercromby" };
            _players[1] = new Player(GetTileIndex(8, 8)) { Name = "Bertram" };
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
