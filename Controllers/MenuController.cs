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
        /// <summary>
        /// The view managed by this controller.
        /// </summary>
        internal StartScreen StartScreen => (StartScreen)this.view;

        /// <summary>
        /// This constructor sets the view that the controller is controlling and then initializes this view and all the events.
        /// </summary>
        /// <param name="view">The view managed by this controller.</param>
        public MenuController() : base(new StartScreen(new Vector2(Game.screenSize.X / 2, Game.screenSize.Y / 2), Game.screenSize.X, Game.screenSize.Y))
        {
        }

        /// <summary>
        /// Abstract method that initializes all the event handlers.
        /// This method sets initializes all events of its own view, but also possibly of their children!
        /// E.g: the screen controller can set the events of the screen itself, but also the events on the buttons on the screen.
        /// This is because buttons are so small, that they do not need their own controller.
        /// </summary>
        protected override void InitializeViewAndEvents()
        {
            this.StartScreen.startButton.OnMouseUp += StartHandler;
            this.StartScreen.exitButton.OnMouseUp += ExitHandler;
        }

        /// <summary>
        /// the handler for clicking on the start button.
        /// </summary>
        /// <param name="sender">the object that is being clicked on.</param>
        /// <param name="mouse">the current state of the mouse.</param>
        private void StartHandler(GameObject sender, MouseState mouse)
        {
            Game.game.ChangeGameState("game");
        }
        /// <summary>
        /// the handler for clicking on the exit button.
        /// </summary>
        /// <param name="sender">the object that is being clicked on.</param>
        /// <param name="mouse">the current state of the mouse.</param>
        private void ExitHandler(GameObject sender, MouseState mouse)
        {
            Game.game.Exit();
        }



    }
}
