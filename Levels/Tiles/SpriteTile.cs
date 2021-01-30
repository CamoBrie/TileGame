using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TileGame.GameObjects;

namespace TileGame.Levels.Tiles
{
    internal class SpriteTile
    {
        internal Vector2 centerPosition;
        internal Vector2 spriteOffset;
        internal Texture2D texture;
        internal int width;
        internal int height;

        private Vector2 tlPosition;

        internal SpriteTile(Vector2 position, Vector2 spriteOffset, string assetName)
        {
            this.centerPosition = position;
            this.spriteOffset = spriteOffset;
            this.texture = Game.game.getSprite(assetName);

            this.width = texture.Width;
            this.height = texture.Height;

            this.tlPosition = new Vector2((int)centerPosition.X - texture.Width / 2, (int)centerPosition.Y - texture.Height / 2);
        }

        /// <summary>
        /// gets the sprite's bounding box.
        /// </summary>
        /// <returns></returns>
        internal Rectangle getBoundingBox()
        {
            return new Rectangle((int)this.centerPosition.X + (int)this.spriteOffset.X - this.width / 2, (int)this.centerPosition.Y + (int)this.spriteOffset.Y - this.height / 2, this.width, this.height);
        }

        /// <summary>
        /// draw the sprite to the screen.
        /// </summary>
        /// <param name="batch"></param>
        internal void draw(SpriteBatch batch)
        {
            batch.Draw(texture, tlPosition + spriteOffset, Color.White);
        }

        /// <summary>
        /// draw the sprite to the screen with the provided color
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="color">the color in which to draw the sprite in.</param>
        internal void draw(SpriteBatch batch, Color color)
        {
            batch.Draw(texture, tlPosition + spriteOffset, color);
        }
    }
}