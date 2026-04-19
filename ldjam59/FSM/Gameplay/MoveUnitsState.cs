using HackThePlanet.Components;
using HackThePlanet.Components.Elements;
using HackThePlanet.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace HackThePlanet.FSM.Gameplay
{
    internal class MoveUnitsState : MainLoopGameState<MoveUnitsState>
    {
        enum MoveState
        {
            Select,
            Move
        }

        private HighlightCursorComponent _cursor;
        private CursorComponent _selection;
        private ButtonState _previousButtonState;
        private ButtonComponent _endMove;
        private bool _pressedInside;
        private int _x;
        private int _y;
        private MoveState state;
        private List<IUnit> _units;
        private IUnit _selectedUnit;
        private List<int> _acceptablePositions;

        public override void Enter(StateManager stateManager)
        {
            base.Enter(stateManager);

            _units = Game.State.GetPlayerUnits();
            foreach (var unit in _units) unit.HasActed = false;

            state = MoveState.Select;

            if (_endMove == null)
            {
                var pos = new Vector2(750 - 380 / 2, 400);
                _endMove = new ButtonComponent(Game, Content.Load<Texture2D>("button"), "End Move", 380, 4)
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

                _selection = new CursorComponent(Game, Game.SelectionCursor, Layer.GuiFront)
                {
                    Enabled = false
                };
            }

            _endMove.OnClick += EndMove_Clicked;
            _selection.Enabled = false;

            AddComponent(_cursor);
            AddComponent(_selection);
            AddComponent(_endMove);
        }

        public override void Exit(StateManager stateManager)
        {
            _endMove.OnClick -= EndMove_Clicked;
            base.Exit(stateManager);
        }

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
            _cursor.Color = Color.Green * .5f;
            var normalizedPos = gridPos / 54; // each square is 54 pixels!!
            var x = (int)normalizedPos.X;
            var y = (int)normalizedPos.Y;

            if (state == MoveState.Move && _selectedUnit != null)
            {
                _acceptablePositions = GameState.GetFreeSquaresAround(_selectedUnit);
                var index = GameState.GetTileIndex(x, y);
                _cursor.Enabled = true;
                _cursor.Color = (_acceptablePositions.Contains(index) ? Color.Yellow : Color.Transparent) * .5f;
            }

            _cursor.Position = new Vector2(x, y) * 54;

            var mouseContained = _cursor.Contained;
            var isPressed = mouseState.LeftButton == ButtonState.Pressed;
            var wasPressed = _previousButtonState == ButtonState.Pressed;

            if (!wasPressed && isPressed)
            {
                _pressedInside = mouseContained;
                _x = x;
                _y = y;
            }

            if (wasPressed && !isPressed)
            {
                if (_pressedInside && mouseContained)
                {
                    _previousButtonState = ButtonState.Released;
                    DoClick(_x, _y);
                }

                _pressedInside = false;
            }

            _previousButtonState = mouseState.LeftButton;

        }

        private void DoClick(int x, int y)
        {
            switch( state)
            {
                case MoveState.Select:
                    var unit = Game.State.GetUnitAt(x, y);
                    if (_units.Contains(unit) && !unit.HasActed)
                    {
                        // Set up selection cursor
                        _selection.Enabled = true;
                        _selectedUnit = unit;
                        _selection.Position = new Vector2(x, y) * 54f;
                        state = MoveState.Move;
                    }
                    break;
                case MoveState.Move:
                    var unitAlt = Game.State.GetUnitAt(x, y);
                    if (_units.Contains(unitAlt))
                    {
                        // Set up selection cursor
                        _selection.Enabled = true;
                        _selectedUnit = unitAlt;
                        _selection.Position = new Vector2(x, y) * 54f;
                    }
                    else
                    {
                        if (!_acceptablePositions.Contains(GameState.GetTileIndex(x,y)))
                        {
                            return;
                        }

                        GameState.MoveUnit(_selectedUnit, x, y);
                        _selectedUnit.HasActed = true;
                        _selectedUnit = null;
                        _selection.Enabled = false;
                        state = MoveState.Select;
                    }
                    break;
            }
        }

        private void EndMove_Clicked(object sender, EventArgs e)
        {
            StateManager.ChangeState(UnitAttackState.Instance);
        }
    }
}
