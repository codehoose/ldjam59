using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace HackThePlanet.Components.Elements
{
    internal class ButtonComponent : HtpDrawableComponent
    {
        private readonly SpriteSheet _spriteSheet;        
        private readonly int _width;
        private readonly int _endCapWidth;
        private int _offset;
        private Vector2 _textOffset;
        private ButtonState _previousButtonState;
        private bool _pressedInside;
        
        public event EventHandler OnClick;

        public string Text { get; set; }

        public Color TextColor { get; set; } = Color.Black;

        public bool Disabled { get; set; }

        public ButtonComponent(HackThePlanetGame game, Texture2D texture, string text, int width, int endCapWidth) : base(game, texture, Layer.Gui)
        {
            _spriteSheet = new SpriteSheet(32, 32, texture.Bounds);
            Text = text;
            _width = width;
            _endCapWidth = endCapWidth;
        }

        protected virtual bool IsPressed(bool isPressed) => isPressed;

        public override void Update(GameTime gameTime)
        {
            var mouseState = Mouse.GetState();
            var rect = new Rectangle((int)Position.X, (int)Position.Y, _width, _spriteSheet.TextureHeight);
            var mouseContained = rect.Contains(mouseState.X, mouseState.Y);
            var isPressed = mouseState.LeftButton == ButtonState.Pressed;
            var wasPressed = _previousButtonState == ButtonState.Pressed;

            _textOffset = (new Vector2(rect.Width, rect.Height) - HtpGame.Font.MeasureString(Text)) / 2f;

            _offset = IsPressed(mouseContained && isPressed) ? _spriteSheet.CellWidth : 0;
            
            _textOffset += !Disabled && IsPressed(mouseContained && isPressed) ? new Vector2(2, 2) : Vector2.Zero;
            if (Disabled)
            {
                _offset = _spriteSheet.CellWidth * 2;
                return;
            }

            if (!wasPressed && isPressed)
            {
                _pressedInside = mouseContained;
            }

            if (wasPressed && !isPressed)
            {
                if (_pressedInside && mouseContained)
                {
                    OnClick?.Invoke(this, EventArgs.Empty);
                }

                _pressedInside = false;
            }

            _previousButtonState = mouseState.LeftButton;
        }

        public override void Draw(GameTime gameTime)
        {
            var leftHand = new Rectangle(_offset, 0, _endCapWidth, Texture.Height);
            var rightHand = new Rectangle(_offset + 32 - _endCapWidth, 0, _endCapWidth, Texture.Height);
            var middleSrc = new Rectangle(_offset + 4, 0, 32 - _endCapWidth * 2, Texture.Height);
            var middleDest = new Rectangle((int)Position.X + _endCapWidth, (int)Position.Y, _width - _endCapWidth * 2, Texture.Height);
        
            HtpGame.SpriteBatch.Draw(Texture, Position, leftHand, Color, 0f, Vector2.Zero, Scale, SpriteEffects.None, SortOrder);
            HtpGame.SpriteBatch.Draw(Texture, Position+ new Vector2(_width-_endCapWidth, 0), rightHand, Color, 0f, Vector2.Zero, Scale, SpriteEffects.None, SortOrder);
            HtpGame.SpriteBatch.Draw(Texture, middleDest, middleSrc, Color, 0f, Vector2.Zero, SpriteEffects.None, SortOrder);

            var txtColor = Disabled ? Color.Gray : TextColor;

            HtpGame.SpriteBatch.DrawString(HtpGame.Font, Text, Position + _textOffset, txtColor, 0f, new Vector2(0.5f, 0.5f), 1f, SpriteEffects.None, Layer.GuiFront);
        }
    }
}
