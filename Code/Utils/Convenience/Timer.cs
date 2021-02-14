using Microsoft.Xna.Framework;
using System;
using TileGame.Code.GameObjects.Default;

namespace TileGame.Code.Utils.Convenience
{
    internal class Timer : GameObject
    {
        /// <summary>
        /// The time remaining on the timer.
        /// You can add or subtract time from this variable if you want to artificially change the duration of the timer.
        /// </summary>
        internal float remainingTime;

        /// <summary>
        /// The total length of the timer.
        /// </summary>
        internal float timerLength;

        /// <summary>
        /// The action to perform.
        /// </summary>
        private readonly Action method;

        /// <summary>
        /// Should the timer repeat.
        /// </summary>
        private readonly bool repeat;

        /// <summary>
        /// Returns true if the timer has reached zero.
        /// </summary>
        internal bool ReachedEnd => remainingTime <= 0.0f;

        /// <summary>
        /// If destroyWhenDone is enabled, the timer will remove its child reference from its parent when it reached the end.
        /// </summary>
        private readonly bool destroyWhenDone;

        private bool finished = false;

        internal Timer()
        {
            remainingTime = 0;
            timerLength = 0;
            finished = true;
        }
        internal Timer(float duration, bool destroyWhenDone = false)
        {
            remainingTime = duration;
            timerLength = duration;
            this.destroyWhenDone = destroyWhenDone;
        }
        internal Timer(float duration, Action method, bool destroyWhenDone = false)
        {
            remainingTime = duration;
            timerLength = duration;
            this.method = method;
            this.destroyWhenDone = destroyWhenDone;
        }

        /// <summary>
        /// Create a timer with a certain duration that gets updated by its parent.
        /// </summary>
        /// <param name="parent">The parentobject of the timer</param>
        /// <param name="duration">The duration of the timer</param>
        /// <param name="destroyWhenDone">If set to true, the timer will remove its child reference from its parent when there is no time remaining</param>
        internal Timer(GameObject parent, float duration, bool destroyWhenDone = false) : base(parent)
        {
            remainingTime = duration;
            timerLength = duration;
            this.destroyWhenDone = destroyWhenDone;
        }

        /// <summary>
        /// Create a timer that performs an Action after the duration, or performs an action at an interval.
        /// </summary>
        /// <param name="parent">The parentObject of the timer</param>
        /// <param name="duration">The duration before the action is performed, or the interval</param>
        /// <param name="method">The action to perform</param>
        /// <param name="repeat">If the action should repeat at intervals</param>
        internal Timer(GameObject parent, float duration, Action method, bool repeat = false) : base(parent)
        {
            remainingTime = duration;
            timerLength = duration;
            destroyWhenDone = true;
            this.method = method;
            this.repeat = repeat;
        }

        /// <summary>
        /// Update the remaining time.
        /// </summary>
        /// <param name="time"></param>
        internal override void Update(GameTime time)
        {
            if (ReachedEnd && !finished)
            {
                method?.Invoke();
                if (destroyWhenDone)
                {
                    if (repeat)
                    {
                        remainingTime = timerLength;
                    }
                    else
                    {
                        parent.children.Remove(this);
                    }
                }
                else
                {
                    remainingTime = 0.0f;
                    finished = true;
                } 
            }
            base.Update(time);
            remainingTime -= (float)time.ElapsedGameTime.TotalSeconds;
        }

    }
}
