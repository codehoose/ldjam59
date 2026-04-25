using Microsoft.Xna.Framework;

namespace HackThePlanet.Components
{
    internal abstract class HtpBaseDrawableComponent : DrawableGameComponent, IHtpComponent
    {
        internal float SortOrder { get; set; }

        internal Vector2 Position { get; set; } = Vector2.Zero;

        internal Color Color { get; set; } = Color.White;

        public HackThePlanetGame HtpGame { get; }

        public HtpBaseDrawableComponent(HackThePlanetGame game, float sortOrder = 1f) : base(game)
        {
            HtpGame = game;
            SortOrder = sortOrder;
        }
    }
}
