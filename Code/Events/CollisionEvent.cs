using Microsoft.Xna.Framework;
using System;
using TileGame.Code.GameObjects.Default;

namespace TileGame.Code.Events
{
    static class CollisionEvent
    {
        /// <summary>
        /// The event that is fired on a collision event.
        /// </summary>
        /// <param name="sender">sender of the collision.</param>
        /// <param name="collider">collider of the collision.</param>
        internal delegate void collisionEvent(GameObject sender, GameObject collider);

        internal static void _default(GameObject tile, GameObject entity)
        {
            if (entity is GameEntity collEntity)
            {

                // Calculate current and minimum-non-intersecting distances between centers.
                float distanceX = tile.centerPosition.X - entity.centerPosition.X;
                float distanceY = tile.centerPosition.Y - entity.centerPosition.Y;
                float minDistanceX = tile.width / 2 + entity.width / 2;
                float minDistanceY = tile.height / 2 + entity.height / 2;

                float depthX = distanceX > 0 ? minDistanceX - distanceX : -minDistanceX - distanceX;
                float depthY = distanceY > 0 ? minDistanceY - distanceY : -minDistanceY - distanceY;


                Rectangle tileBox = tile.BoundingBox;
                if (Math.Abs(depthX) < Math.Abs(depthY))
                {
                    // Collision on the X axis
                    if (depthX > 0)
                    {
                        // Collision on entity right
                        entity.centerPosition = new Vector2(tileBox.Left - (entity.width / 2), entity.centerPosition.Y);
                    }
                    else
                    {
                        // Collision on entity left
                        entity.centerPosition = new Vector2(tileBox.Right + (entity.width / 2), entity.centerPosition.Y);
                    }

                    collEntity.velocity.X = 0;
                }
                else if (Math.Abs(depthX) >= Math.Abs(depthY))
                {
                    // Collision on the Y axis
                    if (depthY > 0)
                    {
                        // Collision on entity bottom
                        entity.centerPosition = new Vector2(entity.centerPosition.X, tileBox.Top - entity.height / 2);
                    }
                    else
                    {
                        // Collision on entity top
                        entity.centerPosition = new Vector2(entity.centerPosition.X, tileBox.Bottom + entity.height / 2);
                    }

                    collEntity.velocity.Y = 0;
                }
            }

        }
    }


}
