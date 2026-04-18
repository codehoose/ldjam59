using HackThePlanet.Components;
using HackThePlanet.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;

namespace HackThePlanet.FSM
{
    internal abstract class BaseState<T> : IState where T: IState
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

        protected StateManager StateManager { get; private set; }
        protected HackThePlanetGame Game => StateManager.Game;
        protected ContentManager Content => StateManager.Game.Content;
        protected GameState GameState => StateManager.Game.State;

        public virtual void Enter(StateManager stateManager)
        {
            StateManager = stateManager;
        }

        public virtual void Exit(StateManager stateManager)
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
    }
}
