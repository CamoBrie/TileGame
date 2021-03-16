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
        /// The buttons that make the cyclebutton cycle when pressed
        /// </summary>
        internal UIButton leftButton, rightButton;

        /// <summary>
        /// Create a button that cycles through a bunch of labeled values when pressed
        /// </summary>
        /// <param name="pos">the relative position and size.</param>
        /// <param name="anchorMode">the Anchor preset used.</param>
        /// <param name="parent">the parent to anchor to</param>
        /// <param name="values">A list of values of any type</param>
        /// <param name="labels">A list of labels you wish to be displayed with the values</param>
        internal CycleButton(Rectangle pos, Anchor anchorMode, UIObject parent, T[] values, string[] labels) : base(pos, "views/menu/wood_button_4", anchorMode, parent)
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

            this.label = new TextObject(this, "views/fonts/Sans", CurrentLabel, Color.White, textAlignment.Center, 1f, true);

            leftButton = new UIButton(new Rectangle(-height, 0, height, height), "views/menu/wood_button_left", "", Anchor.TopLeft, this);
            rightButton = new UIButton(new Rectangle(0, 0, height, height), "views/menu/wood_button_right", "", Anchor.TopRight, this);

            leftButton.OnMouseUp += LeftButton_OnMouseUp;
            rightButton.OnMouseUp += RightButton_OnMouseUp;
        }

        /// <summary>
        /// Increment the index and update the labels when the right button is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="state"></param>
        private void RightButton_OnMouseUp(GameObject sender, MouseState state)
        {
            if (currentIndex == values.Count - 1)
                currentIndex = 0;
            else
                currentIndex++;
            this.label.ChangeTo(CurrentLabel);
        }

        /// <summary>
        /// Decrease the index and update the labels when the left button is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="state"></param>
        private void LeftButton_OnMouseUp(GameObject sender, MouseState state)
        {
            if (currentIndex == 0)
                currentIndex = values.Count - 1;
            else
                currentIndex--;
            this.label.ChangeTo(CurrentLabel);
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
    }
}
