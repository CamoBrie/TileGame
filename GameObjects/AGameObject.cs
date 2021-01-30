using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileGame.GameObjects
{
    abstract class AGameObject
    {
        internal Vector2 centerPosition { get; set; }
        internal int width { get; set; }
        internal int height {get; set;}
        internal Texture2D sprite { get; set; }
        internal int ID { get; set; }

        internal abstract void Draw(SpriteBatch batch);
        internal abstract void DrawOwnSprite(SpriteBatch batch);
        internal abstract void DrawOwnSprite(SpriteBatch batch, Color color);
        internal abstract void Update(GameTime time);
        internal abstract void HandleInput();
        internal abstract Rectangle GetBoundingBox();
    }
}
