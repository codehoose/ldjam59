namespace HackThePlanet.Components
{
    using Gwydion.Core.FSM;
    using Microsoft.Xna.Framework;

    internal class FSMComponent : HtpComponent
    {
        public IStateManager StateManager { get; }

        public FSMComponent(HackThePlanetGame game, IStateManager stateManager) : base(game)
        {
            StateManager = stateManager;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            float deltaTime = (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 1000f);
            StateManager.Tick(deltaTime);
        }
    }
}
