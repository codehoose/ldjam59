using HackThePlanet.Components;
using HackThePlanet.Components.Elements;
using HackThePlanet.Models;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace HackThePlanet.FSM.Gameplay
{
    /// <summary>
    /// Base class for all main loop base states, this will by default draw the pieces
    /// on the board, the main board etc.
    /// </summary>
    internal class MainLoopGameState<T> : BaseState<T> where T: IState
    {
        private BackgroundGridComponent _grid;
        private GameStateComponent _gameState;
        private List<UnitRenderComponent> _agents = [];
        private List<UnitRenderComponent> _units = [];

        public override void Enter(StateManager stateManager)
        {
            base.Enter(stateManager);

            if (_grid == null)
            {
                SetupComponents(stateManager);
            }

            var playerUnits = Game.State.GetAgentUnits();

            foreach (var u in Game.State.GetUnits())
            {
                var tex = u.Type == UnitType.Crawler ? Game.Crawler : Game.Drone;
                var unit = new UnitRenderComponent(Game, u, tex)
                {
                    IsGhost = u.IsGhost && playerUnits.Contains(u)
                };
                _units.Add(unit);
                AddComponent(unit);
            }

            var textures = new[] { Game.WhiteHat, Game.BlackHat };

            var index = 0;
            foreach (var a in Game.State.GetAgents())
            {
                var agentComponent = new UnitRenderComponent(Game, a, textures[index++]);
                _agents.Add(agentComponent);
            }

            AddComponent(_grid);
            AddComponent(_gameState);
            foreach (var agent in _agents)
            {
                agent.Init();
                AddComponent(agent);
            }
        }

        public override void Exit(StateManager stateManager)
        {
            base.Exit(stateManager);

            _units.Clear();
            _agents.Clear();
        }

        protected void RemoveUnit(IUnit unit)
        {
            var index = _units.FindIndex(u => u.Unit == unit);
            if (index >= 0)
            {
                var unitComponent = _units[index];
                RemoveComponent(unitComponent);
                _units.RemoveAt(index);
            }
            else
            {
                index = _agents.FindIndex(u => u.Unit == unit);
                if (index > 0)
                {
                    _agents.RemoveAt(index);
                }
            }
        }

        private void SetupComponents(StateManager stateManager)
        {
            _grid = new BackgroundGridComponent(Game, Content.Load<Texture2D>("block"));
            _gameState = new GameStateComponent(Game);
        }
    }
}
