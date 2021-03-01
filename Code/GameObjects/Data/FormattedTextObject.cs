using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TileGame.Code.GameObjects.Data
{
    internal class FormattedTextObject
    {
        internal string text;
        internal string trimmedText => text.Trim();
        internal float scale;
        internal Color color;
        internal Texture2D texture;
        internal SpriteFont font;

        internal Vector2 TextureSize
        {
            get
            {
                float scale = font.MeasureString("o").Y / texture.Height;
                return Vector2.Zero;
            }
        }

        internal Point drawSize;

        internal Point trimmedDrawSize;

        public FormattedTextObject(string text, float scale, Color color,SpriteFont font, Texture2D texture = null)
        {
            this.text = text;
            this.scale = scale;
            this.color = color;
            this.texture = texture;
            this.font = font;
            drawSize = GetDrawSize();
            trimmedDrawSize = GetTrimmedSize();
        }

        internal Point GetDrawSize()
        {
            if(texture == null)
            {
                return (font.MeasureString(text) * scale).ToPoint();
            }
            else
            {
                return new Point((int)(((font.MeasureString("o").Y * scale)/texture.Height) * texture.Width), (int)(font.MeasureString("o").Y * scale));
            }
        }

        internal Point GetTrimmedSize()
        {
            if (texture == null)
            {
                return (font.MeasureString(trimmedText) * scale).ToPoint();
            }
            else
            {
                return GetDrawSize();
            }
        }

        internal void Trim()
        {
            this.text = this.text.Trim();
            drawSize = GetDrawSize();
            trimmedDrawSize = GetTrimmedSize();
        }

        internal void Draw(SpriteBatch batch, Vector2 drawPosition)
        {
            if (texture == null)
                batch.DrawString(font, text, drawPosition, color, 0f, Vector2.Zero, this.scale, SpriteEffects.None, 0f);
            else
                batch.Draw(this.texture, new Rectangle(drawPosition.ToPoint(), drawSize), Color.White);
        }
    }
}