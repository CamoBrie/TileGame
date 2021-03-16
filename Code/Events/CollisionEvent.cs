using Microsoft.Xna.Framework;
using System;
using TileGame.Code.GameObjects;
using TileGame.Code.GameObjects.Default;

namespace TileGame.Code.Events
{
    internal static class CollisionEvent
    {
        /// <summary>
        /// The event that is fired on a collision event.
        /// </summary>
        /// <param name="sender">sender of the collision.</param>
        /// <param name="collider">collider of the collision.</param>
        internal delegate void collisionEvent(GameObject sender, GameObject collider);

        internal static void _default(GameObject self, GameObject other)
        {
            if (other is GameEntity otherEntity)
            {

                // Calculate current and minimum-non-intersecting distances between centers.
                float distanceX = self.centerPosition.X - other.centerPosition.X;
                float distanceY = self.centerPosition.Y - other.centerPosition.Y;
                float minDistanceX = self.width / 2 + other.width / 2;
                float minDistanceY = self.height / 2 + other.height / 2;

                float depthX = distanceX > 0 ? minDistanceX - distanceX : -minDistanceX - distanceX;
                float depthY = distanceY > 0 ? minDistanceY - distanceY : -minDistanceY - distanceY;


                Rectangle tileBox = self.GetBoundingBox();
                if (Math.Abs((int)depthY) > Math.Abs((int)depthX))
                {
                    // Collision on the X axis
                    if (depthX > 0)
                    {
                        // Collision on entity right
                        other.centerPosition = new Vector2(tileBox.Left - (other.width / 2) - 2, other.centerPosition.Y);
                    }
                    else
                    {
                        // Collision on entity left
                        other.centerPosition = new Vector2(tileBox.Right + (other.width / 2) + 2, other.centerPosition.Y);
                    }

#if DEBUG
                    if (otherEntity is Player player)
                        Console.WriteLine(self.centerPosition + "  X        Player depth: " + depthX + "," + depthY + "   Vel: " + player.velocity);
#endif
                    otherEntity.velocity.X = 0;
                }
                else if (Math.Abs((int)depthX) > Math.Abs((int)depthY))
                {
                    // Collision on the Y axis
                    if (depthY > 0)
                    {
                        // Collision on entity bottom
                        other.centerPosition = new Vector2(other.centerPosition.X, tileBox.Top - other.height / 2 - 2);
                    }
                    else
                    {
                        // Collision on entity top
                        other.centerPosition = new Vector2(other.centerPosition.X, tileBox.Bottom + other.height / 2 + 2);
                    }
#if DEBUG
                    if (otherEntity is Player player)
                        Console.WriteLine(self.centerPosition + "  Y        Player distance: " + depthX + "," + depthY + "   Vel:    " + player.velocity);
#endif
                    otherEntity.velocity.Y = 0;
                }
            }

        }
    }


}
