using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Aseprite.Documents;
using MonoGame.Aseprite.Graphics;

namespace TileGame.GameObjects
{
    class Player : GameEntity
    {

        internal Player(Vector2 center, int width, int height, string assetName) : base(center, width, height, assetName)
        {

        }

        /// <summary>
        /// handle the input provided by the controller
        /// </summary>
        /// <param name="intentDir"></param>
        internal void HandleInput(Vector2 intentDir)
        {
            this.acceleration = intentDir;
        }
    }
}
