using HackThePlanet.Commands;
using HackThePlanet.Commands.Gameplay;
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

        private HtpDrawableComponent _cursor;
        private HtpDrawableComponent _selection;
        private ButtonState _previousButtonState;
        private ButtonComponent _endMove;
        private bool _pressedInside;
        private int _x;
        private int _y;
        private MoveState state;
        private List<IUnit> _units;
        private IUnit _selectedUnit;
        private List<int> _acceptablePositions;

        public override void Enter(IStateManager stateManager)
        {
            base.Enter(stateManager);

            ResetHasBeenUsedFlag();

            _units = GameState.Instance.GetPlayerUnits();
            foreach (var unit in _units) unit.HasActed = false;

            state = MoveState.Select;

            if (_endMove == null)
            {
                var pos = new Vector2(750 - 380 / 2, 400);
                _endMove = new ButtonComponent((HackThePlanetGame)stateManager.Game, Content.Load<Texture2D>("button"), "End Move", 380, 4)
                {
                    Position = pos
                };

                if (_cursor == null)
                {
                    var tex = new Texture2D(stateManager.Game.GraphicsDevice, 1, 1);
                    tex.SetData([Color.White]);

                    _cursor = new HtpDrawableComponent((HackThePlanetGame)stateManager.Game, tex, Layer.Gui)
                    {
                        Scale = 54,
                        Enabled = false
                    };
                }

                _selection = new HtpDrawableComponent((HackThePlanetGame)stateManager.Game, HackThePlanetGame.Instance.SelectionCursor, Layer.GuiFront)
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

        public override void Exit(IStateManager stateManager)
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
                _cursor.Enabled = false;
                return;
            }

            _cursor.Enabled = true;
            _cursor.Color = Color.Green * .5f;
            var normalizedPos = gridPos / 54; // each square is 54 pixels!!
            var x = (int)normalizedPos.X;
            var y = (int)normalizedPos.Y;

            if (state == MoveState.Move && _selectedUnit != null)
            {
                _acceptablePositions = GameState.Instance.GetFreeSquaresAround(_selectedUnit);
                var index = GameState.Instance.GetTileIndex(x, y);
                _cursor.Enabled = true;
                _cursor.Color = (_acceptablePositions.Contains(index) ? Color.Yellow : Color.Transparent) * .5f;
            }

            _cursor.Position = new Vector2(x, y) * 54;

            var mouseContained = _cursor.Enabled;
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
                    var unit = GameState.Instance.GetUnitAt(x, y);
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
                    var unitAlt = GameState.Instance.GetUnitAt(x, y);
                    if (_units.Contains(unitAlt))
                    {
                        // Set up selection cursor
                        _selection.Enabled = true;
                        _selectedUnit = unitAlt;
                        _selection.Position = new Vector2(x, y) * 54f;
                    }
                    else
                    {
                        if (!_acceptablePositions.Contains(GameState.Instance.GetTileIndex(x,y)))
                        {
                            return;
                        }

                        var moveUnit = new MoveUnitCommand(_selectedUnit, x, y);
                        CommandStack.Instance.Execute(moveUnit);
                        SetHasBeenUsed(_selectedUnit);
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
