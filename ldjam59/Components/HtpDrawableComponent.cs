using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HackThePlanet.Components
{
    /// <summary>
    /// Hack the Planet base drawable component
    /// </summary>
    internal class HtpDrawableComponent : DrawableGameComponent, IHtpComponent
    {
        internal HackThePlanetGame HtpGame { get; }

        internal Texture2D Texture { get; }

        internal float SortOrder { get; set; }

        internal Vector2 Position { get; set; } = Vector2.Zero;

        internal Color Color { get; set; } = Color.White;

        internal float Scale { get; set; } = 1f;

        internal Rectangle SrcRect { get; set; }

        HackThePlanetGame IHtpComponent.HtpGame => HtpGame;

        public HtpDrawableComponent(HackThePlanetGame game, Texture2D texture, float sortOrder) : base(game)
        {
            Texture = texture;
            HtpGame = game;
            SrcRect = Texture != null ? Texture.Bounds : Rectangle.Empty;
            SortOrder = sortOrder;
        }

        public override void Draw(GameTime gameTime)
        {
            if (Texture == null || !Enabled) return;

            HtpGame.SpriteBatch.Draw(Texture, Position, SrcRect, Color, 0f,
                Vector2.Zero, Scale, SpriteEffects.None, SortOrder);
        }
    }
}
