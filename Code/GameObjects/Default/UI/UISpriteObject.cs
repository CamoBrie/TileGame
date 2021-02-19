using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using TileGame.Code.Utils;
using TileGame.Code.Utils.Convenience;
using TileGame.Code.GameObjects.Default.Drawing;
using static TileGame.Code.Events.CollisionEvent;
using static TileGame.Code.Events.MouseEvent;

namespace TileGame.Code.GameObjects.Default.UI
{
    class UISpriteObject : UIObject
    {
        /// <summary>
        /// The texture of this object.
        /// </summary>
        internal Texture2D texture;

        /// <summary>
        /// Creates A UiObject with a sprite.
        /// </summary>
        /// <param name="pos">the relative drawing position and size.</param>
        /// <param name="assetName">the asset name to be used.</param>
        /// <param name="anchorMode">the Anchor preset used.</param>
        /// <param name="parent">the parent to anchor to</param>
        internal UISpriteObject(Rectangle pos, string assetName, Anchor anchorMode, UIObject parent) : base(pos, anchorMode, parent)
        {
            texture = Game.game.GetSprite(assetName);
        }

        #region Drawing functions
        /// <summary>
        /// Draws the texture to the batch without any color changes.
        /// </summary>
        /// <param name="batch">The batch to draw to.</param>
        internal override void Draw(SpriteBatch batch)
        {
            batch.Draw(texture, GetDrawPos(), Color.White);
            base.Draw(batch);
        }
        #endregion
    }
}
