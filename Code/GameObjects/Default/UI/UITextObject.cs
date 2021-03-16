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
        /// <summary>
        /// The TextObject used to display the text
        /// </summary>
        protected TextObject textObject;

        /// <summary>
        /// Create a UIObject with text
        /// </summary>
        /// <param name="pos">the relative position and size.</param>
        /// <param name="anchorMode">the Anchor preset used.</param>
        /// <param name="parent">the parent to anchor to</param>
        /// <param name="text">The text to display (See TextObject doc for TextObject formatting Commands)</param>
        /// <param name="textColor">The colour of the text</param>
        /// <param name="fontName">The path to the font for the text</param>
        /// <param name="alignment">The TextAlingment (Left, right, or center)</param>
        /// <param name="textScale">The Scale of the text</param>
        /// <param name="cropText">Whether to crop the displayed text to fit in this Object when drawing</param>
        internal UITextObject(Rectangle pos, Anchor anchorMode, UIObject parent, string text, Color textColor, string fontName = "", textAlignment alignment = textAlignment.Left, float textScale = 1.0f, bool cropText = true) : base(pos, anchorMode, parent)
        {
            textObject = new TextObject(this, fontName, text, textColor, alignment, textScale, cropText);
        }

        internal UITextObject(UIObject parent, string text, Color textColor, string fontName = "", textAlignment alignment = textAlignment.Left, float textScale = 1.0f, bool cropText = true) : base(new Rectangle(0,0, parent.width, parent.height), Anchor.TopLeft, parent)
        {
            textObject = new TextObject(this, fontName, text, textColor, alignment, textScale, cropText);
        }
    }
}
