using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TileGame.Code.Animations;

namespace TileGame.Code.GameObjects.Default.UI
{
    class UIAnimatedObject : UIObject
    {

        ///<summary>
        /// The animationcontroller that handles the animation of the object.
        ///</summary>
        internal readonly AnimationController animationController;

        /// <summary>
        /// This constructor creates an animation controller with the specified assetname
        /// </summary>
        /// <param name="pos">the relative drawing position and size.</param>
        /// <param name="assetName">the asset name to be used.</param>
        /// <param name="anchorMode">the Anchor preset used.</param>
        /// <param name="parent">the parent to anchor to</param>
        internal UIAnimatedObject(Rectangle pos, string assetName, Anchor anchorMode, UIObject parent) : base(pos, anchorMode, parent)
        {
            animationController = new AnimationController(assetName);
        }

        /// <summary>
        /// Plays the animation with the specified animation name and if it only needs to be played once.
        /// </summary>
        /// <param name="animationName">the name of the animation to be played.</param>
        /// <param name="playOnce">a boolean if the animation only needs to be played once.</param>
        internal void PlayAnimation(string animationName, bool playOnce = false)
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
