using HackThePlanet.Components;
using HackThePlanet.Components.Elements;
using HackThePlanet.Models;
using Microsoft.Xna.Framework;
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
        private HtpDrawableComponent _hackerman;
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
                var isWhiteHat = GameState.GetIsWhiteHat(u);
                var unit = new UnitRenderComponent(Game, u, Game.Units, isWhiteHat)
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
                var agentComponent = new UnitRenderComponent(Game, a, textures[index++], false);
                _agents.Add(agentComponent);
            }

            _hackerman.Color = Game.State.CurrentPlayerIndex == 0 ? Color.White : Color.Orange;
            _hackerman.Effects = Game.State.CurrentPlayerIndex != 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            _hackerman.Scale = 2;

            //AddComponent(_grid);
            AddComponent(_gameState);
            AddComponent(_hackerman);
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

        protected void SetHasBeenUsed(IUnit unit)
        {
            var index = _units.FindIndex(u => u.Unit == unit);
            if (index >= 0)
            {
                var unitComponent = _units[index];
                unitComponent.HasBeenUsed = true;
            }
            else
            {
                index = _agents.FindIndex(u => u.Unit == unit);
                if (index >= 0)
                {
                    _agents[index].HasBeenUsed = true;
                }
            }
        }

        protected void ResetHasBeenUsedFlag()
        {
            foreach (var unit in _units)
            {
                unit.HasBeenUsed = false;
            }

            foreach (var agent in _agents)
            {
                agent.HasBeenUsed = false;
            }
        }

        private void SetupComponents(StateManager stateManager)
        {
            _grid = new BackgroundGridComponent(Game, Content.Load<Texture2D>("block"));
            _gameState = new GameStateComponent(Game);
            _hackerman = new HtpDrawableComponent(Game, Game.HackermanSide, Layer.Background)
            {
                Position = new Vector2(540, 0)
            };
        }
    }
}
