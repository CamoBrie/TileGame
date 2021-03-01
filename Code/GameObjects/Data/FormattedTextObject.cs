using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TileGame.Code.Data;

namespace TileGame.Code.GameObjects.Data
{
    internal class FormattedTextObject
    {
        internal string text;
        internal string trimmedText => text.Trim();
        internal float scale;
        float drawScale => scale * Settings.UIScale;
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



        public FormattedTextObject(string text, float scale, Color color,SpriteFont font, Texture2D texture = null)
        {
            this.text = text;
            this.scale = scale;
            this.color = color;
            this.texture = texture;
            this.font = font;
        }

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

        internal void Trim()
        {
            this.text = this.text.Trim();
        }

        internal void Draw(SpriteBatch batch, Vector2 drawPosition)
        {
            if (texture == null)
                batch.DrawString(font, text, drawPosition, color, 0f, Vector2.Zero, this.drawScale, SpriteEffects.None, 0f);
            else
                batch.Draw(this.texture, new Rectangle(drawPosition.ToPoint(), drawSize), Color.White);
        }
    }
}