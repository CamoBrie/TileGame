using Microsoft.Xna.Framework;
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
            this.onCollisionDetected += doCollision;
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
            this.onCollisionDetected += collisionEvent;
        }

        /// <summary>
        /// default collision method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="collider"></param>
        internal void doCollision(GameObject sender, GameObject collider)
        {
            //TODO: make the player not go through the wall
        }
    }
}