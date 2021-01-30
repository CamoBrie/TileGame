using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileGame.GameObjects;
using TileGame.Input;
using TileGame.Levels;

namespace TileGame.Views
{
    class GameView : GameObject
    {
        private Level level;
        private Player player;
        internal GameView(Vector2 center, int width, int height) : base(center, width, height)
        {
            //testing code
            player = new Player(new Vector2(110, 110), 100, 100, "bat");
            player.PlayAnimation("DE-fly", true);
            this.children.Add(player);
            //testing code
            level = new Level("path/to/level", ref player);

  
        }

        internal override void Update(GameTime time)
        {
            base.Update(time);
            level.Update();
        }

        internal override void Draw(SpriteBatch batch)
        {
            //first draw the tiles under the player,
            level.Pre_draw(batch);
            //then draw the player,
            player.Draw(batch);
            //and only then draw the tiles on top of the player.
            level.Post_draw(batch);
        }

        internal override void HandleInput()
        {
            /// Player keys
            Vector2 intentDir = Vector2.Zero;
            if(InputManager.KeyDown(Keys.W))
            {
                intentDir.Y--;
            }
            if (InputManager.KeyDown(Keys.A))
            {
                intentDir.X--;
            }
            if (InputManager.KeyDown(Keys.S))
            {
                intentDir.Y++;
            }
            if (InputManager.KeyDown(Keys.D))
            {
                intentDir.X++;
            }
            player.HandleInput(intentDir);
        }
    }
}
