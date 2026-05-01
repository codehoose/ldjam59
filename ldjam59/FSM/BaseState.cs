namespace HackThePlanet.FSM
{
    using HackThePlanet.Components;
    using HackThePlanet.Models;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using System;
    using System.Collections.Generic;

    public abstract class BaseState<T> : IState where T: IState
    {
        private List<GameComponent> _components = [];
        
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance==null)
                {
                    _instance = Activator.CreateInstance<T>();
                }
                return _instance;
            }
        }

        protected IStateManager StateManager { get; private set; }
        //protected HackThePlanetGame Game => StateManager.Game;
        protected ContentManager Content => StateManager.Game.Content;
        //protected GameState GameState => GameState.Instance;

        public virtual void Enter(IStateManager stateManager)
        {
            StateManager = stateManager;
        }

        public virtual void Exit(IStateManager stateManager)
        {
            foreach (var c in _components)
            {
                if (c is IParentComponent parent)
                {
                    parent.RemoveComponents();
                }

                HackThePlanetGame.Instance.Components.Remove(c);
            }
        }

        public virtual void Tick(float deltaTime)
        {
            
        }

        protected void AddComponent(GameComponent component)
        {
            HackThePlanetGame.Instance.Components.Add(component);
            _components.Add(component);

            if (component is IParentComponent parent)
            {
                parent.AddComponents();
            }
        }

        protected void RemoveComponent(GameComponent component)
        {
            HackThePlanetGame.Instance.Components.Remove(component);
            _components.Remove(component);

            if (component is IParentComponent parent)
            {
                parent.RemoveComponents();
            }
        }
    }
}
