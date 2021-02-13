using Microsoft.Xna.Framework;
using TileGame.Code.GameObjects.Default;
using TileGame.Code.GameObjects.Default.Drawing;
using static TileGame.Code.Events.CollisionEvent;

namespace TileGame.Levels
{
    internal class LevelTile : GameObject
    {
        /// <summary>
        /// Position in grid
        /// </summary>
        private Point Pos;

        /// <summary>
        /// Animated- / spriteobject used to display the tile
        /// </summary>
        internal readonly GameObject displayObject;

        /// <summary>
        /// Default colliding tile constructor
        /// </summary>
        /// <param name="positionInGrid"></param>
        internal LevelTile(Point positionInGrid, int tileSize) : base(Vector2.Zero, 0, 0)
        {
            Pos = positionInGrid;
            displayObject = new SpriteObject(new Vector2(Pos.X * tileSize + tileSize/2, Pos.Y * tileSize + tileSize / 2), tileSize, tileSize, "");
            collides = true;
            width = tileSize;
            height = tileSize;
            centerPosition = new Vector2(Pos.X * tileSize + tileSize / 2, Pos.Y * tileSize + tileSize / 2);
            OnIntersect += _default;
        }
    }
}
