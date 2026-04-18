using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace HackThePlanet.Components
{
    internal class SpriteComponent : HtpDrawableComponent
    {
        private SpriteSheet _spriteSheet;

        private int _lastIndex = -1;


        public SpriteComponent(HackThePlanetGame game, Texture2D texture, int cellWidth, int cellHeight, int index = 0, int sort = 0, float scale = 1f)
            : base(game, texture, sort)
        {
            _spriteSheet = new SpriteSheet(cellWidth, cellHeight, texture.Bounds);
            _spriteSheet.Index = index;
            SortOrder = sort;
            Scale = scale;
        }

        public void SetFrame(int row, int col)
            => _spriteSheet.Index = row * _spriteSheet.Columns + col;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (_spriteSheet.Index != _lastIndex)
            {
                _lastIndex = _spriteSheet.Index;
                _spriteSheet.RecalcSrcRect();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            HtpGame.SpriteBatch.Draw(Texture, Position, _spriteSheet.SrcRect, Color, 0f,
                Vector2.Zero, Scale, SpriteEffects.None, SortOrder);
        }

    }
}
