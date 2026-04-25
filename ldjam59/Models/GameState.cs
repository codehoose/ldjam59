using System;
using System.Collections.Generic;
using System.Linq;

namespace HackThePlanet.Models
{
    internal class GameState
    {
        internal static int DEFAULT_CYCLES = 4;

        private static GameState _instance;

        private readonly Board _board = new Board();
        private readonly Player[] _players = new Player[2];
        private Player _currentPlayer;
        private Player _winner;
        private int _current;
        private int _cycles;

        public static GameState Instance => _instance;

        public Player CurrentPlayer => _currentPlayer;
        public int CurrentPlayerIndex => _current;
        public Player Winner => _winner;
        public int Cycles => _cycles;

        public GameState()
        {
            _instance = this;
        }

        public IEnumerable<Agent> GetAgents() => _players.Select(p => p.Agent);
        public IEnumerable<IUnit> GetUnits() => _board.GetAllUnits();

        public (int, int) GetUnitGridPosition(IUnit agent) => _board.GetUnitGridPosition(agent);
        public (int, int) GetGridPosition(int index) => (index % 10, index / 10);
        public List<int> GetFreeSquaresAround(IUnit agent) => _board.GetFreeSquaresAround(agent);
        public List<int> GetAttackTargetsAround(IUnit agent) => _board.GetAttackTargetsAround(agent);
        public List<int> GetOpponentTargets(Agent agent) => _board.GetOpponentTargets(agent);
        public int GetTileIndex(int x, int y) => _board.GetTileIndex(x, y);
        public bool IsOccupied(int x, int y) => _board.IsOccupied(x, y);
        internal IUnit GetUnitAt(int x, int y) => _board.GetUnitAt(x, y);
        internal void MoveUnit(IUnit unit, int x, int y) => _board.MoveUnit(unit, x, y);

        internal void KillProcess(IUnit defender)
        {
            _board.RemoveUnit(defender);
            if (defender is Agent agent) agent.IsAlive = false;
            _cycles -= 2;
        }

        public void Init()
        {
            _players[0] = _board.AddPlayer("Abercromby", 1, 1);
            _players[1] = _board.AddPlayer("Bertram", 8, 8);
            _currentPlayer = _players[_current];
            _cycles = DEFAULT_CYCLES;
        }

        internal List<IUnit> GetPlayerUnits()
        {
            var agent = _currentPlayer.Agent;
            var result = _board.GetAllUnits().Select(u => u as Unit).Where(u => u != null && u.Agent == agent)
                .Select(u => (IUnit)u).ToList();
            result.Add(agent);
            return result;
        }

        internal List<IUnit> GetAgentUnits()
        {
            var agent = _currentPlayer.Agent;
            return _board.GetAllUnits().Select(u => u as Unit).Where(u => u != null && u.Agent == agent)
                .Select(u => (IUnit)u).ToList();
        }

        internal IUnit DeployUnit(int x, int y, bool isGhost, UnitType type)
        {
            var unit = _board.AddUnit(_currentPlayer, type, x, y, isGhost);
            _cycles -= isGhost || type == UnitType.Crawler ? 1 : 2;
            return unit;
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

        internal (int, Player) GetWinningPlayer()
        {
            var player = _players[0].Agent.IsAlive ? _players[0] : _players[0];
            var index = _players[0].Agent.IsAlive ? 0 : 1;
            return (index, player);
        }

        internal bool GetIsWhiteHat(IUnit unit)
        {
            var whiteHatAgent = _players[0].Agent;
            var exists = _board.GetAllUnits()
                               .Select(u => u as Unit)
                               .Where(u => u != null && u == unit && u.Agent == whiteHatAgent)
                               .Any();
            return exists;
        }

        internal void RemoveUnit(IUnit unit, bool giveBackCycles = false)
        {
            if (giveBackCycles)
            {
                _cycles += unit.IsGhost || unit.Type == UnitType.Crawler ? 1 : 2;
            }
            _board.RemoveUnit(unit);
        }
    }
}
