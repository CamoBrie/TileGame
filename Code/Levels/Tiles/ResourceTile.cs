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
    class ResourceTile : AnimatedLevelTile
    {
        internal int resourcesLeft;
        internal int hardness;
        internal int resourceMultiplier = 1;
        bool falling = false;

        internal ResourceTile(LevelGrid grid, Point positionInGrid, int tileSize, string assetName) : base(grid, positionInGrid, tileSize, assetName)
        {
            OnIntersect += HitTile;
        }

        internal void HitTile(GameObject self, GameObject other)
        {
            if(other?.parent is Player && !falling) 
            {
                resourcesLeft--;
                giveResource();

                if (resourcesLeft <= 0)
                {
                    PlayAnimation("Fall");
                    destroy = new Timer(this, 0.6f, () => grid.RemoveTile(Pos));
                    falling = true;
                }
                else
                {
                    PlayAnimation("Hit", true);
                }

                other.parent.RemoveFromChildren(other);
            }
        }

        protected virtual void giveResource()
        {

        }
    }
}
