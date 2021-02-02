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
        internal LevelTile(Point positionInGrid) : base()
        {
            this.Pos = positionInGrid;
            
        }
    }
}
