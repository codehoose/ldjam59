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


            //Components.Add(new BackgroundGridComponent(this, Content.Load<Texture2D>("block")));

            //var sprite = new SpriteComponent(this, _sprites, 16, 16, 23, scale: 4);
            //sprite.Color = Palette.BloodOrange;
            //Components.Add(sprite);

            // TODO: use this.Content to load your game content here
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
            //transformMatrix: Matrix.CreateTranslation(0, 8, 0));
            //transformMatrix: Matrix.CreateScale(4f));

            // TODO: Add your drawing code here
            //_spriteBatch.Draw(_sprites, Vector2.Zero, Color.White);

            base.Draw(gameTime);
            _spriteBatch.End();
        }
    }
}
