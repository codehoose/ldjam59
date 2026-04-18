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
    }
}
