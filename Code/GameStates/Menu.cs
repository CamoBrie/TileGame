using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TileGame.Code.GameObjects.Default;
using TileGame.Code.GameObjects.Default.Drawing;

namespace TileGame.Code.GameStates
{
    internal class GSMenu : GameObject
    {
        #region Buttons
        /// <summary>
        /// The button for starting the game.
        /// </summary>
        internal GameObject startButton;
        /// <summary>
        /// The button for exiting the game.
        /// </summary>
        internal GameObject exitButton;
        #endregion

        internal GSMenu(Vector2 center, int width, int height) : base(center, width, height)
        {
            startButton = new SpriteObject(new Vector2(Game.screenSize.X / 2, Game.screenSize.Y / 4), 200, 200, "views/menu/start_button");
            children.Add(startButton);

            exitButton = new SpriteObject(new Vector2(Game.screenSize.X / 2, Game.screenSize.Y / 4 * 3), 200, 200, "views/menu/exit_button");
            children.Add(exitButton);

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
