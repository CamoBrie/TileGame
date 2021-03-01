using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Globalization;
using TileGame.Code.GameObjects.Data;
using TileGame.Code.Utils;
using System;
using TileGame.Code.Data;

namespace TileGame.Code.GameObjects.Default.Drawing
{
    internal class TextObject
    {
        /// <summary>
        /// The font to be used.
        /// </summary>
        private readonly SpriteFont font;

        private Color color = Color.Black;

        internal textAlignment alignment;

        internal float scale = 1.0f;

        Rectangle pos;

        Rectangle drawField
        {
            get
            {
                return pos;
                //return new Rectangle((int)(pos.X - (pos.Width * Settings.UIScale) / 2), (int)(pos.Y - (pos.Height * Settings.UIScale) / 2), (int)(pos.Width * Settings.UIScale), (int)(pos.Height * Settings.UIScale));
            }
        }

        internal bool ScaleToFitText;

        Vector2 GetAlignmentOffSet(Point lineSize)
        {
            float x = 0;
            switch (alignment)
            {
                case textAlignment.Right:
                    x = drawField.Width - lineSize.X;
                    break;
                case textAlignment.Center:
                    x = (drawField.Width / 2) - (lineSize.X / 2);
                    break;
                default:
                    break;
            }
            return new Vector2(x, 0);
        }


        /// <summary>
        /// The text to be analyzed.
        /// </summary>
        private readonly string text;

        /// <summary>
        /// The list of formatted text objects.
        /// </summary>
        private readonly List<FormattedTextObject> formattedTextObjects = new List<FormattedTextObject>();

        List<List<FormattedTextObject>> lines = new List<List<FormattedTextObject>>();

        internal TextObject(string fontName, string text, Rectangle drawField, textAlignment alignment = textAlignment.Left)
        {
            this.font = Game.fonts.Get(fontName);
            this.alignment = alignment;
            this.text = text;
            this.pos = drawField;
            this.ScaleToFitText = false;
            formattedTextObjects = Analyze(text);
            lines = GenerateLines();
            //foreach(List<FormattedTextObject> L in lines)
            //{
            //    foreach(FormattedTextObject fto in L)
            //    {
            //        Console.Write(fto.text);
            //    }
            //    Console.WriteLine();
            //}
        }

        internal TextObject(GameObject parent, string fontName, string text, Rectangle drawField, Color color, textAlignment alignment = textAlignment.Left, float scale = 1.0f, bool scaleToFitToText = false)
        {
            this.font = Game.fonts.Get(fontName);
            this.alignment = alignment;
            this.color = color;
            this.scale = scale;
            this.text = text;
            this.pos = drawField;
            this.ScaleToFitText = scaleToFitToText;
            formattedTextObjects = Analyze(text);
            lines = GenerateLines();
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
                if(remainingFTOs[0].trimmedDrawSize.X > drawField.Width)
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
                    if((currentLineLength + remainingFTOs[0].drawSize.X < drawField.Width))
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
        /// <param name="destRect">the rectangle in which to draw the text</param>
        /// <param name="lines">the amount of lines in the rectangle.</param>
        internal void Draw(SpriteBatch batch)
        {
            float yPos = 0f;
            for(int y = 0; y < lines.Count; y++)
            {
                if (!ScaleToFitText && (yPos + GetSizeOfLine(lines[y]).Y > drawField.Height))
                    break;

                float xPos = 0f;
                for(int x = 0; x < lines[y].Count; x++)
                {
                    lines[y][x].Draw(batch, new Vector2(xPos + drawField.Left, yPos + drawField.Top) + GetAlignmentOffSet(GetSizeOfLine(lines[y])));
                    xPos += lines[y][x].drawSize.X;
                }
                int LineHeight = 0;
                foreach(FormattedTextObject fto in lines[y])
                    if (fto.drawSize.Y > LineHeight)
                        LineHeight = fto.drawSize.Y;
                yPos += LineHeight;
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
