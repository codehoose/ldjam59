using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HackThePlanet.Components
{
    internal class BackgroundGridComponent : HtpDrawableComponent
    {
        private readonly Texture2D _block;

        public BackgroundGridComponent(HackThePlanetGame game, Texture2D block) : base(game)
        {
            _block = block;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            for (var y = 0; y < 10; y++)
            {
                for (var x = 0; x < 10; x++)
                {
                    HtpGame.SpriteBatch.Draw(_block, new Rectangle(x * 54, y * 54, 54, 54), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, Layer.Background);
                }
            }
        }
    }
}
