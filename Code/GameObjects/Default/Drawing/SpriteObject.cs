﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace TileGame.Code.GameObjects.Default.Drawing
{
    internal class SpriteObject : GameObject
    {
        /// <summary>
        /// The texture of this object.
        /// </summary>
        internal Texture2D texture;

        internal SpriteObject(Vector2 center, int width, int height, string assetName, bool collides = false) : base(center, width, height, collides)
        {
            centerPosition = center;
            texture = Game.textures.Get(assetName);
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