using Microsoft.Xna.Framework;

namespace HackThePlanet.Components
{
    internal class TextComponent : HtpBaseDrawableComponent
    {
        public string Text { get; set; }

        public TextComponent(HackThePlanetGame game) : base(game)
        {
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            if (string.IsNullOrEmpty(Text)) return;
            HtpGame.SpriteBatch.DrawString(HtpGame.Font, Text, Position, Color, 0f, Vector2.Zero, 1f, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, Layer.GuiText);
        }
    }
}
