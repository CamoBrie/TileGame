using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TileGame.Code.Animations;
using TileGame.Code.GameObjects.Default.Drawing;
using Microsoft.Xna.Framework.Input;

namespace TileGame.Code.GameObjects.Default.UI
{
    class CycleButton<T> : UIAnimatedObject
    {
        Dictionary<T, String> displayLabels;

        int currentIndex;

        List<T> values;

        TextObject textObject;

        internal T CurrentValue => values[currentIndex];

        internal string CurrentLabel => displayLabels[CurrentValue];

        internal CycleButton(Point pos, Anchor anchorMode, UIObject parent, T[] values, string[] labels) : base(new Rectangle(pos, new Point(128, 32)), "views/menu/button", anchorMode, parent)
        {
            currentIndex = 0;
            if (values.Length != labels.Length)
                Console.WriteLine("!!Warning!!: CycleButton values and labels do not correspond!");

            this.values = new List<T>();
            displayLabels = new Dictionary<T, string>();
            for(int i = 0; i < values.Length; i++)
            {
                try
                {
                    displayLabels.Add(values[i], labels[i]);
                    this.values.Add(values[i]);
                }
                catch (IndexOutOfRangeException)
                {
                    try
                    {
                        Console.Write("value: " + values[i] + ", ");
                    }
                    catch (IndexOutOfRangeException)
                    {
                        Console.Write("label: " + labels[i] + ", ");
                    }

                }
            }
            if (values.Length != labels.Length)
                Console.WriteLine("have not been Added");

            this.textObject = new TextObject("views/fonts/Sans", CurrentLabel, this.GetDrawPos());
            this.OnMouseDown += ButtonHold;
            this.OnMouseUp += CyclePress;
        }

        private void CyclePress(GameObject sender, MouseState state)
        {
            SetTimer(0.001f, Cycle);
            PlayAnimation("pressed");
        }

        private void ButtonHold(GameObject sender, MouseState state)
        {
            PlayAnimation("pressed");
        }

        

        void Cycle()
        {
            PlayAnimation("DE-default");
            if (currentIndex == values.Count - 1)
                currentIndex = 0;
            else
                currentIndex++;
            textObject = new TextObject("views/fonts/Sans", CurrentLabel, GetDrawPos());
        }

        internal override void Draw(SpriteBatch batch)
        {
            base.Draw(batch);
            textObject.Draw(batch, 1);
        }

    }
}
