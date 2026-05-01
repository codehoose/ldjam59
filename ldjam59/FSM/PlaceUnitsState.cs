namespace HackThePlanet.FSM
{
    using Gwydion.Core.FSM;
    using HackThePlanet;
    using HackThePlanet.Commands;
    using HackThePlanet.Commands.Gameplay;
    using HackThePlanet.Components;
    using HackThePlanet.Components.Elements;
    using HackThePlanet.Input;
    using HackThePlanet.Models;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using System.Collections.Generic;

    internal class PlaceUnitsState : MainLoopGameState<PlaceUnitsState>
    {
        enum SummonPhase
        {
            None,
            SelectUnitType,
            PlaceUnitType
        }

        private List<UnitRenderComponent> _placedUnits = [];
        private DeploymentMenuComponent _deploymentMenu;
        private MouseClickState _mouse;
        private SummonPhase _phase = SummonPhase.None;
        private HtpDrawableComponent _cursor;
        private UnitType _deployUnit;
        private bool _deployUnitIsGhost;

        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);

            if (_phase != SummonPhase.PlaceUnitType) return;

            _mouse.Tick(deltaTime);

            var normalizedPos = _mouse.Position / 54; // each square is 54 pixels!!
            var x = (int)normalizedPos.X;
            var y = (int)normalizedPos.Y;

            var acceptablePositions = GameState.Instance.GetFreeSquaresAround(GameState.Instance.CurrentPlayer.Agent);
            var index = GameState.Instance.GetTileIndex(x, y);
            var validCellAndHasCycles = acceptablePositions.Contains(index) && GameState.Instance.Cycles > 0;
            _cursor.Enabled = _mouse.Contained;
            _cursor.Color = (validCellAndHasCycles ? Color.Green : Color.Red) * .5f;
            _cursor.Position = new Vector2(x, y) * 54;
        }

        public override void Enter(IStateManager stateManager)
        {
            base.Enter(stateManager);
            _phase = SummonPhase.SelectUnitType;

            _mouse = new MouseClickState(new Rectangle(0, 0, 540, 540));
            _mouse.Click += (_, e) => DoClick(e);

            if (_deploymentMenu == null)
            {
                _deploymentMenu = new DeploymentMenuComponent((HackThePlanetGame)stateManager.Game, Content.Load<Texture2D>("button"));
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

            _deploymentMenu.OnClick += Menu_Click;

            AddComponent(_deploymentMenu);
            AddComponent(_cursor);
        }

        public override void Exit(IStateManager stateManager)
        {
            _deploymentMenu.OnClick -= Menu_Click;

            foreach (var unit in _placedUnits)
            {
                RemoveComponent(unit);
            }

            stateManager.Game.Components.Remove(_cursor);
            base.Exit(stateManager);
        }

        private void Menu_Click(object sender, MenuChoiceEventArgs e)
        {
            switch(e.MenuChoice)
            {
                case MenuChoice.DeployDrone:
                    _deployUnit = UnitType.Drone;
                    _deployUnitIsGhost = false;
                    _phase = SummonPhase.PlaceUnitType;
                    break;
                case MenuChoice.DeployCrawler:
                    _deployUnit = UnitType.Crawler;
                    _deployUnitIsGhost = false;
                    _phase = SummonPhase.PlaceUnitType;
                    break;
                case MenuChoice.DeployDroneGhost:
                    _deployUnit = UnitType.Drone;
                    _deployUnitIsGhost = true;
                    _phase = SummonPhase.PlaceUnitType;
                    break;
                case MenuChoice.DeployCrawlerGhost:
                    _deployUnit = UnitType.Crawler;
                    _deployUnitIsGhost = true;
                    _phase = SummonPhase.PlaceUnitType;
                    break;
                case MenuChoice.EndSequence:
                    StateManager.ChangeState(MoveUnitsState.Instance);
                    break;
            }
        }

        private void DoClick(Vector2 pos)
        {
            var gridPos = pos / 54;
            var x = (int)gridPos.X;
            var y = (int)gridPos.Y;

            if (GameState.Instance.IsOccupied(x, y) || GameState.Instance.Cycles == 0)
            {
                return;
            }

            CommandStack.Instance.Execute(new AddUnitToBoardCommand(x, y, _deployUnitIsGhost, _deployUnit));
            var index = GameState.Instance.GetTileIndex(x, y);
            var newUnit = new UnitRenderComponent((HackThePlanetGame)StateManager.Game, new Unit(null, _deployUnit, _deployUnitIsGhost, index), HackThePlanetGame.Instance.Units, GameState.Instance.CurrentPlayerIndex == 0);
            AddComponent(newUnit);
            _placedUnits.Add(newUnit);
        }
    }
}
