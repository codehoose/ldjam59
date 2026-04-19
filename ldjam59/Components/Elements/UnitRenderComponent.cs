using HackThePlanet.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HackThePlanet.Components.Elements
{
    internal class UnitRenderComponent : HtpDrawableComponent
    {
        private IUnit _unit;
        private SpriteSheet _spriteSheet;
        private Texture2D _ghostTexture;
        private int _lastIndex = -1;

        public bool IsGhost { get; set; }
        public IUnit Unit => _unit;

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

        public UnitRenderComponent(HackThePlanetGame game, IUnit unit, Texture2D texture, bool isWhiteHat) : base(game, texture, Layer.Units)
        {
            _spriteSheet = new SpriteSheet(54, 54, texture.Bounds);
            _unit = unit;
            _ghostTexture = game.Ghost;

            // Now let's figure out what type of unit we have
            if (_unit is Agent) return; // bail early.. Don't care.
            var u = _unit as Unit;

            var droneStart = isWhiteHat ? 0 : 54;
            var crawlerStart = isWhiteHat ? 108 : 108 + 54;
            var start = u.Type == UnitType.Crawler ? crawlerStart : droneStart;

            SrcRect = new Rectangle(start, 0, 54, 54);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            if (IsGhost)
            {
                HtpGame.SpriteBatch.Draw(_ghostTexture, Position, SrcRect, Color, 0f,
                    Vector2.Zero, Scale, SpriteEffects.None, Layer.UnitIsGhost);
            }
        }
    }
}
