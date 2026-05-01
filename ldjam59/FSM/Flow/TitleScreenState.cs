namespace HackThePlanet.FSM.Flow
{
    using Gwydion.Core.FSM;
    using HackThePlanet;
    using HackThePlanet.Components;
    using HackThePlanet.Components.Elements;
    using HackThePlanet.FSM;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using System;

    internal class TitleScreenState : BaseState<TitleScreenState>
    {
        private HtpDrawableComponent _background;
        private ButtonComponent _playGame;
        private ButtonComponent _instructions;

        public override void Enter(IStateManager stateManager)
        {
            base.Enter(stateManager);

            if (_background == null)
            {
                var pos = new Vector2(600, 350);

                _background = new HtpDrawableComponent((HackThePlanetGame)stateManager.Game, Content.Load<Texture2D>("hackerman-title"), Layer.Background)
                {
                    Scale = 2
                };
                _playGame = new ButtonComponent((HackThePlanetGame)stateManager.Game, Content.Load<Texture2D>("button"), "Play Game", 256, 4)
                {
                    Position = pos
                };
                _playGame.OnClick += PlayGame_Clicked;


                pos += new Vector2(0, 40);
                _instructions = new ButtonComponent((HackThePlanetGame)stateManager.Game, Content.Load<Texture2D>("button"), "Instructions", 256, 4)
                {
                    Position = pos,
                    Disabled = true
                };
                _instructions.OnClick += PlayGame_Clicked;
            }

            AddComponent(_background);
            AddComponent(_playGame);
            AddComponent(_instructions);
        }

        public override void Exit(IStateManager stateManager)
        {
            base.Exit(stateManager);
            _instructions.OnClick -= Instructions_Clicked;
            _playGame.OnClick -= PlayGame_Clicked;
        }

        private void Instructions_Clicked(object sender, EventArgs e)
        {
            StateManager.ChangeState(InstructionsState.Instance);
        }

        private void PlayGame_Clicked(object sender, EventArgs e)
        {
            StateManager.ChangeState(InitializeGameState.Instance);
        }
    }
}
