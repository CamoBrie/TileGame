using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        internal Oak(LevelGrid grid, Point positionInGrid, int tileSize) : base(grid, positionInGrid, tileSize, "views/game/trees/Oak_test")
        {
            Initialize(tileSize);
            this.hardness = 1;
            this.resourcesLeft = 3;
            resourceMultiplier = 1;
        }

        internal override void Draw(SpriteBatch batch)
        {
            base.Draw(batch);
        }
    }
}
