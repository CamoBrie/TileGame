using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileGame.Code.Utils.Convenience;
using static TileGame.Code.Events.CollisionEvent;

namespace TileGame.Code.GameObjects.Default.Items
{
    class Tool
    {
        GameEntity parent;
        internal Timer cooldown = new Timer();
        internal bool ready
        {
            get => cooldown.ReachedEnd;
        }
        internal Tool(GameEntity parent)
        {
            this.parent = parent;
        }
        internal void Update(GameTime time)
        {
            this.cooldown?.Update(time);
        }

        internal void Swing(Vector2 direction)
        {
            parent.isMoving = false;

            cooldown = new Timer(0.5f, () => {
                CollisionObject toolCollision = new CollisionObject(direction * 64, 10, 10, _HitTile);
                toolCollision.SetTimer(0.5f, () => parent.RemoveFromChildren(toolCollision));
                parent.AddToChildren(toolCollision);

                cooldown = new Timer(1f, () => { 
                    parent.isMoving = true; 
                });
            });
        }
    }
}
