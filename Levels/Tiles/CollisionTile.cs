using Microsoft.Xna.Framework;
using System;
using TileGame.GameObjects;

namespace TileGame.Levels.Tiles
{
    internal class CollisionTile : GameObject
    {
        /// <summary>
        /// default collision object, just acts a wall
        /// </summary>
        /// <param name="center"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        internal CollisionTile(Vector2 center, int width, int height) : base(center, width, height, "")
        {
            this.OnCollisionDetected += DoCollision;
        }
        /// <summary>
        /// collision object with a collision method
        /// </summary>
        /// <param name="center"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="collisionEvent">the collision method to provide</param>
        internal CollisionTile(Vector2 center, int width, int height, collisionEvent collisionEvent) : base(center, width, height, "")
        {
            this.OnCollisionDetected += collisionEvent;
        }

        /// <summary>
        /// default collision method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="collider"></param>
        internal void DoCollision(GameObject tile, GameObject entity)
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


                    Rectangle tileBox = tile.GetBoundingBox();
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