using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TileGame.Code.Data;

namespace TileGame.Code.GameObjects.Data
{
    internal class FormattedTextObject
    {
        /// <summary>
        /// The characters to display (if not a picture)
        /// </summary>
        internal string text;
        /// <summary>
        /// The characters to display whitout whitespace at the start and the end
        /// </summary>
        internal string trimmedText => text.Trim();
        /// <summary>
        /// The assigned scale of the text
        /// </summary>
        internal float scale;
        /// <summary>
        /// The scale at which to draw the text
        /// </summary>
        float drawScale => scale * Settings.UIScale;
        /// <summary>
        /// The color of the text / picture
        /// </summary>
        internal Color color;
        /// <summary>
        /// The texture to display if this is a picture
        /// </summary>
        internal Texture2D texture;
        /// <summary>
        /// The font for the text
        /// </summary>
        internal SpriteFont font;
        /// <summary>
        /// The size of the text or picture in pixels
        /// </summary>
        internal Point drawSize
        {
            get
            {
                if (texture == null)
                {
                    return (font.MeasureString(text) * drawScale).ToPoint();
                }
                else
                {
                    return new Point((int)(((font.MeasureString("o").Y * drawScale) / texture.Height) * texture.Width), (int)(font.MeasureString("o").Y * drawScale));
                }
            }
        }
        /// <summary>
        /// The size of the text or picture in pixels if it were to be trimmed
        /// </summary>
        internal Point trimmedDrawSize
        {
            get
            {
                if (texture == null)
                {
                    return (font.MeasureString(trimmedText) * drawScale).ToPoint();
                }
                else
                {
                    return drawSize;
                }
            }

        }
        /// <summary>
        /// Create a Formatted textObject that can be either a formatted string, or a picture
        /// </summary>
        /// <param name="text">The string to display</param>
        /// <param name="scale">the scale of the formatted TextObject</param>
        /// <param name="color">The colour of the formatted TextObject</param>
        /// <param name="font">The font for the string</param>
        /// <param name="texture">The texture for the picture, only pass if this is no string</param>
        public FormattedTextObject(string text, float scale, Color color,SpriteFont font, Texture2D texture = null)
        {
            this.text = text;
            this.scale = scale;
            this.color = color;
            this.texture = texture;
            this.font = font;
        }

        /// <summary>
        /// Trim the Text
        /// </summary>
        internal void Trim()
        {
            this.text = this.text.Trim();
        }

        /// <summary>
        /// Draw the fto
        /// </summary>
        /// <param name="batch">The batch to draw to</param>
        /// <param name="drawPosition">The Position to draw at</param>
        internal void Draw(SpriteBatch batch, Vector2 drawPosition)
        {
            //Draw the string correctly formatted if there is no texture
            if (texture == null)
                batch.DrawString(font, text, drawPosition, color, 0f, Vector2.Zero, this.drawScale, SpriteEffects.None, 0f);
            else
            //else draw the texture
                batch.Draw(this.texture, new Rectangle(drawPosition.ToPoint(), drawSize), Color.White);
        }
    }
}