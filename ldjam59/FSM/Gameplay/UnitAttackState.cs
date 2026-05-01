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

        private HtpDrawableComponent _cursor;
        private MouseClickState _mouse;
        private HtpDrawableComponent _selection;
        private ButtonComponent _endAttack;
        private AttackState state;
        private List<IUnit> _units;
        private IUnit _selectedUnit;
        private IUnit _attackingUnit;
        private List<int> _acceptablePositions;

        public override void Enter(IStateManager stateManager)
        {
            base.Enter(stateManager);

            ResetHasBeenUsedFlag();
            _units = GameState.Instance.GetPlayerUnits();
            foreach (var unit in _units) unit.HasActed = false;

            state = AttackState.Select;

            if (_mouse == null)
            {
                _mouse = new MouseClickState(new Rectangle(0, 0, 540, 540));
            }

            if (_endAttack == null)
            {
                var pos = new Vector2(750 - 380 / 2, 400);
                _endAttack = new ButtonComponent((HackThePlanetGame)stateManager.Game, Content.Load<Texture2D>("button"), "End Attack", 380, 4)
                {
                    Position = pos
                };

                _selection = new HtpDrawableComponent((HackThePlanetGame)stateManager.Game, HackThePlanetGame.Instance.SelectionCursor, Layer.GuiFront)
                {
                    Enabled = false
                };
            }

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

            _endAttack.OnClick += EndMove_Clicked;
            _mouse.Click += (s, e) => DoClick((int)(e.X / 54f), (int)(e.Y / 54f));
            _selection.Enabled = false;

            AddComponent(_cursor);
            AddComponent(_selection);
            AddComponent(_endAttack);
        }

        public override void Exit(IStateManager stateManager)
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
                _acceptablePositions = GameState.Instance.GetAttackTargetsAround(_selectedUnit);
                var index = GameState.Instance.GetTileIndex(x, y);
                _cursor.Enabled = true;
                _cursor.Color = (_acceptablePositions.Contains(index) ? Color.Yellow : Color.Transparent) * .5f;
            }

            _cursor.Position = new Vector2(x, y) * 54;
        }

        private void DoClick(int x, int y)
        {
            switch (state)
            {
                case AttackState.Select:
                    _attackingUnit = GameState.Instance.GetUnitAt(x, y);
                    if (_attackingUnit == null || _attackingUnit.IsGhost) return; // Ghosts can't attack
                    if (UnitIsOurs(_attackingUnit) && !_attackingUnit.HasActed)
                    {
                        // Set up selection cursor
                        _selection.Enabled = true;
                        _selectedUnit = _attackingUnit;
                        _selection.Position = new Vector2(x, y) * 54f;
                        state = AttackState.Attack;
                    }
                    break;
                case AttackState.Attack:
                    var defender = GameState.Instance.GetUnitAt(x, y);
                    if (defender == null) return;
                    if (UnitIsOurs(defender))
                    {
                        // It's one of ours!
                        _selection.Enabled = true;
                        _selectedUnit = defender;
                        _attackingUnit = defender;
                        _selection.Position = new Vector2(x, y) * 54f;
                    }
                    else
                    {
                        if (!_acceptablePositions.Contains(defender.TileIndex) || UnitIsOurs(defender))
                        {
                            // TODO: Sound effect
                            _selection.Enabled = false;
                            _selectedUnit = null;
                            state = AttackState.Select;
                            return;
                        }

                        var killUnitCommand = new KillProcessCommand(_attackingUnit, defender);
                        CommandStack.Instance.Execute(killUnitCommand);
                        GameState.Instance.KillProcess(defender);
                        RemoveUnit(defender); // Remove from renderer
                        SetHasBeenUsed(_selectedUnit);
                        _selection.Enabled = false;
                        _selectedUnit = null;
                        _attackingUnit = null;
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

        private bool UnitIsOurs(IUnit defender) => _units.Contains(defender);

        private void EndMove_Clicked(object sender, EventArgs e)
        {
            StateManager.ChangeState(RunProgramState.Instance);
        }
    }
}
