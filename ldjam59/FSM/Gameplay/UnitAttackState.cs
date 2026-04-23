namespace HackThePlanet.FSM.Gameplay
{
    using HackThePlanet.Commands;
    using HackThePlanet.Commands.Gameplay;
    using HackThePlanet.Components;
    using HackThePlanet.Components.Elements;
    using HackThePlanet.FSM.Gameplay.Flow;
    using HackThePlanet.Input;
    using HackThePlanet.Models;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using System;
    using System.Collections.Generic;

    internal class UnitAttackState : MainLoopGameState<UnitAttackState>
    {
        enum AttackState
        {
            Select,
            Attack
        }

        private HighlightCursorComponent _cursor;
        private MouseClickState _mouse;
        private CursorComponent _selection;
        private ButtonState _previousButtonState;
        private ButtonComponent _endAttack;
        private bool _pressedInside;
        private AttackState state;
        private List<IUnit> _units;
        private IUnit _selectedUnit;
        private int _x;
        private int _y;
        private IUnit _attackingUnit;

        public override void Enter(StateManager stateManager)
        {
            base.Enter(stateManager);

            ResetHasBeenUsedFlag();
            _units = Game.State.GetPlayerUnits();
            foreach (var unit in _units) unit.HasActed = false;

            state = AttackState.Select;

            if (_mouse == null)
            {
                _mouse = new MouseClickState(new Rectangle(0, 0, 540, 540));
            }

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
            _mouse.Click += (s, e) => DoClick((int)(e.X / 54f), (int)(e.Y / 54f));
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
            _mouse.Tick(deltaTime);
            _cursor.Enabled = _mouse.Contained;
            _cursor.Color = Color.Green * .5f;

            var normalizedPos = _mouse.Position / 54; // each square is 54 pixels!!
            var x = (int)normalizedPos.X;
            var y = (int)normalizedPos.Y;

            if (_cursor.Enabled && state == AttackState.Attack && _selectedUnit != null)
            {
                var acceptablePositions = GameState.GetAttackTargetsAround(_selectedUnit);
                var index = GameState.GetTileIndex(x, y);
                _cursor.Enabled = true;
                _cursor.Color = (acceptablePositions.Contains(index) ? Color.Yellow : Color.Transparent) * .5f;
            }

            _cursor.Position = new Vector2(x, y) * 54;
        }

        private void DoClick(int x, int y)
        {
            switch (state)
            {
                case AttackState.Select:
                    _attackingUnit = Game.State.GetUnitAt(x, y);
                    if (_attackingUnit == null || _attackingUnit.IsGhost) return; // Ghosts can't attack
                    if (_units.Contains(_attackingUnit) && !_attackingUnit.HasActed)
                    {
                        // Set up selection cursor
                        _selection.Enabled = true;
                        _selectedUnit = _attackingUnit;
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
                        _attackingUnit = defender;
                        _selection.Position = new Vector2(x, y) * 54f;
                    }
                    else
                    {
                        var killUnitCommand = new KillProcessCommand(_attackingUnit, _selectedUnit);
                        CommandStack.Instance.Execute(killUnitCommand);
                        Game.State.KillProcess(defender);
                        RemoveUnit(defender); // Remove from renderer
                        _selection.Enabled = false;
                        SetHasBeenUsed(_selectedUnit);
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
