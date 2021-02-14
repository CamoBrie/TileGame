using Microsoft.Xna.Framework;
using TileGame.Code.Utils.Convenience;
using static TileGame.Code.Events.CollisionEvent;

namespace TileGame.Code.GameObjects.Default.Items
{
    internal class Tool
    {
        /// <summary>
        /// the parent of the tool.
        /// </summary>
        private readonly GameEntity parent;

        /// <summary>
        /// the cooldown before the tool can be used again.
        /// </summary>
        internal Timer cooldown = new Timer();

        /// <summary>
        /// determines if the tool can be used.
        /// </summary>
        internal bool ready => cooldown.ReachedEnd;

        /// <summary>
        /// creates a tool with the specified parent.
        /// </summary>
        /// <param name="parent"></param>
        internal Tool(GameEntity parent)
        {
            this.parent = parent;
        }

        /// <summary>
        /// updates the cooldown of the tool.
        /// </summary>
        /// <param name="time"></param>
        internal void Update(GameTime time)
        {
            cooldown?.Update(time);
        }

        /// <summary>
        /// swings the tool in a certain direction, preventing movement and placing a collision
        /// trigger next to where the player is aiming.
        /// </summary>
        /// <param name="direction"></param>
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
