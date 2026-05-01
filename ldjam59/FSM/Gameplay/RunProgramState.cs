using HackThePlanet.Components.Elements;
using Microsoft.Xna.Framework.Graphics;

namespace HackThePlanet.FSM.Gameplay
{
    internal class RunProgramState : MainLoopGameState<RunProgramState>
    {
        private RunProgramComponent _menu;

        public override void Enter(IStateManager stateManager)
        {
            base.Enter(stateManager);

            if (_menu == null)
            {
                _menu = new RunProgramComponent((HackThePlanetGame)stateManager.Game, Content.Load<Texture2D>("button"));
            }

            _menu.OnClick += Menu_Click;
            AddComponent(_menu);
        }

        public override void Exit(IStateManager stateManager)
        {
            _menu.OnClick -= Menu_Click;
            base.Exit(stateManager);
        }

        private void Menu_Click(object sender, MenuChoiceEventArgs e)
        {
            switch (e.MenuChoice)
            {
                case MenuChoice.KillProcess:
                    StateManager.ChangeState(KillProcessState.Instance);
                    break;
                case MenuChoice.ThreatScan:
                    break;
                case MenuChoice.EndSequence:
                    StateManager.ChangeState(EndPlayerTurnState.Instance);
                    break;
            }
        }
    }
}
