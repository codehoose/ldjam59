using HackThePlanet.Components;
using HackThePlanet.Components.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace HackThePlanet.FSM.Gameplay
{
    internal class DeployUnitState : MainLoopGameState<DeployUnitState>
    {
        private ButtonComponent _cancel;
        private HighlightCursorComponent _cursor;
        private Color _color;

        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);
            var mouseState = Mouse.GetState();
            var gridPos = new Vector2(mouseState.X, mouseState.Y);
            if (!new Rectangle(0, 0, 540, 540).Contains(gridPos))
            {
                _cursor.Contained = false;
                _cursor.Enabled = false;
                return;
            }

            _cursor.Contained = true;
            var normalizedPos = gridPos / 54; // each square is 54 pixels!!
            var x = (int)normalizedPos.X;
            var y = (int)normalizedPos.Y;

            var acceptablePositions = GameState.GetFreeSquaresAround(GameState.CurrentPlayer.Agent);
            var index = GameState.GetTileIndex(x, y);
            _cursor.Enabled = true;
            _cursor.Color = (acceptablePositions.Contains(index) ? Color.Green : Color.Red) * .5f;
            _cursor.Position = new Vector2(x, y) * 54;
        }

        public override void Enter(StateManager stateManager)
        {
            base.Enter(stateManager);

            if (_cancel==null)
            {
                var pos = new Vector2(750 - 380 / 2, 400);
                _cancel = new ButtonComponent(Game, Content.Load<Texture2D>("button"), "Cancel Deploy", 380, 4)
                {
                    Position = pos
                };

                var tex = new Texture2D(Game.GraphicsDevice, 1, 1);
                tex.SetData([Color.White]);

                _cursor = new HighlightCursorComponent(Game, tex, Layer.Gui)
                {
                    Width = 54,
                    Height = 54
                };
            }

            _cancel.OnClick += Cancel_Clicked;
            AddComponent(_cursor);
            AddComponent(_cancel);
        }

        public override void Exit(StateManager stateManager)
        {
            _cancel.OnClick -= Cancel_Clicked;
            Game.Components.Remove(_cursor);
            Game.Components.Remove(_cancel);
            base.Exit(stateManager);
        }

        public void Cancel_Clicked(object sender, EventArgs e)
        {
            StateManager.ChangeState(SummonState.Instance);
        }
    }
}
