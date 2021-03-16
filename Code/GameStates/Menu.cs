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
        #region SettingsOptions
        static Point[] resolutionOptions = new Point[] { new Point(1024, 768), new Point(1280, 720), new Point(1280, 800), new Point(1280, 1024), new Point(1360, 768), new Point(1366, 768),
        new Point(1440, 900), new Point(1600, 900), new Point(1680, 1050), new Point(1920, 1080), new Point(1920, 1200), new Point(2560, 1440) };

        static string[] resolutionLabels = new string[] { "1024, 768", "1280, 720", "1280, 800", "1280, 1024", "1360, 768", "1366, 768",
        "1440, 900", "1600, 900", "1680, 1050", "1920, 1080", "1920, 1200", "2560, 1440" };

        static float[] uiScaleOptions = new float[] { 0.6f, 0.8f, 1f, 1.3f, 1.8f };

        static string[] uiScaleLabels = new string[] { "tiny", "small", "regular", "big", "huge" };

        static float[] volumeOptions = new float[] { 0f, 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 1f };

        static string[] volumeLabels = new string[] { "0", "10", "20", "30", "40", "50", "60", "70", "80", "90", "100" };
        #endregion

        #region Buttons

        UIButton continueButton;

        UIButton exitGameButton;

        CycleButton<float> UIScaleButton;

        CycleButton<float> masterVolumeButton;

        CycleButton<float> musicVolumeButton;

        CycleButton<float> effectVolumeButton;

        CycleButton<bool> windowModeButton;

        CycleButton<Point> resolutionButton;
        #endregion 

        static string font = "views/fonts/sans";

        internal GSMenu(Vector2 center, int width, int height) : base(center, width, height)
        {
            Console.WriteLine("UIScale: " + Settings.UIScale);
            Console.WriteLine("Entering Menu!");

            UI.Add(new UITextObject(new Rectangle(-128, 10, 256, 128), Anchor.Top, GameState.canvas, "SETTINGS", Color.Black, font, textAlignment.Center, 3.0f, false));

            UI.Add(new UITextObject(new Rectangle(-384, -192, 256, 64), Anchor.Center, GameState.canvas, "RESOLUTION", Color.Black, font, textAlignment.Left, 1f, false));
            UI.Add(new UITextObject(new Rectangle(-384, -64, 256, 64), Anchor.Center, GameState.canvas, "WINDOW MODE", Color.Black, font, textAlignment.Left, 1f, false));
            UI.Add(new UITextObject(new Rectangle(-384, 64, 256, 64), Anchor.Center, GameState.canvas, "UI SIZE", Color.Black, font, textAlignment.Left, 1f, false));

            UI.Add(new UITextObject(new Rectangle(0, -192, 256, 64), Anchor.Center, GameState.canvas, "MASTER VOLUME", Color.Black, font, textAlignment.Left, 1f, false));
            UI.Add(new UITextObject(new Rectangle(0, -64, 256, 64), Anchor.Center, GameState.canvas, "MUSIC VOLUME", Color.Black, font, textAlignment.Left, 1f, false));
            UI.Add(new UITextObject(new Rectangle(0, 64, 256, 64), Anchor.Center, GameState.canvas, "EFFECT VOLUME", Color.Black, font, textAlignment.Left, 1f, false));

            UI.Add(resolutionButton = new CycleButton<Point>(new Rectangle(-184, -192, 128, 32), Anchor.Center, GameState.canvas, resolutionOptions, resolutionLabels));
            UI.Add(windowModeButton = new CycleButton<bool>(new Rectangle(-184, -64, 128, 32), Anchor.Center, GameState.canvas, new bool[] { false, true }, new string[] { "windowed", "fullscreen" }));
            UI.Add(UIScaleButton = new CycleButton<float>(new Rectangle(-184, 64, 128, 32), Anchor.Center, GameState.canvas, uiScaleOptions, uiScaleLabels));

            UI.Add(masterVolumeButton = new CycleButton<float>(new Rectangle(224, -192, 128, 32), Anchor.Center, GameState.canvas, volumeOptions, volumeLabels));
            UI.Add(musicVolumeButton = new CycleButton<float>(new Rectangle(224, -64, 128, 32), Anchor.Center, GameState.canvas, volumeOptions, volumeLabels));
            UI.Add(effectVolumeButton = new CycleButton<float>(new Rectangle(224, 64, 128, 32), Anchor.Center, GameState.canvas, volumeOptions, volumeLabels));

            UI.Add(exitGameButton = new UIButton(new Rectangle(16, -96, 64, 32), "views/menu/wood_button_2", "", Anchor.Bottom, GameState.canvas));
            new UITextObject(exitGameButton, "EXIT", Color.Black, font, textAlignment.Center, 1.0f, false);

            UI.Add(continueButton = new UIButton(new Rectangle(-80, -96, 64, 32), "views/menu/wood_button_2", "", Anchor.Bottom, GameState.canvas));
            new UITextObject(continueButton, "APPLY", Color.Black, font, textAlignment.Center, 1.0f, false);

            resolutionButton.SetCurrentValue(Settings.Resolution);
            windowModeButton.SetCurrentValue(Settings.FullScreen);
            UIScaleButton.SetCurrentValue(Settings.UIScale);
            masterVolumeButton.SetCurrentValue(Settings.MasterVolume);
            musicVolumeButton.SetCurrentValue(Settings.MusicVolume);
            effectVolumeButton.SetCurrentValue(Settings.EffectVolume);

            continueButton.OnMouseUp += StartHandler;
            exitGameButton.OnMouseUp += ExitHandler;
        }

        internal override void Update(GameTime time)
        {
            Settings.UIScale = UIScaleButton.CurrentValue;
            Settings.FullScreen = windowModeButton.CurrentValue;
            Settings.Resolution = resolutionButton.CurrentValue;
            Settings.MasterVolume = masterVolumeButton.CurrentValue;
            Settings.MusicVolume = musicVolumeButton.CurrentValue;
            Settings.EffectVolume = effectVolumeButton.CurrentValue;

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
            Game.game.ApplySettings();
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
