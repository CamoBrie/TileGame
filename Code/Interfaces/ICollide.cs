using Microsoft.Xna.Framework;
using static TileGame.Code.Events.CollisionEvent;

namespace TileGame.Code.Interfaces
{
    internal interface ICollide
    {
        Rectangle BoundingBox { get; set; }
        event collisionEvent OnIntersect;
    }
}
