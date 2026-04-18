namespace HackThePlanet.FSM
{
    internal class StateManager
    {
        private IState _currentState;

        public StateManager() : this(null) { }

        public StateManager(IState initialState)
        {
            if (initialState != null)
            {
                _currentState = initialState;
                _currentState.Enter(this);
            }
        }

        public void ChangeState(IState newState)
        {
            if (_currentState!=null)
            {
                _currentState.Exit();
            }

            _currentState = newState;
            _currentState?.Enter(this);
        }

        public void Tick(float deltaTime)
        {
            if (_currentState == null) return;

            _currentState.Tick(deltaTime);
        }
    }
}
