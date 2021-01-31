using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileGame.Animations
{
    class AnimationController
    {
        ///<summary>
        /// The AsepriteDocument handled by this controller.
        ///</summary>
        AsepriteDocument doc;

        /// <summary>
        /// The document storing hitboxes for the animations. 
        /// Null if the doc has no indication of having a hitbox-doc.
        /// </summary>
        AsepriteDocument hitBoxDoc;

        ///<summary>
        /// The dictionary that holds all the animations inside the doc.
        ///</summary>
        Dictionary<string, Animation> animations = new Dictionary<string, Animation>();

        ///<summary>
        /// The animation played whenever there is not a current animation, or the requested animation does not exist.
        /// This defaults to the animation under the tag starting with "DE-".
        ///</summary>
        Animation defaultAnimation;

        ///<summary>
        /// The currently selected animation to be played.
        ///</summary>
        internal Animation currentAnimation;

        /// <summary>
        /// The name of the document asset.
        /// </summary>
        string assetName;

        /// <summary>
        /// If any tag in the doc is red, it is indicated that the animation also has another doc that shows the collision animation,
        /// returning true.
        /// </summary>
        bool containsHitBoxAnimation
        {
            get
            {
                foreach (KeyValuePair<string, AsepriteTag> tag in doc.Tags)
                    if (tag.Value.Color.R == 255)
                        return true;
                return false;
            }
        }

        ///<summary>
        /// The current frame that is to be displayed on the screen.
        ///</summary>
        internal AsepriteFrame currentFrame
        {
            get
            {
                return currentAnimation.currentFrame;
            }
        }

        /// <summary>
        /// Creates a controller that handles the animations in the AsepriteDocument.
        /// </summary>
        /// <param name="doc">the AsepriteDocument that is to be handled by the controller.</param>
        internal AnimationController(string assetName)
        {
            this.doc = Game.game.GetAseDoc(assetName);
            this.assetName = assetName;
            if (doc.Tags.Count > 0)
            {
                string defaultTag = doc.Tags.Keys.Where(key => key.StartsWith("DE-")).ToList()[0];
                defaultAnimation = new Animation(ref doc, defaultTag);
                if (containsHitBoxAnimation)
                    hitBoxDoc = Game.game.GetAseDoc(assetName + "_BOX");
            }
            else
            {
                Console.WriteLine($"[ERROR] {assetName} has no tags.");
                defaultAnimation = new Animation(ref doc);
            }

        }

        /// <summary>
        /// Gets the animation from the dictionary if it exists, otherwise adds it.
        /// </summary>
        /// <param name="animationName">the animation name that is to be returned.</param>
        /// <returns>The Animation object.</returns>
        private Animation GetAnimation(string animationName)
        {
            if (animations.TryGetValue(animationName, out var ani))
            {
                Console.WriteLine($"{animationName} retrieved.");
                return ani;
            }
            else
            {
                if (doc.Tags.TryGetValue(animationName, out var tag))
                {
                    Animation animation = new Animation(ref doc, tag.Name);
                    animations.Add(tag.Name, animation);
                    return animation;
                }
                else
                {
                    Console.Error.WriteLine($"[ERROR] {animationName} animation was missing from {assetName}.");
                    return defaultAnimation;
                }
            }
        }

        /// <summary>
        /// Sets the default animation to the specified input.
        /// </summary>
        /// <param name="animationName">the name of the animation that needs to be set as default.</param>
        internal void SetDefaultAnimation(string animationName) {
            this.defaultAnimation = GetAnimation(animationName);
        }

        /// <summary>
        /// Set the animation named "animationName" as the active animation,
        /// When playOnce is true the animation switches to the default animation after being played fully
        /// </summary>
        /// <param name="animationName">the animation that needs to be set as active.</param>
        /// <param name="playOnce">the bool that determines if the animation is repeated or not.</param>
        internal void Play(string animationName, bool playOnce = false)
        {
            this.currentAnimation = GetAnimation(animationName);
        }


        #region Update/Draw

        /// <summary>
        /// updates the controller.
        /// </summary>
        /// <param name="time">the time variable.</param>
        internal void Update(GameTime time)
        {
            currentAnimation.Update(time);
        }

        /// <summary>
        /// draws the current animation.
        /// </summary>
        /// <param name="batch">the spritebatch where to draw to.</param>
        /// <param name="destRect">the destination rectangle of the sprite.</param>
        internal void Draw(SpriteBatch batch, Rectangle destRect)
        {
            Rectangle sourceRect = new Rectangle(currentFrame.X, currentFrame.Y, currentFrame.Width, currentFrame.Height);
            batch.Draw(doc.Texture, destRect, sourceRect, Color.White);
        }

        #endregion
    }
}
