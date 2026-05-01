namespace HackThePlanet.Components.Elements
{
	using HackThePlanet.Models;
	using Microsoft.Xna.Framework;

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

            _text.Text = $"Player: {GameState.Instance.CurrentPlayer.Name}, Cycles: {GameState.Instance.Cycles}";

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
