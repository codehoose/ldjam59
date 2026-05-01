namespace HackThePlanet.FSM
{
    using Microsoft.Xna.Framework;

    public class StateManager<T> : IStateManager<T> where T: Game
    {
        private IState _currentState;

        public T Game { get; }

        Game IStateManager.Game => Game;

        public StateManager(T game) : this(game, null) { }

        public StateManager(T game, IState initialState)
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
