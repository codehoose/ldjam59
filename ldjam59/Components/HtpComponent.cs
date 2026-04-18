using Microsoft.Xna.Framework;

namespace HackThePlanet.Components
{
    /// <summary>
    /// Hack the Planet base updateable component
    /// </summary>
    internal class HtpComponent : GameComponent, IHtpComponent
    {
        internal HackThePlanetGame HtpGame { get; }

        HackThePlanetGame IHtpComponent.HtpGame => HtpGame;

        public HtpComponent(HackThePlanetGame game) : base(game)
        {
            HtpGame = game;
        }
    }
}
