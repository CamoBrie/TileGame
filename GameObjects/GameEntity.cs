using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileGame.GameObjects
{
    class GameEntity : GameObject
    {
        /// <summary>
        /// this float represents the base speed of the entity.
        /// </summary>
        internal float baseSpeed = 5;

        /// <summary>
        /// this float represents the maximum speed of the entity.
        /// </summary>
        internal Vector2 maxSpeed = new Vector2(10);

        /// <summary>
        /// this float represents the friction with the ground.
        /// </summary>
        internal float friction = 0.6f;

        /// <summary>
        /// This vector2 represents the direction the object can move in.
        /// </summary>
        internal Vector2 velocity = new Vector2(0, 0);

        /// <summary>
        /// this vector2 represents the acceleration of the object.
        /// For stationary game entities, this can always remain zero.
        /// </summary>
        internal Vector2 acceleration = new Vector2(0, 0);

        /// <summary>
        /// this vector keeps track of the distance moved last update cycle.
        /// </summary>
        internal Vector2 movedPosition = new Vector2(0, 0);

        /// <summary>
        /// This constant makes sure that we can express "velocity" in terms of pixel per second.
        /// This conversion factor is based on the current updatespeed. So if you change the update speed, you need to change this also.
        /// </summary>
        public readonly float velocityScale = 16.6667f;

        internal GameEntity(Vector2 center, int width, int height, string assetName) : base(center, width, height, assetName)
        {
        }

        /// <summary>
        /// This function overrides the basic update function.
        /// First it calls the base function. So that all its children are updated and whatnot.
        /// Then it specifically updates the entity, to include its speed.
        /// </summary>
        /// <param name="time"> The game time, handed down by the controller. </param>
        internal override void Update(GameTime time)
        {
            base.Update(time);
            this.MoveEntity(time);
        }

        /// <summary>
        /// This function is the base code for moving an entity.
        /// </summary>
        /// <param name="time">The game time, handed down by the controller. </param>
        protected virtual void MoveEntity(GameTime time)
        {
            this.acceleration *= this.baseSpeed;
            this.velocity += this.acceleration;
            this.velocity *= friction;

            this.velocity = Vector2.Clamp(velocity, -maxSpeed, maxSpeed);

            this.movedPosition = velocity * this.velocityScale / (float)time.ElapsedGameTime.TotalMilliseconds;
            this.centerPosition += velocity * this.velocityScale / (float)time.ElapsedGameTime.TotalMilliseconds;
            this.acceleration = Vector2.Zero;
        }
    }
}
