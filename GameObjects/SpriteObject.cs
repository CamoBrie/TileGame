    using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TileGame.GameObjects;

namespace TileGame.GameObjects
{
    internal class SpriteObject : GameObject
    {
        internal Texture2D texture;

        internal SpriteObject(Vector2 center, int width, int height, string assetName) : base(center, width, height)
        {
            this.centerPosition = center;
            this.texture = Game.game.GetSprite(assetName);
        }

        #region Drawing functions
        internal override void Draw(SpriteBatch batch)
        {
            batch.Draw(texture, GetBoundingBox(), Color.White);
        }

        internal void Draw(SpriteBatch batch, Color color)
        {
            batch.Draw(texture, GetBoundingBox(), color);
        }
        #endregion
    }
}