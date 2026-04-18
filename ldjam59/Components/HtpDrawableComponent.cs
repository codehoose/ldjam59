using Microsoft.Xna.Framework;

namespace HackThePlanet.Components
{
    /// <summary>
    /// Hack the Planet base drawable component
    /// </summary>
    internal class HtpDrawableComponent : DrawableGameComponent, IHtpComponent
    {
        internal HackThePlanetGame HtpGame { get; }

        HackThePlanetGame IHtpComponent.HtpGame => HtpGame;

        public HtpDrawableComponent(HackThePlanetGame game) : base(game)
        {
            HtpGame = game;
        }
    }
}
