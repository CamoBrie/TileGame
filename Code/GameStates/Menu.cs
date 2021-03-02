using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TileGame.Code.GameObjects.Default;
using TileGame.Code.GameObjects.Default.UI;
using TileGame.Code.GameObjects.Default.Drawing;
using System;
using TileGame.Code.Data;

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

        internal CycleButton<float> UIScaleButton;
        #endregion

        internal GSMenu(Vector2 center, int width, int height) : base(center, width, height)
        {
            UI.Add(startButton = new UISpriteObject(new Rectangle(-200,-200,400,400), "views/menu/start_button", Anchor.Left, GameState.canvas));
            UI.Add(exitButton = new UISpriteObject(new Rectangle(-60, 10, 50, 50), "views/menu/exit_button", Anchor.TopRight, GameState.canvas));
            UI.Add(new UITextObject(new Rectangle(0,0,300,100), Anchor.TopLeft, GameState.canvas, "SETTINGS\\img(views/menu/start_button)", Color.Black, "views/fonts/sans", textAlignment.Left, 1.0f, false));
            UI.Add(UIScaleButton = new CycleButton<float>(new Rectangle(-64, -16, 128, 32), Anchor.Center, GameState.canvas, new float[]{0.6f, 0.8f, 1f, 1.5f, 1.8f} , new string[] { "tiny", "small", "regular", "big", "huge"}));
            UIScaleButton.SetCurrentValue(Settings.UIScale);

            startButton.OnMouseUp += StartHandler;
            exitButton.OnMouseUp += ExitHandler;
        }

        internal override void Update(GameTime time)
        {
            Settings.UIScale = UIScaleButton.CurrentValue;
            base.Update(time);
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
