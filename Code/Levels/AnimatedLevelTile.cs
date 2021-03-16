using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TileGame.Code.Animations;
using TileGame.Code.GameObjects.Default;
using TileGame.Code.GameObjects.Default.Drawing;
using TileGame.Code.Levels;
using TileGame.Code.Levels.Tiles;
using TileGame.Code.Utils.Convenience;
using static TileGame.Code.Events.CollisionEvent;

namespace TileGame.Levels
{
    internal class AnimatedLevelTile : LevelTile
    {
        ///<summary>
        /// The animationcontroller that handles the animation of the object.
        ///</summary>
        internal readonly AnimationController animationController;

        /// <summary>
        /// Default colliding tile constructor
        /// </summary>
        /// <param name="positionInGrid"></param>
        internal AnimatedLevelTile(LevelGrid grid, Point positionInGrid, int tileSize, string assetName) : base(grid, positionInGrid, tileSize)
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
            if (active)
                animationController.Update(time);
            base.Update(time);
        }

        /// <summary>
        /// Draws the animation controller.
        /// </summary>
        /// <param name="time">the time object.</param>
        internal override void Draw(SpriteBatch batch)
        {
            if (active)
                animationController.Draw(batch, GetDrawPos());
            base.Draw(batch);
        }
    }
}
