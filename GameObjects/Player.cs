using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileGame.GameObjects
{
    class Player : GameEntity
    {
        internal Player(Vector2 center, int width, int height, string assetName) : base(center, width, height, assetName)
        {

        }

        internal void handleInput(Vector2 intentDir)
        {
            this.acceleration = intentDir;
        }
    }
}
