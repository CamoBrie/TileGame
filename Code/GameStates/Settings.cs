using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TileGame.Code.GameObjects;
using TileGame.Code.GameObjects.Default;
using TileGame.Code.Utils;
using TileGame.Levels;
using System.Collections.Generic;

namespace TileGame.Code.GameStates
{
    class GSSettings : GameState
    {

        internal GSSettings(Vector2 center, int width, int height) : base(center, width, height)
        {

        }

    }
}
