using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileGame.Code.GameObjects.Default.Drawing;
using TileGame.Levels;

namespace TileGame.Code.Levels.Tiles.Trees
{
    class Oak : Tree
    {
        internal Oak(LevelGrid grid, Point positionInGrid, int tileSize) : base(grid, positionInGrid, tileSize)
        {
            Initialize(tileSize, "views/game/trees/Oak");
            this.hardness = 1;
            this.resourcesLeft = 3;
        }
    }
}
