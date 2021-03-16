using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileGame.Code.GameObjects;
using TileGame.Code.GameObjects.Default.Drawing;
using TileGame.Levels;
using static TileGame.Code.Events.CollisionEvent;

namespace TileGame.Code.Levels.Tiles
{
    class Tree : ResourceTile
    {
        //internal override Rectangle GetBoundingBox()
        //{
        //    return new Rectangle((int)globalPosition.X - width / 4, (int)globalPosition.Y - height / 2, width/2, height);
        //}

        //internal override Rectangle GetDrawPos()
        //{
        //    return new Rectangle((int)globalPosition.X - width / 2, (int)globalPosition.Y - (int)(height*1.5f), width, height*2);
        //}

        internal Tree(LevelGrid grid, Point positionInGrid, int tileSize, string assetName) : base(grid, positionInGrid, tileSize, assetName)
        {

        }

        internal override void Initialize(int tileSize)
        {
            width = (int)(tileSize);
            height = (int)(tileSize);
        }

        protected override void giveResource()
        {
            base.giveResource();
            Player.inventory.wood += resourceMultiplier;
        }
    }
}
