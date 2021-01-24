using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileGame.GameObjects;

namespace TileGame.Views
{
    class StartScreen : GameObject
    {
        #region Buttons
        internal GameObject startButton;
        internal GameObject exitButton;
        #endregion

        internal StartScreen(Vector2 center, int width, int height, string assetName = "") : base(center, width, height, assetName)
        {
            startButton = new GameObject(new Vector2(Game.screenSize.X / 2, Game.screenSize.Y / 4), 200, 200, "views/menu/start_button");
            this.children.Add(startButton);

            exitButton = new GameObject(new Vector2(Game.screenSize.X / 2, Game.screenSize.Y / 4 * 3), 200, 200, "views/menu/exit_button");
            this.children.Add(exitButton);
        }
    }
}
