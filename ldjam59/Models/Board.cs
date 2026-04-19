using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HackThePlanet.Models
{
    internal class Board
    {
        public static int WIDTH = 10;
        public static int HEIGHT = 10;

        public readonly IUnit[] _board = new IUnit[WIDTH * HEIGHT];

        /// <summary>
        /// Gets all the basic units, does not return Agents. 
        /// </summary>
        /// <returns></returns>
        public IUnit[] GetAllUnits() => _board.Where(b => b != null && b is not Agent).ToArray();

        public bool IsOccupied(int index) => _board[index] != null;

        public bool IsOccupied(int x, int y) => _board[GetTileIndex(x, y)] != null;

        public IUnit GetUnitAt(int x, int y) => _board[GetTileIndex(x, y)];

        internal void MoveUnit(IUnit unit, int x, int y)
        {
            _board[unit.TileIndex] = null;
            unit.TileIndex = GetTileIndex(x, y);
            _board[unit.TileIndex] = unit;
        }

        public Player AddPlayer(string playerName, int x, int y)
        {
            var index = GetTileIndex(x, y);
            var player = new Player(index) { Name = playerName };
            _board[index] = player.Agent;
            return player;
        }

        public Unit AddUnit(Player parent, UnitType type, int x, int y, bool isIllusion)
        {
            var acceptableTiles = GetFreeSquaresAround(parent.Agent);
            var index = GetTileIndex(x, y);
            if (!acceptableTiles.Contains(index)) return null;

            var unit = new Unit(parent.Agent, type, isIllusion, index);
            _board[index] = unit;
            return unit;
        }

        public List<int> GetFreeSquaresAround(IUnit agent)
        {
            var list = new List<int>();
            var (ax, ay) = GetAgentGridPosition(agent);

            for (var y = ay - 1; y <= ay + 1; y++)
            {
                for (var x = ax - 1; x <= ax + 1; x++)
                {
                    var index = GetTileIndex(x, y);
                    if (!IsOccupied(index))
                    {
                        list.Add(index);
                    }
                }
            }

            return list;
        }

        public (int, int) GetAgentGridPosition(IUnit agent)
        {
            var x = agent.TileIndex % 10;
            var y = agent.TileIndex / 10;
            return (x, y);
        }

        public int GetTileIndex(int x, int y)
        {
            var cx = Math.Clamp(x, 0, WIDTH - 1);
            var cy = Math.Clamp(y, 0, HEIGHT - 1);

            return cy * WIDTH + cx;
        }

        internal void RemoveUnit(IUnit unit)
        {
            _board[unit.TileIndex] = null;
        }

        internal List<int> GetAttackTargetsAround(IUnit unit)
        {
            // Couple of things here.
            // Drones can see 2 squares around them, BUT! They can only fire if they have line of sight
            // AI can't attack AI

            // Let's get the list of units first and then we can cull them...
            var sight = unit.Type == UnitType.Drone ? 2 : 1;

            // Are we another AI?
            var isAi = unit is Agent;

            var list = new List<int>();
            var (ax, ay) = GetAgentGridPosition(unit);

            for (var y = ay - sight; y <= ay + sight; y++)
            {
                for (var x = ax - sight; x <= ax + sight; x++)
                {
                    var index = GetTileIndex(x, y);
                    if (IsOccupied(index) && unit.TileIndex != index)
                    {
                        if (_board[index] is Unit || (_board[index] is Agent && !isAi))
                        {
                            list.Add(index);
                        }
                    }
                }
            }

            return list;
        }

        internal List<int> GetOpponentTargets(Agent agent) =>
            _board.Where(u => u is not null and not Agent)
                  .Select(u => (Unit)u)
                  .Where(u => u.Agent != agent)
                  .Select(u => u.TileIndex)
                  .ToList();
    }
}
