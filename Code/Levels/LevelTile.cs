using Microsoft.Xna.Framework;
using TileGame.Code.GameObjects.Default;

namespace TileGame.Levels
{
    abstract class LevelTile : GameObject
    {
        /// <summary>
        /// Position in grid
        /// </summary>
        Point Pos;

        /// <summary>
        /// Animated- / spriteobject used to display the tile
        /// </summary>
        GameObject displayObject;

        /// <summary>
        /// Default colliding tile constructor
        /// </summary>
        /// <param name="positionInGrid"></param>
        internal LevelTile(Point positionInGrid) : base(Vector2.Zero, 0, 0)
        {
            this.Pos = positionInGrid;
            
        }
    }
}
