using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace HackThePlanet.Components
{
    internal class SpriteComponent : HtpDrawableComponent
    {
        private Rectangle _srcRect;
        private int _cellWidth;
        private int _cellHeight;
        private int _index;
        private int _lastIndex = -1;

        private int TotalFrames => Rows * Columns;
        public int Rows => Texture.Height / _cellHeight;
        public int Columns => Texture.Width / _cellWidth;

        public int Index
        {
            get => _index;
            set
            {
                _index = Math.Min(Math.Max(0, value), TotalFrames - 1);
            }
        }

        public SpriteComponent(HackThePlanetGame game, Texture2D texture, int cellWidth, int cellHeight, int index = 0, int sort = 0, float scale = 1f)
            : base(game, texture, sort)
        {
            _cellWidth = cellWidth;
            _cellHeight = cellHeight;
            Index = index;
            SortOrder = sort;
            Scale = scale;
        }

        public void SetFrame(int row, int col)
            => Index = row * Columns + col;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (_index != _lastIndex)
            {
                _lastIndex = _index;
                var (row, col) = GetPosition();
                var sx = col * _cellWidth;
                var sy = row * _cellHeight;
                _srcRect = new Rectangle(sx, sy, _cellWidth, _cellHeight);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            HtpGame.SpriteBatch.Draw(Texture, Position, _srcRect, Color, 0f,
                Vector2.Zero, Scale, SpriteEffects.None, SortOrder);
        }

        private (int row, int col) GetPosition()
        {
            var index = Math.Min(Math.Max(0, Index), TotalFrames - 1);

            var row = index / Columns;
            var col = index % Columns;
            return (row, col);
        }
    }
}
