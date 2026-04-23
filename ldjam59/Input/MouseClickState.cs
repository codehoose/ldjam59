using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace HackThePlanet.Input
{
    internal class MouseClickState
    {
        private bool _pressedInside;
        private Rectangle? _bounds;
        private Vector2 _clickPosition;
        private Vector2 _position;
        private ButtonState _previousButtonState;

        public event EventHandler<Vector2> Click;

        public Vector2 Position => _position;
        public bool Contained { get; private set; }

        public MouseClickState(Rectangle? bounds)
        {
            _bounds = bounds;
        }

        public void Tick(float deltaTime)
        {
            var mouseState = Mouse.GetState();
            _position = new Vector2(mouseState.X, mouseState.Y);

            Contained = _bounds == null ? true : _bounds.Value.Contains(_position);
            if (!Contained) return;

            var mouseContained = Contained;
            var isPressed = mouseState.LeftButton == ButtonState.Pressed;
            var wasPressed = _previousButtonState == ButtonState.Pressed;

            if (!wasPressed && isPressed)
            {
                _pressedInside = mouseContained;
                _clickPosition = _position;
            }

            if (wasPressed && !isPressed)
            {
                if (_pressedInside && mouseContained)
                {
                    _previousButtonState = ButtonState.Released;
                    Click?.Invoke(this, _clickPosition);
                }

                _pressedInside = false;
            }

            _previousButtonState = mouseState.LeftButton;
        }
    }
}
