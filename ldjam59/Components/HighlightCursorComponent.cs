using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HackThePlanet.Components
{
    internal class HighlightCursorComponent : HtpDrawableComponent
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public HighlightCursorComponent(HackThePlanetGame game, Texture2D texture, float sortOrder) : base(game, texture, sortOrder)
        {
        }

        public override void Draw(GameTime gameTime)
        {
            if (Texture == null || !Enabled) return;

            HtpGame.SpriteBatch.Draw(Texture, new Rectangle((int)Position.X, (int)Position.Y, Width, Height), null, Color, 0f, Vector2.Zero, SpriteEffects.None, SortOrder);
        }
    }
}
