using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TileGame.Code.GameObjects.Default;
using TileGame.Code.GameObjects.Default.UI;
using TileGame.Code.GameObjects.Default.Drawing;


namespace TileGame.Code.GameStates
{
    //RainBow:
    //"\\scale(1)\\color(red)R \\color(orange)A \\color(yellow)I \\color(green)N \\color(blue)B \\color(indigo)O \\color(violet)W\\r.color\\r.scale"
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
            UI.Add(startButton = new UISpriteObject(new Rectangle(-200,-200,400,400), "views/menu/start_button", Anchor.Left, GameState.canvas));
            UI.Add(exitButton = new UISpriteObject(new Rectangle(-60, 10, 50, 50), "views/menu/exit_button", Anchor.TopRight, GameState.canvas));
            UI.Add(new UITextObject(new Rectangle(0, 0, 300, 100), "Main Menu", Anchor.Center, GameState.canvas, "views/fonts/sans"));
            UI.Add(new CycleButton<int>(Point.Zero, Anchor.Center, GameState.canvas, new int[]{ 1, 2, 3, 4} , new string[]{ "bks dxcbe xuc hdie bcxd uxube d bkch udkbu cuie bxcjh kcb bs hbcj yde", "2", "3" }));

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
