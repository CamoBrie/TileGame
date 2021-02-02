using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileGame.Collision;
using TileGame.GameObjects;

namespace TileGame.Levels
{
    class LevelGrid
    {
        internal static int width, height;

        internal static int tileSize;

        private static LevelTile[,] grid;

        internal LevelGrid(int width, int height)
        {
            grid = new LevelTile[width, height];
        }

        /// <summary>
        /// Get the position in the grid for any vector2
        /// </summary>
        /// <param name="centerpos">position of the object</param>
        /// <returns></returns>
        internal static Point PosInGrid(Vector2 centerpos)
        {
            return Vector2.Clamp(centerpos / new Vector2(tileSize), Vector2.Zero, new Vector2(width, height)).ToPoint();
        }

        internal static LevelTile GetTileInGrid(Point pos)
        {
            return grid[pos.X, pos.Y];
        }

    }
}
