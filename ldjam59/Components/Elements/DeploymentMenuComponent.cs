namespace HackThePlanet.Components.Elements
{
    using Gwydion.Components;
    using HackThePlanet.Models;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using System;

    internal class DeploymentMenuComponent : HtpComponent, IParentComponent
    {
        private readonly ToggleButtonComponent _deployCrawler;
        private readonly ToggleButtonComponent _deployDrone;
        private readonly ToggleButtonComponent _deployCrawlerGhost;
        private readonly ToggleButtonComponent _deployDroneGhost;
        private readonly ButtonComponent _endDeploy;
        private int _cycles;

        public event EventHandler<MenuChoiceEventArgs> OnClick;

        public DeploymentMenuComponent(HackThePlanetGame game, Texture2D buttonTexture) : base(game)
        {
            var pos = new Vector2(750 - 380 / 2, 220);
            _cycles = GameState.Instance.Cycles;

            _deployCrawler = new ToggleButtonComponent(game, buttonTexture, "Deploy Crawler", 380, 4)
            {
                Position = pos,
            };
            pos += new Vector2(0, 40);
            _deployCrawler.OnClick += Button_Click;
            _deployCrawler.Disabled = GameState.Instance.Cycles < 1;

            _deployCrawlerGhost = new ToggleButtonComponent(game, buttonTexture, "Deploy Crawler Ghost", 380, 4)
            {
                Position = pos,
            };
            pos += new Vector2(0, 40);
            _deployCrawlerGhost.OnClick += Button_Click;
            _deployCrawlerGhost.Disabled = GameState.Instance.Cycles < 1;

            _deployDrone = new ToggleButtonComponent(game, buttonTexture, "Deploy Drone", 380, 4)
            {
                Position = pos
            };
            pos += new Vector2(0, 40);
            _deployDrone.OnClick += Button_Click;
            _deployDrone.Disabled = GameState.Instance.Cycles < 2;

            _deployDroneGhost = new ToggleButtonComponent(game, buttonTexture, "Deploy Drone Ghost", 380, 4)
            {
                Position = pos
            };
            pos += new Vector2(0, 60);
            _deployDroneGhost.OnClick += Button_Click;
            _deployDroneGhost.Disabled = GameState.Instance.Cycles < 1;

            _endDeploy = new ButtonComponent(game, buttonTexture, "End Deploy", 380, 4)
            {
                Position = pos
            };
            _endDeploy.OnClick += Button_Click;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (_cycles != GameState.Instance.Cycles)
            {
                _deployCrawler.Disabled = GameState.Instance.Cycles < 1;
                _deployCrawlerGhost.Disabled = GameState.Instance.Cycles < 1;
                _deployDrone.Disabled = GameState.Instance.Cycles < 2;
                _deployDroneGhost.Disabled = GameState.Instance.Cycles < 1;
                _cycles = GameState.Instance.Cycles;
            }
        }

        public void RemoveComponents()
        {
            HtpGame.Components.Remove(_deployCrawler);
            HtpGame.Components.Remove(_deployDrone);
            HtpGame.Components.Remove(_deployCrawlerGhost);
            HtpGame.Components.Remove(_deployDroneGhost);
            HtpGame.Components.Remove(_endDeploy);
        }

        public void AddComponents()
        {
            _deployCrawler.Reset();
            _deployDrone.Reset();
            _deployCrawlerGhost.Reset();
            _deployDroneGhost.Reset();

            HtpGame.Components.Add(_deployCrawler);
            HtpGame.Components.Add(_deployDrone);
            HtpGame.Components.Add(_deployCrawlerGhost);
            HtpGame.Components.Add(_deployDroneGhost);
            HtpGame.Components.Add(_endDeploy);
        }

        private void Button_Click(object sender, EventArgs e)
        {
            var choice = MenuChoice.EndSequence;
            if (sender.Equals(_deployCrawler)) choice = MenuChoice.DeployCrawler;
            if (sender.Equals(_deployDrone)) choice = MenuChoice.DeployDrone;
            if (sender.Equals(_deployCrawlerGhost)) choice = MenuChoice.DeployCrawlerGhost;
            if (sender.Equals(_deployDroneGhost)) choice = MenuChoice.DeployDroneGhost;

            OnClick?.Invoke(this, new MenuChoiceEventArgs(choice));
        }
    }
}
