using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using TileGame.Code.GameObjects.Default;

namespace TileGame.Code.Utils.Convenience
{
    class Timer : GameObject
    {
        /// <summary>
        /// The time remaining on the timer.
        /// You can add or subtract time from this variable if you want to artificially change the duration of the timer.
        /// </summary>
        internal float remainingTime;

        /// <summary>
        /// Returns true if the timer has reached zero
        /// </summary>
        internal bool ReachedEnd => remainingTime <= 0.0f;

        /// <summary>
        /// If destroyWhenDone is enabled, the timer will remove its child reference from its parent when it reached the end
        /// </summary>
        private readonly bool destroyWhenDone;

        /// <summary>
        /// Create a timer with a certain duration that gets updated by its parent
        /// </summary>
        /// <param name="parent">The parentobject of the timer</param>
        /// <param name="time">The duration of the timer</param>
        /// <param name="destroyWhenDone">If set to true, the timer will remove its child reference from its parent when there is no time remaining</param>
        internal Timer(GameObject parent, float time, bool destroyWhenDone = false) : base(parent)
        {
            remainingTime = time;
            this.destroyWhenDone = destroyWhenDone;
        }

        /// <summary>
        /// Update the remaining time.
        /// </summary>
        /// <param name="time"></param>
        internal override void Update(GameTime time)
        {
            if (ReachedEnd && destroyWhenDone)
                parent.children.Remove(this);
            base.Update(time);
            remainingTime -= (float)time.ElapsedGameTime.TotalSeconds;
        }
    }
}
