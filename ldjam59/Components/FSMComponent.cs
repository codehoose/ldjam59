using HackThePlanet.FSM;
using Microsoft.Xna.Framework;

namespace HackThePlanet.Components
{
    internal class FSMComponent : HtpComponent
    {
        public StateManager StateManager { get; }

        public FSMComponent(HackThePlanetGame game) : base(game)
        {
            StateManager = new StateManager(game);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            float deltaTime = (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 1000f);
            StateManager.Tick(deltaTime);
        }
    }
}
