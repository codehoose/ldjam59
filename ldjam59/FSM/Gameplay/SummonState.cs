using HackThePlanet.Components;
using HackThePlanet.Components.Elements;
using HackThePlanet.Models;
using Microsoft.Xna.Framework.Graphics;

namespace HackThePlanet.FSM.Gameplay
{
    internal class SummonState : MainLoopGameState<SummonState>, IParentComponent
    {
        private DeploymentMenuComponent _deploymentMenu;

        public void RemoveComponents()
        {
            throw new System.NotImplementedException();
        }

        public void AddComponents()
        {
            throw new System.NotImplementedException();
        }

        public override void Enter(StateManager stateManager)
        {
            base.Enter(stateManager);

            if (_deploymentMenu == null)
            {
                _deploymentMenu = new DeploymentMenuComponent(Game, Content.Load<Texture2D>("button"));
            }

            _deploymentMenu.OnClick += Menu_Click;

            AddComponent(_deploymentMenu);
        }

        public override void Exit(StateManager stateManager)
        {
            _deploymentMenu.OnClick -= Menu_Click;
            base.Exit(stateManager);
        }

        private void Menu_Click(object sender, MenuChoiceEventArgs e)
        {
            switch(e.MenuChoice)
            {
                case MenuChoice.DeployDrone:
                    GameState.UnitToDeploy = UnitType.Drone;
                    GameState.UnitToDeployIsGhost = false;
                    StateManager.ChangeState(DeployUnitState.Instance);
                    break;
                case MenuChoice.DeployCrawler:
                    GameState.UnitToDeploy = UnitType.Crawler;
                    GameState.UnitToDeployIsGhost = false;
                    StateManager.ChangeState(DeployUnitState.Instance);
                    break;
                case MenuChoice.DeployDroneGhost:
                    GameState.UnitToDeploy = UnitType.Drone;
                    GameState.UnitToDeployIsGhost = true;
                    StateManager.ChangeState(DeployUnitState.Instance);
                    break;
                case MenuChoice.DeployCrawlerGhost:
                    GameState.UnitToDeploy = UnitType.Crawler;
                    GameState.UnitToDeployIsGhost = true;
                    StateManager.ChangeState(DeployUnitState.Instance);
                    break;
                case MenuChoice.EndSequence:
                    StateManager.ChangeState(EndPlayerTurnState.Instance);
                    break;
            }
        }
    }
}
