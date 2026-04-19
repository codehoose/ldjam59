using HackThePlanet.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace HackThePlanet.Components.Elements
{
    internal class DeploymentMenuComponent : HtpComponent, IParentComponent
    {
        private readonly ButtonComponent _deployCrawler;
        private readonly ButtonComponent _deployDrone;
        private readonly ButtonComponent _deployCrawlerGhost;
        private readonly ButtonComponent _deployDroneGhost;
        private readonly ButtonComponent _endDeploy;
        private int _cycles;

        public event EventHandler<MenuChoiceEventArgs> OnClick;

        public DeploymentMenuComponent(HackThePlanetGame game, Texture2D buttonTexture) : base(game)
        {
            var pos = new Vector2(750 - 380 / 2, 220);
            _cycles = game.State.Cycles;

            _deployCrawler = new ButtonComponent(game, buttonTexture, "Deploy Crawler", 380, 4)
            {
                Position = pos,
            };
            pos += new Vector2(0, 40);
            _deployCrawler.OnClick += Button_Click;
            _deployCrawler.Disabled = HtpGame.State.Cycles < 1;

            _deployCrawlerGhost = new ButtonComponent(game, buttonTexture, "Deploy Crawler Ghost", 380, 4)
            {
                Position = pos,
            };
            pos += new Vector2(0, 40);
            _deployCrawlerGhost.OnClick += Button_Click;
            _deployCrawlerGhost.Disabled = HtpGame.State.Cycles < 1;

            _deployDrone = new ButtonComponent(game, buttonTexture, "Deploy Drone", 380, 4)
            {
                Position = pos
            };
            pos += new Vector2(0, 40);
            _deployDrone.OnClick += Button_Click;
            _deployDrone.Disabled = HtpGame.State.Cycles < 2;

            _deployDroneGhost = new ButtonComponent(game, buttonTexture, "Deploy Drone Ghost", 380, 4)
            {
                Position = pos
            };
            pos += new Vector2(0, 60);
            _deployDroneGhost.OnClick += Button_Click;
            _deployDroneGhost.Disabled = HtpGame.State.Cycles < 1;

            _endDeploy = new ButtonComponent(game, buttonTexture, "End Deploy", 380, 4)
            {
                Position = pos
            };
            _endDeploy.OnClick += Button_Click;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (_cycles != HtpGame.State.Cycles)
            {
                _deployCrawler.Disabled = HtpGame.State.Cycles < 1;
                _deployCrawlerGhost.Disabled = HtpGame.State.Cycles < 1;
                _deployDrone.Disabled = HtpGame.State.Cycles < 2;
                _deployDrone.Disabled = HtpGame.State.Cycles < 1;
                _cycles = HtpGame.State.Cycles;
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
