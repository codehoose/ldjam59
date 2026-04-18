using HackThePlanet.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HackThePlanet.Components.Elements
{
    internal class UnitRenderComponent : HtpDrawableComponent
    {
        private IUnit _unit;
        private int _lastIndex = -1;

        public void Init()
        {
            _lastIndex = -1;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (_lastIndex != _unit.TileIndex)
            {
                _lastIndex = _unit.TileIndex;
                var (x, y) = HtpGame.State.GetAgentGridPosition(_unit);
                Position = new Vector2(x * 54, y * 54);
            }
        }

        public UnitRenderComponent(HackThePlanetGame game, IUnit unit, Texture2D texture) : base(game, texture, Layer.Units)
        {
            _unit = unit;
        }
    }
}
