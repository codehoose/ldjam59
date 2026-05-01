using HackThePlanet.Components;
using HackThePlanet.FSM;
using HackThePlanet.FSM.Gameplay.Flow;
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
        private IStateManager _stateManager;
        private FSMComponent _stateManagerComponent;

        public static HackThePlanetGame Instance { get; private set; }

        internal GameState State { get; private set; } = new();

        public SpriteBatch SpriteBatch { get { return _spriteBatch; } }

        public SpriteFont Font { get; private set; }
        public Texture2D WhiteHat { get; private set; }
        public Texture2D BlackHat { get; private set; }
        public Texture2D Ghost { get; private set; }
        public Texture2D HasBeenUsed { get; private set; }
        public Texture2D Units { get; private set; }
        public Texture2D SelectionCursor { get; private set; }
        public Texture2D Hackerman { get; private set; }
        public Texture2D HackermanSide { get; private set; }

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

            Font = Content.Load<SpriteFont>("tempfont");
            WhiteHat = Content.Load<Texture2D>("ai-white");
            BlackHat = Content.Load<Texture2D>("ai-black");
            Units = Content.Load<Texture2D>("units");
            Ghost = Content.Load<Texture2D>("incognito");
            SelectionCursor = Content.Load<Texture2D>("selection");
            Hackerman = Content.Load<Texture2D>("hackerman");
            HackermanSide = Content.Load<Texture2D>("hackermanside");
            HasBeenUsed = Content.Load<Texture2D>("hasbeenused");

            _stateManager = new StateManager<HackThePlanetGame>(this);
            _stateManagerComponent = new FSMComponent(this, _stateManager);
            Components.Add(_stateManagerComponent);
            _stateManagerComponent.StateManager.ChangeState(TitleScreenState.Instance);
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
