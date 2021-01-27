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

        private Vector2 tlPosition;

        internal SpriteTile(Vector2 position, Vector2 spriteOffset, string assetName)
        {
            this.centerPosition = position;
            this.spriteOffset = spriteOffset;
            this.texture = Game.game.getSprite(assetName);

            this.tlPosition = new Vector2((int)centerPosition.X - texture.Width / 2, (int)centerPosition.Y - texture.Height / 2);
        }

        /// <summary>
        /// draw the sprite to the screen.
        /// </summary>
        /// <param name="batch"></param>
        internal void draw(SpriteBatch batch)
        {
            batch.Draw(texture, tlPosition + spriteOffset, Color.White);
        }

        internal void draw(SpriteBatch batch, Color color)
        {
            batch.Draw(texture, tlPosition + spriteOffset, color);
        }
    }
}