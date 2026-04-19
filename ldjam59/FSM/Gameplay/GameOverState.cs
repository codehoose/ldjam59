using HackThePlanet.Components;
using HackThePlanet.Components.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace HackThePlanet.FSM.Gameplay
{
    internal class GameOverState : BaseState<GameOverState>
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

            var (index, winningPlayer) = GameState.GetWinningPlayer();
            var firstPlayerWon = index == 0;

            _hackerman.Color = firstPlayerWon ? Color.White : Color.Orange;
            _hackerman.Effects = !firstPlayerWon ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            _hackerman.Scale = 2;

            _button.Text = $"Congrats, {stateManager.Game.State.CurrentPlayer.Name}! You Won!";
            AddComponent(_hackerman);
            AddComponent(_button);
        }

        private void Button_Click(object sender, EventArgs e)
        {
            StateManager.ChangeState(TitleScreenState.Instance);
        }
    }
}
