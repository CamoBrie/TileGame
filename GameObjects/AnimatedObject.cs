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

namespace TileGame.GameObjects
{
    class AnimatedObject : GameObject
    {
        AsepriteDocument doc;

        ///TESTING CODE
        Texture2D texture;
        ///TESTING CODE

        internal AnimatedObject(Vector2 center, int width, int height, string assetName) : base(center, width, height)
        {
            ///TESTING CODE
            this.texture = Game.game.GetSprite(assetName);
            ///TESTING CODE
        }

        ///TESTING CODE
        internal override void Draw(SpriteBatch batch)
        {
            batch.Draw(texture, this.GetBoundingBox(), Color.White);
        }
        ///TESTING CODE
    }
}
