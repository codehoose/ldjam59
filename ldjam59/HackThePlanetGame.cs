using HackThePlanet.Components;
using HackThePlanet.FSM.Gameplay;
using HackThePlanet.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HackThePlanet
{
    public class HackThePlanetGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private FSMComponent _stateManager;

        public static HackThePlanetGame Instance { get; private set; }

        internal GameState State { get; private set; } = new();

        public SpriteBatch SpriteBatch { get { return _spriteBatch; } }

        public SpriteFont Font { get; private set; }
        public Texture2D WhiteHat { get; private set; }
        public Texture2D BlackHat { get; private set; }
        public Texture2D Crawler { get; private set; }
        public Texture2D Drone { get; private set; }
        public Texture2D Ghost { get; private set; }

        public HackThePlanetGame()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 960,
                PreferredBackBufferHeight = 540
            };
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Instance = this;
            Window.Title = "Hack the Planet!";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _stateManager = new FSMComponent(this);
            Components.Add(_stateManager);
            _stateManager.StateManager.ChangeState(InitializeGameState.Instance);

            Font = Content.Load<SpriteFont>("tempfont");
            WhiteHat = Content.Load<Texture2D>("ai-white");
            BlackHat = Content.Load<Texture2D>("ai-black");
            Crawler = Content.Load<Texture2D>("crawler");
            Drone = Content.Load<Texture2D>("drone");
            Ghost = Content.Load<Texture2D>("incognito");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin(SpriteSortMode.BackToFront,
                               BlendState.AlphaBlend,
                               SamplerState.PointWrap,
                               DepthStencilState.Default,
                               RasterizerState.CullNone);
            base.Draw(gameTime);
            _spriteBatch.End();
        }
    }
}
