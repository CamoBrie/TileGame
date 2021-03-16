using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TileGame.Code.GameObjects;
using TileGame.Code.GameObjects.Default;
using TileGame.Code.Utils;
using TileGame.Levels;
namespace TileGame.Code.GameStates
{
    class GameState : GameObject
    {
        protected List<UIObject> UI;

        internal static UIObject canvas;


        internal GameState(Vector2 center, int width, int height) : base(center, width, height)
        {
            UI = new List<UIObject>();
            canvas = new UIObject();
        }

        internal override void Update(GameTime time)
        {
            base.Update(time);
            canvas.Update(time);
        }

        internal void DrawUI(SpriteBatch batch)
        {
            foreach (UIObject u in canvas.children)
                u.Draw(batch);
        }

    }
}
