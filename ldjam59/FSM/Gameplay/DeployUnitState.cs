using HackThePlanet.Commands;
using HackThePlanet.Commands.Gameplay;
using HackThePlanet.Components;
using HackThePlanet.Components.Elements;
using HackThePlanet.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace HackThePlanet.FSM.Gameplay
{
    internal class DeployUnitState : MainLoopGameState<DeployUnitState>
    {
        private MouseClickState _mouse;
        private ButtonComponent _cancel;
        private HighlightCursorComponent _cursor;

        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);
            _mouse.Tick(deltaTime);

            var normalizedPos = _mouse.Position / 54; // each square is 54 pixels!!
            var x = (int)normalizedPos.X;
            var y = (int)normalizedPos.Y;

            var acceptablePositions = GameState.GetFreeSquaresAround(GameState.CurrentPlayer.Agent);
            var index = GameState.GetTileIndex(x, y);
            _cursor.Enabled = _mouse.Contained;
            _cursor.Color = (acceptablePositions.Contains(index) ? Color.Green : Color.Red) * .5f;
            _cursor.Position = new Vector2(x, y) * 54;
        }

        public override void Enter(StateManager stateManager)
        {
            base.Enter(stateManager);

            _mouse = new MouseClickState(new Rectangle(0, 0, 540, 540));
            _mouse.Click += (_, e) => DoClick(e);

            if (_cancel == null)
            {
                var pos = new Vector2(750 - 380 / 2, 400);
                _cancel = new ButtonComponent(Game, Content.Load<Texture2D>("button"), "Cancel", 380, 4)
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

        private void DoClick(Vector2 pos)
        {
            var gridPos = pos / 54;
            var x = (int)gridPos.X;
            var y = (int)gridPos.Y;

            if (GameState.IsOccupied(x, y))
            {
                return;
            }

            // TODO: Sound effects!
            CommandStack.Instance.Execute(new AddUnitToBoardCommand(x, y, GameState.UnitToDeployIsGhost, GameState.UnitToDeploy));
            StateManager.ChangeState(SummonState.Instance);
        }
    }
}
