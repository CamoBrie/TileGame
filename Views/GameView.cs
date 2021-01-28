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
        internal GameView(Vector2 center, int width, int height, string assetName = "") : base(center, width, height, assetName)
        {
            //testing code
            player = new Player(new Vector2(110, 110), 100, 100, "views/game/player");
            this.children.Add(player);
            //testing code
            level = new Level("path/to/level", ref player);

  
        }

        internal override void update(GameTime time)
        {
            base.update(time);
            level.update();
        }

        internal override void draw(SpriteBatch batch)
        {
            //first draw the tiles under the player,
            level.pre_draw(batch);
            //then draw the player,
            player.draw(batch);
            //and only then draw the tiles on top of the player.
            level.post_draw(batch);
        }

        internal override void handleInput()
        {
            /// Player keys
            Vector2 intentDir = Vector2.Zero;
            if(InputManager.keyDown(Keys.W))
            {
                intentDir.Y--;
            }
            if (InputManager.keyDown(Keys.A))
            {
                intentDir.X--;
            }
            if (InputManager.keyDown(Keys.S))
            {
                intentDir.Y++;
            }
            if (InputManager.keyDown(Keys.D))
            {
                intentDir.X++;
            }
            player.handleInput(intentDir);

            base.handleInput();
        }
    }
}
