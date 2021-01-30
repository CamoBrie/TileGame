using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileGame.GameObjects;
using MonoGame.Aseprite.Documents;
using MonoGame.Aseprite.Graphics;

namespace TileGame.Animation
{
    class Animation : GameObject
    {
        AsepriteDocument doc;



        internal Animation(GameObject parent, string fileName) : base(parent.centerPosition, parent.width, parent.height, "")
        {

        }
    }
}
