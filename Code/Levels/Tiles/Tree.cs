using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileGame.Code.GameObjects.Default.Drawing;
using TileGame.Levels;
using static TileGame.Code.Events.CollisionEvent;

namespace TileGame.Code.Levels.Tiles
{
    class Tree : ResourceTile
    {
        internal Tree(LevelGrid grid, Point positionInGrid, int tileSize) : base(grid, positionInGrid, tileSize)
        {
        }

        internal void Initialize(int tileSize, string assetName)
        {
            displayObject = new AnimatedObject(new Vector2(Pos.X * tileSize + tileSize / 2, Pos.Y * tileSize), tileSize, tileSize * 2, assetName);
            width = (int)(tileSize * 0.5f);
            height = (int)(tileSize);
        }
    }
}
