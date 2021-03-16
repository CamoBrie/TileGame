using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Globalization;
using TileGame.Code.GameObjects.Data;

namespace TileGame.Code.GameObjects.Default.Drawing
{
    internal class TextObject : GameObject
    {
        /// <summary>
        /// The (default)font to be used.
        /// </summary>
        private readonly SpriteFont font;
        /// <summary>
        /// The (default)color
        /// </summary>
        private Color color = Color.Black;
        /// <summary>
        /// The way to align the text in the drawfield (left, right, or center)
        /// </summary>
        internal textAlignment alignment;
        /// <summary>
        /// The (default)scale
        /// </summary>
        internal float scale = 1.0f;
        /// <summary>
        /// Whether to crop text that would flow Outside the drawfield
        /// </summary>
        internal bool cropText;
        /// <summary>
        /// Whether the parent is a UIObject, relevent for IE Applying UIScale
        /// </summary>
        private readonly bool isUI;

        /// <summary>
        /// Get the drawPos, depending on whether the parent is a UIOBject
        /// </summary>
        /// <returns></returns>
        internal override Rectangle GetDrawPos()
        {
            if (!isUI)
                return base.GetDrawPos();
            else
                return (parent as UIObject).GetDrawPos();
        }

        /// <summary>
        /// Get the Offset for a line, depending on the alignment
        /// </summary>
        /// <param name="lineSize">The Size of the line</param>
        /// <returns></returns>
        Vector2 GetAlignmentOffSet(Point lineSize)
        {
            float x = 0;
            switch (alignment)
            {
                case textAlignment.Right:
                    x = GetDrawPos().Width - lineSize.X;
                    break;
                case textAlignment.Center:
                    x = (GetDrawPos().Width / 2) - (lineSize.X / 2);
                    break;
                default:
                    break;
            }
            return new Vector2(x, 0);
        }

        /// <summary>
        /// The list of formatted text objects.
        /// </summary>
        private List<FormattedTextObject> formattedTextObjects = new List<FormattedTextObject>();

        /// <summary>
        /// A List of the Lines (Lists of ftos)
        /// </summary>
        List<List<FormattedTextObject>> lines = new List<List<FormattedTextObject>>();

        /// <summary>
        /// Create a textobject, that formats and displays the text. See doc for Formatting commands.
        /// </summary>
        /// <param name="parent">The gameObject to attach the text to</param>
        /// <param name="fontName">the name of the font (can be altered with Formatting Commands)</param>
        /// <param name="text">The text incl. formatting commands to display</param>
        /// <param name="color">The color of the text (can be altered with Formatting Commands)</param>
        /// <param name="alignment">The textAlignment (Left, Right, Center)</param>
        /// <param name="scale">The Size of the text (can be altered with Formatting Commands)</param>
        /// <param name="cropText">Whether to crop the text to only show text that fits inside the drawfield</param>
        internal TextObject(GameObject parent, string fontName, string text, Color color, textAlignment alignment = textAlignment.Left, float scale = 1.0f, bool cropText = true) : base(parent)
        {
            this.font = Game.fonts.Get(fontName);
            this.alignment = alignment;
            this.color = color;
            this.scale = scale;
            this.cropText = cropText;
            isUI = parent as UIObject != null;
            apply(text);
        }
        /// <summary>
        /// Convert the text to ftos and lines
        /// </summary>
        /// <param name="text"></param>
        void apply(string text)
        {
            formattedTextObjects = Analyze(text);
            lines = GenerateLines();
        }
        /// <summary>
        /// Change the text
        /// </summary>
        /// <param name="text"></param>
        internal void ChangeTo(string text)
        {
            apply(text);
        }

        /// <summary>
        /// Fit all fto's in a list of lines(a list of fto's) 3
        /// </summary>
        /// <returns></returns>
        List<List<FormattedTextObject>> GenerateLines()
        {
            //Create Lines list to return
            List<List<FormattedTextObject>> lines = new List<List<FormattedTextObject>>();
            //Create copy of the list of all the FTO's to move the items over to the lines
            List<FormattedTextObject> remainingFTOs = new List<FormattedTextObject>(formattedTextObjects);
            //While FTO's still need to be added from the remaining FTO's, add new lines and fill them
            for(int i = 0; remainingFTOs.Count > 0; i++)
            {
                //Add a new Line
                lines.Add(new List<FormattedTextObject>());
                //Reset the current Line length
                float currentLineLength = 0f;

                //In case the FTOs length is larger then the whole line, add it and move to the next line. (The FTO doesnt fit in a line, even by itself)
                if(remainingFTOs[0].trimmedDrawSize.X > GetDrawPos().Width)
                {
                    //Add the FTO to the line
                    lines[i].Add(remainingFTOs[0]);
                    //Remove the FTO from the remaining FTOs
                    remainingFTOs.RemoveAt(0);
                    //continue to the next line
                    continue;
                }

                //While FTO's still need to be added from the remaining FTO's, add fto's to the current line, unless it passes the line length
                while (remainingFTOs.Count > 0)
                {
                    //Check if the next FTO can be added, considering line length. If so add the FTO, otherwise switch to the next line
                    if((currentLineLength + remainingFTOs[0].drawSize.X < GetDrawPos().Width))
                    {
                        //Trim if first in line (so the line doesn't start with whitespace)
                        if (lines[i].Count == 0)
                            remainingFTOs[0].Trim();

                        //Update the currentline length (length of all currently present FTOs in current line)
                        currentLineLength += remainingFTOs[0].drawSize.X;
                        //Add the FTO to the line
                        lines[i].Add(remainingFTOs[0]);
                        //Remove the FTO from the remaining FTOs
                        remainingFTOs.RemoveAt(0);
                    }
                    else
                    {
                        //If the FTO can't be added, break from the FTO adding loop and continue to the next line
                        break;
                    }
                }
            }
            //Return the lines
            return lines;
        }

        

