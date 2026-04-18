using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HackThePlanet.Components
{
    internal class BackgroundGridComponent : HtpDrawableComponent
    {
        public BackgroundGridComponent(HackThePlanetGame game, Texture2D block) : base(game, block, Layer.Background)
        {

        }

        public override void Draw(GameTime gameTime)
        {
            for (var y = 0; y < 10; y++)
            {
                for (var x = 0; x < 10; x++)
                {
                    HtpGame.SpriteBatch.Draw(Texture, new Vector2(x * 54, y * 54), null, Color.White, 0f, Vector2.Zero, Scale, SpriteEffects.None, SortOrder);
                }
            }
        }
    }
}
