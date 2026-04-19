using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace HackThePlanet.Components.Elements
{
    internal class RunProgramComponent : HtpComponent, IParentComponent
    {
        private readonly ButtonComponent _killProcess;
        private readonly ButtonComponent _threatScan;
        private readonly ButtonComponent _endDeploy;
        private int _cycles;

        public event EventHandler<MenuChoiceEventArgs> OnClick;

        public RunProgramComponent(HackThePlanetGame game, Texture2D buttonTexture) : base(game)
        {
            var pos = new Vector2(750 - 380 / 2, 220);
            _cycles = game.State.Cycles;

            _killProcess = new ButtonComponent(game, buttonTexture, "Kill Process", 380, 4)
            {
                Position = pos,
            };
            pos += new Vector2(0, 40);
            _killProcess.OnClick += Button_Click;
            _killProcess.Disabled = HtpGame.State.Cycles < 2;

            _threatScan = new ButtonComponent(game, buttonTexture, "Threat Scan", 380, 4)
            {
                Position = pos,
                Disabled = true
            };

            pos += new Vector2(0, 80);
            _threatScan.OnClick += Button_Click;
            //_threatScan.Disabled = HtpGame.State.Cycles < 2;

            pos += new Vector2(0, 60);

            _endDeploy = new ButtonComponent(game, buttonTexture, "End Turn", 380, 4)
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
                _killProcess.Disabled = HtpGame.State.Cycles < 2;
                _threatScan.Disabled = true || HtpGame.State.Cycles < 2;

                _cycles = HtpGame.State.Cycles;
            }
        }

        public void RemoveComponents()
        {
            HtpGame.Components.Remove(_killProcess);
            HtpGame.Components.Remove(_threatScan);
            HtpGame.Components.Remove(_endDeploy);
        }

        public void AddComponents()
        {
            HtpGame.Components.Add(_killProcess);
            HtpGame.Components.Add(_threatScan);
            HtpGame.Components.Add(_endDeploy);
        }

        private void Button_Click(object sender, EventArgs e)
        {
            var choice = MenuChoice.EndSequence;
            if (sender.Equals(_killProcess)) choice = MenuChoice.KillProcess;
            if (sender.Equals(_threatScan)) choice = MenuChoice.ThreatScan;

            OnClick?.Invoke(this, new MenuChoiceEventArgs(choice));
        }
    }
}
