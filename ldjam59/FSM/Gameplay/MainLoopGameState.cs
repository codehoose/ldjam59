using HackThePlanet.Components;
using HackThePlanet.Components.Elements;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace HackThePlanet.FSM.Gameplay
{
    /// <summary>
    /// Base class for all main loop base states, this will by default draw the pieces
    /// on the board, the main board etc.
    /// </summary>
    internal class MainLoopGameState : BaseState<MainLoopGameState>
    {
        private BackgroundGridComponent _grid;
        private List<AgentRenderComponent> _agents = [];

        public override void Enter(StateManager stateManager)
        {
            base.Enter(stateManager);

            if (_grid == null)
            {
                SetupComponents(stateManager);
            }

            AddComponent(_grid);
            foreach (var agent in _agents)
            {
                agent.Init();
                AddComponent(agent);
            }
        }

        private void SetupComponents(StateManager stateManager)
        {
            _grid = new BackgroundGridComponent(Game, Content.Load<Texture2D>("block"));
            
            var textures = new [] { Game.WhiteHat, Game.BlackHat};

            var index = 0;
            foreach (var a in Game.State.GetAgents())
            {
                var agentComponent = new AgentRenderComponent(Game, a, textures[index++]);
                _agents.Add(agentComponent);
            }
        }
    }
}
