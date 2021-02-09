using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TileGame.Code.Animations;

namespace TileGame.Code.GameObjects.Default.Drawing
{
    internal class AnimatedObject : GameObject
    {

        ///<summary>
        /// The animationcontroller that handles the animation of the gameEntity.
        ///</summary>
        private readonly AnimationController animationController;
 
        
        /// <summary>
        /// This constructor creates an animation controller with the specified assetname
        /// </summary>
        /// <param name="center">the center position of the object.</param>
        /// <param name="width">the width of the object.</param>
        /// <param name="height">the height of the object.</param>
        /// <param name="assetName">the asset name to be used.</param>
        internal AnimatedObject(Vector2 center, int width, int height, string assetName) : base(center, width, height)
        {
            animationController = new AnimationController(assetName);
        }

        /// <summary>
        /// Plays the animation with the specified animation name and if it only needs to be played once.
        /// </summary>
        /// <param name="animationName">the name of the animation to be played.</param>
        /// <param name="playOnce">a boolean if the animation only needs to be played once.</param>
        internal void PlayAnimation(string animationName, bool playOnce) 
        {
            animationController.Play(animationName, playOnce);
        }

        /// <summary>
        /// Updates the animation controller.
        /// </summary>
        /// <param name="time">the time object.</param>
        internal override void Update(GameTime time)
        {
            animationController.Update(time);
            base.Update(time);
        }

        /// <summary>
        /// Draws the animation controller.
        /// </summary>
        /// <param name="time">the time object.</param>
        internal override void Draw(SpriteBatch batch)
        {
            animationController.Draw(batch, GetDrawPos());
            base.Draw(batch);
        }
    }
}
