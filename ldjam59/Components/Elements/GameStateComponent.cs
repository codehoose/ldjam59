using Microsoft.Xna.Framework;

namespace HackThePlanet.Components.Elements
{
    internal class GameStateComponent : HtpComponent, IParentComponent
    {
        private TextComponent _text;

        public GameStateComponent(HackThePlanetGame game) : base(game)
        {
            _text = new TextComponent(game);
            _text.Position = new Vector2(560, 16);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _text.Text = $"Player: {HtpGame.State.CurrentPlayer.Name}, Cycles: {HtpGame.State.Cycles}";

        }

        public void AddComponents()
        {
            Game.Components.Add(_text);
        }

        public void RemoveComponents()
        {
            Game.Components.Remove(_text);
        }
    }
}
