using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;

namespace HackThePlanet.Components.Elements
{
    internal class ToggleButtonComponent : ButtonComponent
    {
        private bool _toggled;

        public string ToggleGroupName { get; set; } = "ToggleGroup";
        public bool Toggled => _toggled;

        public ToggleButtonComponent(HackThePlanetGame game, Texture2D texture, string text, int width, int endCapWidth) : base(game, texture, text, width, endCapWidth)
        {
            OnClick += ToggleButtonComponent_OnClick;
        }

        public void Reset() => _toggled = false;

        protected override bool IsPressed(bool isPressed) => base.IsPressed(isPressed) || Toggled;

        private void ToggleButtonComponent_OnClick(object sender, EventArgs e)
        {
            _toggled = true;

            var otherToggles = Game.Components.Select(c => c as ToggleButtonComponent).Where(c => c != null && c != this);
            foreach (var toggle in otherToggles)
            {
                toggle._toggled = false;
            }
        }

        ~ToggleButtonComponent()
        {
            OnClick -= ToggleButtonComponent_OnClick;
        }
    }
}
