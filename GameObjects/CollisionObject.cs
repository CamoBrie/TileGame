using Microsoft.Xna.Framework;
using System;
using TileGame.GameObjects;

namespace TileGame.GameObjects
{
    internal class CollisionObject : GameObject
    {
        /// <summary>
        /// creates a collision object using the default collision handler.
        /// </summary>
        /// <param name="center">center position of the collision object.</param>
        /// <param name="width">the width of the object.</param>
        /// <param name="height">the height of the object.</param>
        internal CollisionObject(Vector2 center, int width, int height) : base(center, width, height)
        {
            this.OnCollisionDetected += DoCollision;
        }
        /// <summary>
        /// creates a collision object using a custom collision handler.
        /// </summary>
        /// <param name="center">center position of the collision object.</param>
        /// <param name="width">the width of the object.</param>
        /// <param name="height">the height of the object.</param>
        /// <param name="collisionEvent">The custom collision handler.</param>
        internal CollisionObject(Vector2 center, int width, int height, collisionEvent collisionEvent) : base(center, width, height)
        {
            this.OnCollisionDetected += collisionEvent;
        }
        
        /// <summary>
        /// The default collision handler
        /// </summary>
        /// <param name="tile">the current tile.</param>
        /// <param name="entity">the entity that is colliding.</param>
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
