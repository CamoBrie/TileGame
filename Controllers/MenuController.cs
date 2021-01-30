using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileGame.GameObjects;
using TileGame.Views;

namespace TileGame.Controllers
{
    class MenuController : Controller
    {
        internal StartScreen StartScreen => (StartScreen)this.view;

        public MenuController() : base(new StartScreen(new Vector2(Game.screenSize.X / 2, Game.screenSize.Y / 2), Game.screenSize.X, Game.screenSize.Y))
        {
        }
        protected override void InitializeViewAndEvents()
        {
            this.StartScreen.startButton.OnMouseUp += StartHandler;
            this.StartScreen.exitButton.OnMouseUp += ExitHandler;
        }

        private void StartHandler(GameObject sender, MouseState mouse)
        {
            Game.game.ChangeGameState("game");
        }
        private void ExitHandler(GameObject sender, MouseState mouse)
        {
            Game.game.Exit();
        }



    }
}
