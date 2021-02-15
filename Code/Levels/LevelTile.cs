using Microsoft.Xna.Framework;
using TileGame.Code.GameObjects.Default;
using TileGame.Code.GameObjects.Default.Drawing;
using TileGame.Code.Utils.Convenience;
using static TileGame.Code.Events.CollisionEvent;

namespace TileGame.Levels
{
    internal class LevelTile : GameObject
    {
        /// <summary>
        /// Position in grid
        /// </summary>
        internal Point Pos;

        /// <summary>
        /// Animated- / spriteobject used to display the tile
        /// </summary>
        internal GameObject displayObject;

        internal LevelGrid grid;

        internal Timer destroy;

        /// <summary>
        /// Default colliding tile constructor
        /// </summary>
        /// <param name="positionInGrid"></param>
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
            displayObject = new SpriteObject(new Vector2(Pos.X * tileSize + tileSize / 2, Pos.Y * tileSize + tileSize / 2), tileSize, tileSize, "");
            width = tileSize;
            height = tileSize;
        }

        internal override void Update(GameTime time)
        {
            if(displayObject is AnimatedObject ao)
            {
                ao.Update(time);
            }
            if(destroy != null)
            {
                destroy.Update(time);
            }
        }
    }
}
