using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace HackThePlanet.Components.Elements
{
    internal class DeploymentMenuComponent : HtpComponent, IParentComponent
    {
        private readonly ButtonComponent _deployCrawler;
        private readonly ButtonComponent _deployDrone;
        private readonly ButtonComponent _endDeploy;

        public event EventHandler<MenuChoiceEventArgs> OnClick;

        public DeploymentMenuComponent(HackThePlanetGame game, Texture2D buttonTexture) : base(game)
        {
            var pos = new Vector2(750 - 380 / 2, 300);

            _deployCrawler = new ButtonComponent(game, buttonTexture, "Deploy Crawler", 380, 4)
            {
                Position = pos,
            };
            pos += new Vector2(0, 40);
            _deployCrawler.OnClick += Button_Click;

            _deployDrone = new ButtonComponent(game, buttonTexture, "Deploy Drone", 380, 4)
            {
                Position = pos
            };
            _deployDrone.OnClick += Button_Click;

            pos += new Vector2(0, 60);
            _endDeploy = new ButtonComponent(game, buttonTexture, "End Deploy", 380, 4)
            {
                Position = pos
            };
            _endDeploy.OnClick += Button_Click;
        }

        public void RemoveComponents()
        {
            HtpGame.Components.Remove(_deployCrawler);
            HtpGame.Components.Remove(_deployDrone);
            HtpGame.Components.Remove(_endDeploy);
        }

        public void AddComponents()
        {
            HtpGame.Components.Add(_deployCrawler);
            HtpGame.Components.Add(_deployDrone);
            HtpGame.Components.Add(_endDeploy);
        }

        private void Button_Click(object sender, EventArgs e)
        {
            var choice = MenuChoice.EndSequence;
            if (sender.Equals(_deployCrawler)) choice = MenuChoice.DeployCrawler;
            if (sender.Equals(_deployDrone)) choice = MenuChoice.DeployDrone;

            OnClick?.Invoke(this, new MenuChoiceEventArgs(choice));
        }
    }
}