        /// <summary>
        /// Draws the text to the screen.
        /// </summary>
        /// <param name="batch">the spritebatch where to draw to.</param>
        internal override void Draw(SpriteBatch batch)
        {
            if (active)
            {
                float yPos = 0f;
                for (int y = 0; y < lines.Count; y++)
                {
                    if (cropText && (yPos + GetSizeOfLine(lines[y]).Y > GetDrawPos().Height))
                        break;

                    float xPos = 0f;
                    for (int x = 0; x < lines[y].Count; x++)
                    {
                        lines[y][x].Draw(batch, new Vector2(xPos + GetDrawPos().Left, yPos + GetDrawPos().Top) + GetAlignmentOffSet(GetSizeOfLine(lines[y])));
                        xPos += lines[y][x].drawSize.X;
                    }
                    int LineHeight = 0;
                    foreach (FormattedTextObject fto in lines[y])
                        if (fto.drawSize.Y > LineHeight)
                            LineHeight = fto.drawSize.Y;
                    yPos += LineHeight;
                }
            }
        }

        /// <summary>
        /// Gets the color object from the parameter.
        /// </summary>
        /// <param name="colorName">the name of the color.</param>
        /// <returns>the color object</returns>
        private static Color FromName(string colorName)
        {
            System.Drawing.Color systemColor = System.Drawing.Color.FromName(colorName);
            return new Color(systemColor.R, systemColor.G, systemColor.B, systemColor.A); //Here Color is Microsoft.Xna.Framework.Graphics.Color
        }

        /// <summary>
        /// Measure the size of a line in pixels
        /// </summary>
        /// <param name="line">The line to measure</param>
        /// <returns>the size of the line in pixels</returns>
        Point GetSizeOfLine(List<FormattedTextObject> line)
        {
            Point size = Point.Zero;
            foreach (FormattedTextObject fto in line)
            {
                size.X += fto.drawSize.X;
                if (fto.drawSize.Y > size.Y)
                    size.Y = fto.drawSize.Y;
            }
            return size;
        }

        /// <summary>
        /// Analyze the text parameter and returns a list of formatted text objects based on it.
        /// </summary>
        /// <param name="text">the text to be analyzed.</param>
        /// <returns>the list of objects.</returns>
        private List<FormattedTextObject> Analyze(string text)
        {
            List<FormattedTextObject> fto = new List<FormattedTextObject>();
            string[] words = text.Split(' ');
            Color currentColor = this.color;
            float currentScale = this.scale;
            SpriteFont currentFont = this.font;
            for (int i = 0; i < words.Length; i++)
            {
                if (i != 0)
                    words[i] = " " + words[i];

                string[] stringObjects = words[i].Split(new char[] { '\\' });
                foreach(string s in stringObjects)
                {
                    if (s.Length == 0)
                    {
                        continue;
                    }

                    if (s.StartsWith("color("))
                    {
                        string[] args = s.Substring(6).Split(")".ToCharArray());
                        currentColor = FromName(args[0]);
                        fto.Add(new FormattedTextObject(args[1], currentScale, currentColor, currentFont));
                    }
                    else if (s.StartsWith("img("))
                    {
                        string path = s.Substring(4).Split(")".ToCharArray())[0];
                        fto.Add(new FormattedTextObject("", currentScale, currentColor, currentFont, Game.textures.Get(path)));
                        if (s.Substring(4).Split(")".ToCharArray()).Length > 1)
                        {
                            fto.Add(new FormattedTextObject(s.Substring(4).Split(")".ToCharArray(), 2)[1], currentScale, currentColor, currentFont));
                        }
                    }
                    else if (s.StartsWith("scale("))
                    {
                        string[] args = s.Substring(6).Split(")".ToCharArray());
                        currentScale = float.Parse(args[0], CultureInfo.InvariantCulture.NumberFormat) * this.scale;
                        fto.Add(new FormattedTextObject(args[1], currentScale, currentColor, currentFont));
                    }
                    else if (s.StartsWith("r.color"))
                    {
                        currentColor = this.color;
                        string clean = s.Substring(7);
                        if (clean != "")
                        {
                            fto.Add(new FormattedTextObject(clean, currentScale, currentColor, currentFont));
                        }
                    }
                    else if (s.StartsWith("r.scale"))
                    {
                        currentScale = this.scale;
                        string clean = s.Substring(7);
                        if (clean != "")
                        {
                            fto.Add(new FormattedTextObject(clean, currentScale, currentColor, currentFont));
                        }
                    }
                    else
                    {
                        fto.Add(new FormattedTextObject(s, currentScale, currentColor, currentFont));
                    }
                }
            }
            return fto;
        }
    }

    internal enum textAlignment { Left, Center, Right}
}
