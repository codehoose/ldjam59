using HackThePlanet.Components;
using System;

namespace HackThePlanet.FSM.Gameplay
{
    internal class HandoffScreenState : BaseState<HandoffScreenState>
    {
        private TextComponent _text;
        private WaitForMouseClickComponent _waitForIt;

        public override void Enter(StateManager stateManager)
        {
            base.Enter(stateManager);

            if (_text == null)
            {
                _text = new TextComponent(stateManager.Game);
                _waitForIt = new WaitForMouseClickComponent(stateManager.Game);
            }

            _waitForIt.MouseClicked += Mouse_Clicked;
            _text.Text = $"Your turn P<{stateManager.Game.State.CurrentPlayerIndex}> {stateManager.Game.State.CurrentPlayer.Name}";

            AddComponent(_text);
            AddComponent(_waitForIt);
        }

        public override void Exit(StateManager stateManager)
        {
            _waitForIt.MouseClicked -= Mouse_Clicked;
            base.Exit(stateManager);
        }

        private void Mouse_Clicked(object sender, EventArgs e)
        {
            StateManager.ChangeState(SummonState.Instance);
        }
    }
}
