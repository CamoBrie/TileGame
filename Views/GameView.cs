using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileGame.GameObjects;

namespace TileGame.Views
{
    class GameView : GameObject
    {
        internal GameView(Vector2 center, int width, int height, string assetName = "") : base(center, width, height, assetName)
        {
        }
    }
}
