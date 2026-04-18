using HackThePlanet.Components.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Windows.Forms;

namespace HackThePlanet.FSM.Gameplay
{
    internal class HandoffScreenState : BaseState<HandoffScreenState>
    {
        private ButtonComponent _button;

        public override void Enter(StateManager stateManager)
        {
            base.Enter(stateManager);


            var pos = new Vector2(Game.GraphicsDevice.Viewport.Width - 256, Game.GraphicsDevice.Viewport.Height - 32) / 2f;

            if (_button == null)
            {
                _button = new ButtonComponent(Game,
                    Content.Load<Texture2D>("button"),
                    $"Your turn P<{stateManager.Game.State.CurrentPlayerIndex}> {stateManager.Game.State.CurrentPlayer.Name}",
                    256,
                    4)
                {
                    Position = pos
                };

                _button.OnClick += Button_Click;

            }

            AddComponent(_button);
        }

        private void Button_Click(object sender, EventArgs e)
        {
            StateManager.ChangeState(MoveAttackState.Instance);
        }
    }
}
