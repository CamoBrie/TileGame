using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileGame.Code.GameObjects;
using TileGame.Code.GameObjects.Default;
using TileGame.Code.GameObjects.Default.Drawing;
using TileGame.Code.GameObjects.Default.Items;
using TileGame.Code.Utils.Convenience;
using TileGame.Levels;

namespace TileGame.Code.Levels.Tiles
{
    class ResourceTile : LevelTile
    {
        internal int resourcesLeft;
        internal int hardness;

        internal AnimatedObject display => (AnimatedObject)displayObject;

        internal ResourceTile(LevelGrid grid, Point positionInGrid, int tileSize) : base(grid, positionInGrid, tileSize)
        {
            OnIntersect += HitTile;
        }

        internal void HitTile(GameObject self, GameObject other)
        {
            if(other?.parent is Player) 
            {
                resourcesLeft--;
                

                if (resourcesLeft <= 0)
                {
                    display.PlayAnimation("Fall");
                    destroy = new Timer(this, 0.6f, () => grid.RemoveTile(Pos));
                }
                else
                {
                    display.PlayAnimation("Hit", true);
                }

                other.parent.RemoveFromChildren(other);
            }
        }
    }
}
