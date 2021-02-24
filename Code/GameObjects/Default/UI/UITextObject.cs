using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Globalization;
using TileGame.Code.GameObjects.Data;
using TileGame.Code.Utils;
using TileGame.Code.GameObjects.Default.Drawing;

namespace TileGame.Code.GameObjects.Default.UI
{
    class UITextObject : UIObject
    {
        TextObject textObject;

        /// <summary>
        /// TO-DO: Load font from name and have a default font, for when no font name is passed
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="text"></param>
        /// <param name="anchorMode"></param>
        /// <param name="parent"></param>
        /// <param name="font"></param>
        internal UITextObject(Rectangle pos, string text, Anchor anchorMode, UIObject parent, string fontName = "") : base(pos, anchorMode, parent)
        {
            textObject = new TextObject(fontName, text);
        }

        internal override void Draw(SpriteBatch batch)
        {
            textObject.Draw(batch, GetDrawPos(), 1);
            base.Draw(batch);
        }

    }
}
