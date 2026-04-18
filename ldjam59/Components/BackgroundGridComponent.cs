using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ldjam59.Components
{
    internal class BackgroundGridComponent : SKDrawableGameComponent<LDJamGame>
    {
        Texture2D _texture;
        public BackgroundGridComponent(LDJamGame game) : base(game)
        {
            _texture = new Texture2D(game.GraphicsDevice, 1, 1);
            _texture.SetData(new Color[] { Color.White });
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            for (var y = 0; y < 11; y++)
            {
                var doPlum = y == 0 || y % 2 == 0;
                for (var x = 0; x < 20; x++)
                {
                    var color = (x + y) % 2 == 1 ? Palette.Dark : Palette.DarkPlum;
                    TheGame.SpriteBatch.Draw(_texture, new Rectangle(x * 64, y * 64, 64, 64), null, color, 0f, Vector2.Zero, SpriteEffects.None, 0);
                }
            }
        }
    }
}
