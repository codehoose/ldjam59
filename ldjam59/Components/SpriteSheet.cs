using Microsoft.Xna.Framework;
using System;

namespace HackThePlanet.Components
{
    internal class SpriteSheet
    {
        private Rectangle _srcRect;
        private int _cellWidth;
        private int _cellHeight;
        private int _index;

        private int TotalFrames => Rows * Columns;

        public int TextureHeight { get; }
        public int TextureWidth { get; }
        public int Rows => TextureHeight / _cellHeight;
        public int Columns => TextureWidth / _cellWidth;
        public int CellWidth => _cellWidth;
        public int CellHeight => _cellHeight;

        public Rectangle SrcRect
        {
            get => _srcRect;
            set => _srcRect = value;
        }

        public int Index
        {
            get => _index;
            set
            {
                _index = Math.Min(Math.Max(0, value), TotalFrames - 1);
            }
        }

        public SpriteSheet(int cellWidth, int cellHeight, Rectangle textureBounds)
        {
            _cellWidth = cellWidth;
            _cellHeight = cellHeight;
            TextureHeight = textureBounds.Height;
            TextureWidth = textureBounds.Width;
        }

        public void RecalcSrcRect()
        {
            var (row, col) = GetPosition();
            var sx = col * CellWidth;
            var sy = row * CellHeight;
            SrcRect = new Rectangle(sx, sy, CellWidth, CellHeight);
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
