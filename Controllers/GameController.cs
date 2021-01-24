using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileGame.GameObjects;
using TileGame.Input;
using TileGame.Views;

namespace TileGame.Controllers
{
    class GameController : Controller
    {
        internal GameView gameView => (GameView)this.view;
        public GameController() : base(new GameView(new Vector2(Game.screenSize.X / 2, Game.screenSize.Y / 2), Game.screenSize.X, Game.screenSize.Y))
        {
        }

        protected override void initializeViewAndEvents()
        {
        }

        internal override void handleInput()
        {
            //TODO: handle keypresses, such as opening the menu


        }
    }
}
