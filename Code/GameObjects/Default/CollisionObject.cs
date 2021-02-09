using Microsoft.Xna.Framework;
using TileGame.Code.GameObjects.Default;
using static TileGame.Code.Events.CollisionEvent;
using static TileGame.Code.Events.MouseEvent;

namespace TileGame.Code.GameObjects.Default
{
    internal class CollisionObject : GameObject
    {
        /// <summary>
        /// A collisionobject does collision, but has no collisionobjects (most of the time).
        /// </summary>
        internal override bool doesCollision { get { return false; } }

        /// <summary>
        /// creates a collision object using the default collision handler.
        /// </summary>
        /// <param name="center">center position of the collision object.</param>
        /// <param name="width">the width of the object.</param>
        /// <param name="height">the height of the object.</param>
        internal CollisionObject(Vector2 center, int width, int height) : base(center, width, height)
        {
            this.OnIntersect += _default;
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
            this.OnIntersect += collisionEvent;
        }


        
    }
}
