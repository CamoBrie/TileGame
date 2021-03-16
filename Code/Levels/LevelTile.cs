using Microsoft.Xna.Framework;
using TileGame.Code.GameObjects.Default;
using TileGame.Code.GameObjects.Default.Drawing;
using TileGame.Code.Utils.Convenience;
using TileGame.Levels;
using static TileGame.Code.Events.CollisionEvent;

namespace TileGame.Levels
{
    class LevelTile : GameObject
    {
        /// <summary>
        /// Position in grid
        /// </summary>
        internal Point Pos;

        internal LevelGrid grid;

        internal Timer destroy;

        internal LevelTile(LevelGrid grid, Point positionInGrid, int tileSize) : base(Vector2.Zero, 0, 0)
        {
            this.grid = grid;
            Pos = positionInGrid;
            collides = true;
            OnIntersect += _default;
            centerPosition = new Vector2(Pos.X * tileSize + tileSize / 2, Pos.Y * tileSize + tileSize / 2);
            Initialize(tileSize);
        }

        internal virtual void Initialize(int tileSize)
        {
            width = tileSize;
            height = tileSize;
        }

        internal override void Update(GameTime time)
        {
            if (destroy != null)
            {
                destroy.Update(time);
            }
            base.Update(time);
        }

    }
}
