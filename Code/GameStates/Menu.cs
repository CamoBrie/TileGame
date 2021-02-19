using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TileGame.Code.GameObjects.Default;
using TileGame.Code.GameObjects.Default.UI;
using TileGame.Code.GameObjects.Default.Drawing;

namespace TileGame.Code.GameStates
{
    internal class GSMenu : GameState
    {
        #region Buttons
        /// <summary>
        /// The button for starting the game.
        /// </summary>
        internal UIObject startButton;
        /// <summary>
        /// The button for exiting the game.
        /// </summary>
        internal UIObject exitButton;
        #endregion

        internal GSMenu(Vector2 center, int width, int height) : base(center, width, height)
        {
            //startButton = new SpriteObject(new Vector2(Game.screenSize.X / 2, Game.screenSize.Y / 4), 200, 200, "views/menu/start_button");
            //children.Add(startButton);
            UI.Add(startButton = new UISpriteObject(new Rectangle(-200,-200,400,400), "views/menu/start_button", Anchor.Center, GameState.canvas));
            exitButton = new UISpriteObject(new Rectangle(-100, -100, 200, 200), "views/menu/exit_button", Anchor.BottomRight, startButton);

            //exitButton = new SpriteObject(new Vector2(Game.screenSize.X / 2, Game.screenSize.Y / 4 * 3), 200, 200, "views/menu/exit_button");
            //children.Add(exitButton);

            startButton.OnMouseUp += StartHandler;
            exitButton.OnMouseUp += ExitHandler;
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
