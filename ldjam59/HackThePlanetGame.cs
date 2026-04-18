using HackThePlanet.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HackThePlanet
{
    public class HackThePlanetGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _sprites;

        public SpriteBatch SpriteBatch { get { return _spriteBatch; } }

        public HackThePlanetGame()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 960,
                PreferredBackBufferHeight = 540
            };
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Components.Add(new BackgroundGridComponent(this, Content.Load<Texture2D>("block")));

            //var sprite = new SpriteComponent(this, _sprites, 16, 16, 23, scale: 4);
            //sprite.Color = Palette.BloodOrange;
            //Components.Add(sprite);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin(SpriteSortMode.FrontToBack,
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
