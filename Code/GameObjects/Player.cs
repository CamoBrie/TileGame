using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TileGame.Code.GameObjects.Default;
using System;
using TileGame.Code.GameObjects.Default.Drawing;
using TileGame.Code.Utils.Convenience;

namespace TileGame.Code.GameObjects
{
    internal class Player : GameEntity
    {
        internal Player(Vector2 center, int width, int height, string assetName) : base(center, width, height, assetName)
        {
            this.collides = true;
        }

        internal override void Update(GameTime time)
        {
            base.Update(time);

        }

        /// <summary>
        /// handle the input provided by the controller
        /// </summary>
        /// <param name="intentDir"></param>
        internal void HandleInput(Vector2 intentDir)
        {
            acceleration = intentDir;
        }

        internal override void Draw(SpriteBatch batch)
        {
            base.Draw(batch);
        }
    }
}
