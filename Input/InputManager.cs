using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using Microsoft.Xna.Framework;
using System;

namespace TileGame.Input
{
    internal static class InputManager
    {
        // The following attributes, store the state of the keyboard and the mouse.
        // The basic GameObject class uses this information to decide which event to fire.
        // With any luck, you won't ever need to read any of these attributes.
        internal static Keys[] previousKeys = { };
        internal static Keys[] currentKeys = { };

        // Bool attributes that indicate the state of the mouse.
        internal static bool didAKeyChange = false;
        internal static bool didTheMouseMove = false;
        internal static bool didTheMouseClick = false;
        internal static bool didTheMouseRelease = false;
        internal static bool doesTheMouseDrag = false;


        internal static MouseState MouseState;
        internal static Point previousLocation = Point.Zero;
        internal static Point currentLocation = Point.Zero;
        internal static ButtonState previousLeft = ButtonState.Released;
        internal static ButtonState currentLeft = ButtonState.Released;

        // A list that contains the IDs of all objects that are currently subject to the drag event.
        // Note that this means that the game should trigger a drag event, it should not neccesarily actually be "Dragging" them.
        internal static List<int> dragObjects = new List<int>();

        // The ID of the click that was pressed.
        private static int clickID = -1;
        
        /// <summary>
        /// This update function is called once per frame.
        /// It gets the new state from the input methods and stores it in static properties.
        /// </summary>
        internal static void update()
        {
            // Read which keys are currently being pressed, and compare them to the keys that previously were pressed.
            previousKeys = currentKeys;
            currentKeys = Keyboard.GetState().GetPressedKeys();
            didAKeyChange = previousKeys != currentKeys;

            // Read the location of the mouse, and check if the mouse has moved.
            MouseState = Mouse.GetState();
            previousLocation = currentLocation;
            currentLocation = MouseState.Position;
            didTheMouseMove = previousLocation != currentLocation;

            // Read the current state of the left mouse button, and decide if it is being pressed, moved up, moved down or dragged.
            previousLeft = currentLeft;
            currentLeft = MouseState.LeftButton;

            didTheMouseClick = (currentLeft != previousLeft) && (currentLeft == ButtonState.Pressed);
            didTheMouseRelease = (currentLeft != previousLeft) && (currentLeft == ButtonState.Released);
            doesTheMouseDrag = didTheMouseMove && (currentLeft == ButtonState.Pressed);

            // If the mouse goes down, we generate a new click ID.
            if (didTheMouseClick)
                clickID++;
        }




        /// <summary>
        /// Returns the ID of the current click.
        /// </summary>
        internal static int getClickID()
        {
            return clickID;
        }

        /// <summary>
        /// Determines whether the given key was pressed this frame.
        /// </summary>
        /// <param name="key">The key to query.</param>
        internal static bool keyPressed(Keys key)
        {
            return !previousKeys.Contains(key) && currentKeys.Contains(key);
        }

        /// <summary>
        /// Determines whether the given key was released this frame.
        /// </summary>
        /// <param name="key">The key to query.</param>
        internal static bool keyReleased(Keys key)
        {
            return previousKeys.Contains(key) && !currentKeys.Contains(key);
        }

        /// <summary>
        /// Determines if a key is currently down.
        /// </summary>
        /// <param name="key">The key to the query</param>
        /// <returns></returns>
        internal static bool keyDown(Keys key)
        {
            return currentKeys.Contains(key);
        }
    }
}
