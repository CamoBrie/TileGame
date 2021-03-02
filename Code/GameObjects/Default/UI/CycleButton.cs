using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TileGame.Code.GameObjects.Default.Drawing;
using Microsoft.Xna.Framework.Input;

namespace TileGame.Code.GameObjects.Default.UI
{
    class CycleButton<T> : UIAnimatedObject
    {
        /// <summary>
        /// The Dictionary that matches values to their labels
        /// </summary>
        internal Dictionary<T, String> displayLabels;
        /// <summary>
        /// The current index in the values, to enable cycling through values
        /// </summary>
        int currentIndex;
        /// <summary>
        /// All the values the button can return
        /// </summary>
        readonly List<T> values;
        /// <summary>
        /// The textObject that displays the label
        /// </summary>
        readonly TextObject label;
        /// <summary>
        /// The current value as determined by the current index
        /// </summary>
        internal T CurrentValue => values[currentIndex];
        /// <summary>
        /// The current label as determined by the current value
        /// </summary>
        internal string CurrentLabel => displayLabels[CurrentValue];

        /// <summary>
        /// Create a button that cycles through a bunch of labeled values when pressed
        /// </summary>
        /// <param name="pos">the relative position and size.</param>
        /// <param name="anchorMode">the Anchor preset used.</param>
        /// <param name="parent">the parent to anchor to</param>
        /// <param name="values">A list of values of any type</param>
        /// <param name="labels">A list of labels you wish to be displayed with the values</param>
        internal CycleButton(Rectangle pos, Anchor anchorMode, UIObject parent, T[] values, string[] labels) : base(pos, "views/menu/button", anchorMode, parent)
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

            this.label = new TextObject(this, "views/fonts/Sans", CurrentLabel, Color.Black, textAlignment.Center, 1f, true);

            this.OnMouseDown += ButtonHold;
            this.OnMouseUp += CyclePress;
        }

        /// <summary>
        /// Set the current value to a value stored in values, otherwise do nothing
        /// </summary>
        /// <param name="value">The value to change the button to</param>
        internal void SetCurrentValue(T value)
        {
            if (values.Contains(value))
            {
                currentIndex = values.IndexOf(value);
                this.label.ChangeTo(CurrentLabel);
            }
            else
            {
                Console.WriteLine("(CycleButton:) Value not found");
            }
        }

        /// <summary>
        /// Play an animation and cycle when the button is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="state"></param>
        private void CyclePress(GameObject sender, MouseState state)
        {
            if (currentIndex == values.Count - 1)
                currentIndex = 0;
            else
                currentIndex++;
            this.label.ChangeTo(CurrentLabel);
            Console.WriteLine(CurrentLabel + "  " + CurrentValue);
            PlayAnimation("DE-default");
        }

        /// <summary>
        /// Play an animation when the button is held down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="state"></param>
        private void ButtonHold(GameObject sender, MouseState state)
        {
            PlayAnimation("pressed");
        }
    }
}
