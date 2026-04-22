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
    internal class UnitAttackState : MainLoopGameState<UnitAttackState>
    {
        enum AttackState
        {
            Select,
            Attack
        }

        private HighlightCursorComponent _cursor;
        private CursorComponent _selection;
        private ButtonState _previousButtonState;
        private ButtonComponent _endAttack;
        private bool _pressedInside;
        private AttackState state;
        private List<IUnit> _units;
        private IUnit _selectedUnit;
        private int _x;
        private int _y;

        public override void Enter(StateManager stateManager)
        {
            base.Enter(stateManager);

            _units = Game.State.GetPlayerUnits();
            foreach (var unit in _units) unit.HasActed = false;

            state = AttackState.Select;

            if (_endAttack == null)
            {
                var pos = new Vector2(750 - 380 / 2, 400);
                _endAttack = new ButtonComponent(Game, Content.Load<Texture2D>("button"), "End Attack", 380, 4)
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

            _endAttack.OnClick += EndMove_Clicked;
            _selection.Enabled = false;

            AddComponent(_cursor);
            AddComponent(_selection);
            AddComponent(_endAttack);
        }

        public override void Exit(StateManager stateManager)
        {
            _endAttack.OnClick -= EndMove_Clicked;
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

            if (state == AttackState.Attack && _selectedUnit != null)
            {
                var acceptablePositions = GameState.GetAttackTargetsAround(_selectedUnit);
                var index = GameState.GetTileIndex(x, y);
                _cursor.Enabled = true;
                _cursor.Color = (acceptablePositions.Contains(index) ? Color.Yellow : Color.Transparent) * .5f;
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
            switch (state)
            {
                case AttackState.Select:
                    var unit = Game.State.GetUnitAt(x, y);
                    if (unit.IsGhost) return; // Ghosts can't attack
                    if (_units.Contains(unit) && !unit.HasActed)
                    {
                        // Set up selection cursor
                        _selection.Enabled = true;
                        _selectedUnit = unit;
                        _selection.Position = new Vector2(x, y) * 54f;
                        state = AttackState.Attack;
                    }
                    break;
                case AttackState.Attack:
                    var defender = Game.State.GetUnitAt(x, y);
                    if (defender == null) return;
                    if (_units.Contains(defender))
                    {
                        // It's one of ours!
                        _selection.Enabled = true;
                        _selectedUnit = defender;
                        _selection.Position = new Vector2(x, y) * 54f;
                    }
                    else
                    {
                        Game.State.KillProcess(defender);
                        RemoveUnit(defender); // Remove from renderer
                        _selection.Enabled = false;
                        _selectedUnit = null;
                        state = AttackState.Select;

                        if (defender is Agent agent)
                        {
                            if (!agent.IsAlive)
                            {

                                StateManager.ChangeState(GameOverState.Instance);
                            }
                        }
                    }
                    break;
            }
        }

        private void EndMove_Clicked(object sender, EventArgs e)
        {
            StateManager.ChangeState(RunProgramState.Instance);
        }
    }
}
