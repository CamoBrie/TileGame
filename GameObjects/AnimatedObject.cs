using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileGame.GameObjects;
using MonoGame.Aseprite.Documents;
using MonoGame.Aseprite.Graphics;
using TileGame.Animations;

namespace TileGame.GameObjects
{
    class AnimatedObject : GameObject
    {

        ///<summary>
        /// The animationcontroller that handles the animation of the gameEntity.
        ///</summary>
        AnimationController animationController;
 
        

        internal AnimatedObject(Vector2 center, int width, int height, string assetName) : base(center, width, height)
        {
            this.animationController = new AnimationController(Game.game.GetAseDoc(assetName));
        }

        internal void PlayAnimation(string animationName, bool playOnce) 
        {
            animationController.Play(animationName, playOnce);
        }

        internal override void Update(GameTime time)
        {
            animationController.Update(time);
            base.Update(time);
        }

        internal override void Draw(SpriteBatch batch)
        {
            animationController.Draw(batch, GetDrawPos());
            base.Draw(batch);
        }
    }
}
