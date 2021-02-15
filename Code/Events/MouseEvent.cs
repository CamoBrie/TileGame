using Microsoft.Xna.Framework.Input;
using System;
using TileGame.Code.GameObjects.Default;

namespace TileGame.Code.Events
{
    internal static class MouseEvent
    {
        internal delegate void mouseEvent(GameObject sender, MouseState state);
    }
}
