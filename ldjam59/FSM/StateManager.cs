namespace HackThePlanet.FSM
{
    internal class StateManager
    {
        private IState _currentState;

        internal HackThePlanetGame Game { get; }

        public StateManager(HackThePlanetGame game) : this(game, null) { }

        public StateManager(HackThePlanetGame game, IState initialState)
        {
            Game = game;
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
                _currentState.Exit(this);
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
