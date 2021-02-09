using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TileGame.Code.GameObjects.Data
{
    internal class FormattedTextObject
    {
        internal string text;
        internal float scale;
        internal Color color;
        internal Texture2D texture;

        public FormattedTextObject(string text, float scale, Color color, Texture2D texture = null)
        {
            this.text = text;
            this.scale = scale;
            this.color = color;
            this.texture = texture;
        }
    }
}