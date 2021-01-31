using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TileGame.GameObjects
{
    class TextObject
    {
        /// <summary>
        /// The font to be used.
        /// </summary>
        SpriteFont font;
        /// <summary>
        /// The text to be analyzed.
        /// </summary>
        string text;
        /// <summary>
        /// The list of formatted text objects.
        /// </summary>
        List<FormattedTextObject> formattedTexts = new List<FormattedTextObject>();

        internal TextObject(SpriteFont font, string text)
        {
            this.font = font;
            this.text = text;

            this.formattedTexts = Analyze(text);
        }

        /// <summary>
        /// Draws the text to the screen.
        /// </summary>
        /// <param name="batch">the spritebatch where to draw to.</param>
        /// <param name="destRect">the rectangle in which to draw the text</param>
        /// <param name="lines">the amount of lines in the rectangle.</param>
        internal void Draw(SpriteBatch batch, Rectangle destRect, int lines = 1)
        {
            Vector2 characterSize = font.MeasureString("a");
            int currentLine = 0;
            int currentCharacter = 0;

            foreach (FormattedTextObject fto in formattedTexts)
            {
                if(fto.texture != null)
                {
                    Rectangle drawRect = new Rectangle((int)(destRect.X + characterSize.X * currentCharacter), destRect.Y + destRect.Height / lines * currentLine, (int)(characterSize.Y / fto.texture.Height * fto.texture.Width), (int)characterSize.Y);
                    drawRect.Location = Camera.stw(drawRect.Location.ToVector2()).ToPoint();
                    batch.Draw(fto.texture, drawRect, fto.color);
                    currentCharacter += 2;
                } else
                {
                    if(currentCharacter + fto.text.Length > destRect.Width / (int)characterSize.X)
                    {
                        currentLine++;
                        currentCharacter = 0;
                    } else if(currentLine >= lines)
                    {
                        return;
                    } else
                    {
                        Vector2 pos = new Vector2(destRect.X + characterSize.X * currentCharacter, destRect.Y + destRect.Height / lines * currentLine);
                        Vector2 offset = fto.scale == 1 ? Vector2.Zero : new Vector2(0, font.MeasureString(fto.text).Y - font.MeasureString(fto.text).Y * fto.scale); 
                        batch.DrawString(font, fto.text, Camera.stw(pos + offset), fto.color, 0, Vector2.Zero, fto.scale, SpriteEffects.None, 0);

                        currentCharacter += (int)(fto.text.Length * fto.scale);
                    }
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
        /// Analyze the text parameter and returns a list of formatted text objects based on it.
        /// </summary>
        /// <param name="text">the text to be analyzed.</param>
        /// <returns>the list of objects.</returns>
        private List<FormattedTextObject> Analyze(string text)
        {
            List<FormattedTextObject> fto = new List<FormattedTextObject>();
            string[] st = text.Split("\\".ToCharArray());
            Color currentColor = Color.White;
            float currentScale = 1f;
            foreach(string s in st)
            {
                if(s.Length == 0)
                {
                    continue;
                }

                if(s.StartsWith("color("))
                {
                    string[] args = s.Substring(6).Split(")".ToCharArray());
                    currentColor = FromName(args[0]);
                    fto.Add(new FormattedTextObject(args[1], currentScale, currentColor));
                } 
                else if(s.StartsWith("img("))
                {
                    string path = s.Substring(4).Split(")".ToCharArray())[0];
                    fto.Add(new FormattedTextObject("", currentScale, currentColor, Game.game.GetSprite(path)));
                    if(s.Substring(4).Split(")".ToCharArray()).Length > 1)
                    {
                        fto.Add(new FormattedTextObject(s.Substring(4).Split(")".ToCharArray(), 2)[1], currentScale, currentColor));
                    }
                }
                else if (s.StartsWith("scale("))
                {
                    string[] args = s.Substring(6).Split(")".ToCharArray());
                    currentScale = float.Parse(args[0], CultureInfo.InvariantCulture.NumberFormat);
                    fto.Add(new FormattedTextObject(args[1], currentScale, currentColor));
                }
                else if (s.StartsWith("r.color"))
                {
                    currentColor = Color.White;
                    string clean = s.Substring(7);
                    if(clean != "")
                    {
                        fto.Add(new FormattedTextObject(clean, currentScale, currentColor));
                    }
                }
                else if (s.StartsWith("r.scale"))
                {
                    currentScale = 1f;
                    string clean = s.Substring(7);
                    if (clean != "")
                    {
                        fto.Add(new FormattedTextObject(clean, currentScale, currentColor));
                    }
                } else
                {
                    fto.Add(new FormattedTextObject(s, currentScale, currentColor));
                }
            }
            return fto;
        }


    }
}
