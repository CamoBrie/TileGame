using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileGame.GameObjects;
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
            player = new Player(new Vector2(10, 10), 100, 100, "player");

            //testing code
            level = new Level("path/to/level", ref player);
  
        }

        internal override void update(GameTime time)
        {
            level.update();
            base.update(time);
        }

        internal override void draw(SpriteBatch batch)
        {
            //first draw the tiles under the player,
            level.pre_draw(batch);
            //then draw the player,
            player.draw(batch);
            //and only then draw the tiles on top of the player.
            level.post_draw(batch);

            base.draw(batch);
        }
    }
}
