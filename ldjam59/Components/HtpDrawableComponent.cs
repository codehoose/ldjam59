using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HackThePlanet.Components
{
    /// <summary>
    /// Hack the Planet base drawable component
    /// </summary>
    internal class HtpDrawableComponent : HtpBaseDrawableComponent
    {
        private int _width;
        private int _height;

        internal Texture2D Texture { get; }

        internal float Scale { get; set; } = 1f;

        internal Rectangle SrcRect { get; set; }

        internal SpriteEffects Effects { get; set; } = SpriteEffects.None;

        public int Width 
        {
            get => _width;
            set
            {
                _height = value;
                SrcRect = new Rectangle(0, 0, _width, _height);
            }
        }

        public int Height
        {
            get => _height;
            set
            {
                _height = value;
                SrcRect = new Rectangle(0, 0, _width, _height);
            }
        }

        public HtpDrawableComponent(HackThePlanetGame game, Texture2D texture, float sortOrder) : base(game, sortOrder)
        {
            Texture = texture;
            _width = Texture.Bounds.Width;
            _height = Texture.Bounds.Height;
            SrcRect = Texture != null ? Texture.Bounds : Rectangle.Empty;
            SortOrder = sortOrder;
        }

        public override void Draw(GameTime gameTime)
        {
            if (Texture == null || !Enabled) return;

            HtpGame.SpriteBatch.Draw(Texture, Position, SrcRect, Color, 0f,
                Vector2.Zero, Scale, Effects, SortOrder);
        }
    }
}
