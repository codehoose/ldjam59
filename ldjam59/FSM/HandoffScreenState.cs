namespace HackThePlanet.FSM
{
    using Gwydion.Core.FSM;
    using HackThePlanet;
    using HackThePlanet.Components;
    using HackThePlanet.Components.Elements;
    using HackThePlanet.Models;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using System;

    internal class HandoffScreenState : BaseState<HandoffScreenState>
    {
        private ButtonComponent _button;
        private HtpDrawableComponent _hackerman;

        public override void Enter(IStateManager stateManager)
        {
            base.Enter(stateManager);

            if (_button == null)
            {
                var pos = new Vector2(stateManager.Game.GraphicsDevice.Viewport.Width - 256, stateManager.Game.GraphicsDevice.Viewport.Height - 32) / 2f;
                _button = new ButtonComponent((HackThePlanetGame)stateManager.Game,
                    Content.Load<Texture2D>("button"),
                    "",
                    256,
                    4)
                {
                    Position = pos
                };

                _hackerman = new HtpDrawableComponent((HackThePlanetGame)stateManager.Game, HackThePlanetGame.Instance.Hackerman, Layer.Background);
            }

            _button.OnClick += Button_Click;
            _hackerman.Color = GameState.Instance.CurrentPlayerIndex == 0 ? Color.White : Color.Orange;
            _hackerman.Effects = GameState.Instance.CurrentPlayerIndex != 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            _hackerman.Scale = 2;

            _button.Text = $"Your turn P<{GameState.Instance.CurrentPlayerIndex}> {GameState.Instance.CurrentPlayer.Name}";
            AddComponent(_hackerman);
            AddComponent(_button);
        }

        public override void Exit(IStateManager stateManager)
        {
            _button.OnClick -= Button_Click;

            base.Exit(stateManager);
        }

        private void Button_Click(object sender, EventArgs e)
        {
            StateManager.ChangeState(PlaceUnitsState.Instance);
        }
    }
}
