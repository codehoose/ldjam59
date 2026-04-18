using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace HackThePlanet.Components
{
    internal class WaitForMouseClickComponent : HtpComponent
    {
        public event EventHandler MouseClicked;

        public WaitForMouseClickComponent(HackThePlanetGame game) : base(game)
        {
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            var state = Mouse.GetState();
            if (state.LeftButton == ButtonState.Pressed)
            {
                MouseClicked?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
