using Microsoft.Xna.Framework;

namespace ldjam59.Components
{
    internal class SKDrawableGameComponent<T> : DrawableGameComponent where T : Game
    {
        public T TheGame { get; }
        public SKDrawableGameComponent(T game) : base(game)
        {
            TheGame = game;
        }
    }
}
