using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TileGame.Code.Levels.Tiles.Trees;

namespace TileGame.Levels
{
    internal class LevelGrid
    {
        /// <summary>
        /// amount of tiles in the level.
        /// </summary>
        internal int width, height;

        /// <summary>
        /// size of the tiles, in pixels.
        /// </summary>
        internal int tileSize = 64;

        /// <summary>
        /// the grid where every tile is stored.
        /// </summary>
        internal LevelTile[,] grid;

        /// <summary>
        /// the total width of the level, in pixels.
        /// </summary>
        internal int totalWidth => width * tileSize;
        /// <summary>
        /// the total height of the level, in pixels.
        /// </summary>
        internal int totalHeight => height * tileSize;

        /// <summary>
        /// the parent level.
        /// </summary>
        private readonly Level parent;

        /// <summary>
        /// creates a level grid with the specified parameters.
        /// </summary>
        /// <param name="parent">the parent that makes the grid.</param>
        /// <param name="width">the width of the grid, in tiles.</param>
        /// <param name="height">the height of the grid, in tiles.</param>
        /// <param name="path">the path to the file where the level is stored.</param>
        internal LevelGrid(Level parent, int width, int height, string path)
        {
            this.parent = parent;

            this.width = width;
            this.height = height;

            grid = new LevelTile[width, height];


            //testing code
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (x % 3 == 0)
                    {
                        grid[x, y] = new Oak(this, new Point(x, y), tileSize);
                    }
                }
            }
            //testing code
        }

        /// <summary>
        /// Get the position in the grid for any vector2
        /// </summary>
        /// <param name="centerpos">position of the object</param>
        /// <returns></returns>
        internal Point PosInGrid(Vector2 centerpos)
        {
            Point x = Vector2.Clamp(centerpos / new Vector2(tileSize), new Vector2(0), new Vector2(width, height)).ToPoint();
            return x;
        }

        internal void RemoveTile(Point pos)
        {
            grid[pos.X, pos.Y] = null;
        }

        /// <summary>
        /// gets the tile in the grid based on a point.
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        internal LevelTile GetTileInGrid(Point pos)
        {
            return grid[pos.X, pos.Y];
        }

        /// <summary>
        /// gets the list of tiles that can collide.
        /// </summary>
        /// <returns></returns>
        internal List<LevelTile> GetCollisionTiles()
        {
            List<LevelTile> ret = new List<LevelTile>();
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if(grid[x,y] != null && grid[x,y].hasCollision)
                    {
                        ret.Add(grid[x, y]);
                    }
                }
            }

            return ret;
        }

        #region Drawing
        /// <summary>
        /// all the tiles that have to be drawn before the player.
        /// </summary>
        /// <param name="batch"></param>
        internal void Pre_draw(SpriteBatch batch)
        {
            foreach(LevelTile lt in GetDrawTiles(true))
            {
                lt.Draw(batch);
            }
        }
        /// <summary>
        /// all the tiles that have to be drawn after the player.
        /// </summary>
        /// <param name="batch"></param>
        internal void Post_draw(SpriteBatch batch)
        {
            foreach (LevelTile lt in GetDrawTiles(false))
            {
                lt.Draw(batch);
            }
        }

        /// <summary>
        /// gets all the tiles that have to be drawn.
        /// </summary>
        /// <param name="before">if we need to provide the tiles before or after the player.</param>
        /// <returns></returns>
        private List<LevelTile> GetDrawTiles(bool before = true)
        {
            List<LevelTile> ret = new List<LevelTile>();
            Point playerPos = PosInGrid(parent.player.globalPosition);
            if (before)
            {
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < playerPos.Y; y++)
                    {
                        if (grid[x, y] != null)
                        {
                            ret.Add(grid[x, y]);
                        }
                    }
                }
            } else
            {
                for (int x = 0; x < width; x++)
                {
                    for (int y = playerPos.Y; y < height; y++)
                    {
                        if (grid[x, y] != null)
                        {
                            ret.Add(grid[x, y]);
                        }
                    }
                }
            }

            return ret;
        }

        internal void Update(GameTime time)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (grid[x, y] != null)
                    {
                        grid[x, y].Update(time);
                    }
                }
            }
        }

        #endregion

    }
}
