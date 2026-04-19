using HackThePlanet.Components;
using HackThePlanet.Components.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct2D1.Effects;
using System;

namespace HackThePlanet.FSM.Gameplay
{
    internal class HandoffScreenState : BaseState<HandoffScreenState>
    {
        private ButtonComponent _button;
        private HtpDrawableComponent _hackerman;

        public override void Enter(StateManager stateManager)
        {
            base.Enter(stateManager);

            if (_button == null)
            {
                var pos = new Vector2(Game.GraphicsDevice.Viewport.Width - 256, Game.GraphicsDevice.Viewport.Height - 32) / 2f;
                _button = new ButtonComponent(Game,
                    Content.Load<Texture2D>("button"),
                    "",
                    256,
                    4)
                {
                    Position = pos
                };

                _button.OnClick += Button_Click;

                _hackerman = new HtpDrawableComponent(Game, Game.Hackerman, Layer.Background);
            }

            _hackerman.Color = Game.State.CurrentPlayerIndex == 0 ? Color.White : Color.Orange;
            _hackerman.Effects = Game.State.CurrentPlayerIndex != 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            _hackerman.Scale = 2;

            _button.Text = $"Your turn P<{stateManager.Game.State.CurrentPlayerIndex}> {stateManager.Game.State.CurrentPlayer.Name}";
            AddComponent(_hackerman);
            AddComponent(_button);
        }

        private void Button_Click(object sender, EventArgs e)
        {
            StateManager.ChangeState(SummonState.Instance);
        }
    }
}
