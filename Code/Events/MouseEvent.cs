using Microsoft.Xna.Framework.Input;
using System;
using TileGame.Code.GameObjects.Default;

namespace TileGame.Code.Events
{
    static class MouseEvent
    {
        internal delegate void mouseEvent(GameObject sender, MouseState state);

        internal static void _default(GameObject sender, MouseState state)
        {
            Console.WriteLine("lmao noob");
        }
    }
}
