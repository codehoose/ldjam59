using HackThePlanet.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HackThePlanet.Components.Elements
{
    internal class AgentRenderComponent : HtpDrawableComponent
    {
        private Agent _agent;
        private int _lastIndex = -1;

        public void Init()
        {
            _lastIndex = -1;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (_lastIndex != _agent.TileIndex)
            {
                _lastIndex = _agent.TileIndex;
                var (x, y) = HtpGame.State.GetAgentGridPosition(_agent);
                Position = new Vector2(x * 54, y * 54);
            }
        }

        public AgentRenderComponent(HackThePlanetGame game, Agent agent, Texture2D texture) : base(game, texture, Layer.Units)
        {
            _agent = agent;
        }
    }
}
