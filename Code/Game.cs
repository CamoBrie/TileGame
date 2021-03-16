using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite.Documents;
using System;
using Microsoft.Xna.Framework.Media;
using TileGame.Code.GameStates;
using TileGame.Code.Utils;
using TileGame.Code.Data;
using TileGame.Code.GameObjects.Data;

namespace TileGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        #region Monogame Variables
        internal static Game game;
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        internal Texture2D empty_texture;
        #endregion

        #region Global variables
        /// <summary>
        /// The current size of the screen.
        /// </summary>
        internal static Point screenSize
        {
            get
            {
                return new Point(game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height);
            }
        }

        /// <summary>
        /// the current gameState.
        /// </summary>
        internal GameState gameState;
        /// <summary>
        /// The id that is given to the next generated GameObject.
        /// </summary>
        private int id;
        /// <summary>
        /// The library that stores all the texture files.
        /// </summary>
        internal static ContentLibrary<Texture2D> textures;
        /// <summary>
        /// The library that stores all the .ase files.
        /// </summary>
        internal static ContentLibrary<AsepriteDocument> aseDocs;
        // <summary>
        /// The library that stores all the .spritefont files.
        /// </summary>
        internal static ContentLibrary<SpriteFont> fonts;

        #endregion

        public Game()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //have a reference to the instance that can be accessed from anywhere.
            game = this;
            IsMouseVisible = true;
        }

        #region MonoGame Functions
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = screenSize.X;  // set this value to the desired width of your window
            _graphics.PreferredBackBufferHeight = screenSize.Y;   // set this value to the desired height of your window
            _graphics.ApplyChanges();

            Camera.Reset();



            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //Create an empty texture and setup the Texture2D ContentLibrary
            empty_texture = new Texture2D(GraphicsDevice, 1, 1);
            empty_texture.SetData(new Color[] { new Color(255, 255, 255, 255) });
            textures = new ContentLibrary<Texture2D>(empty_texture, Content.Load<Texture2D>("missing_texture"));

            //Setup the AsepriteDocument ContentLibrary
            aseDocs = new ContentLibrary<AsepriteDocument>(Content.Load<AsepriteDocument>("empty_aseDoc"), Content.Load<AsepriteDocument>("missing_aseDoc"));

            //Setup the AsepriteDocument ContentLibrary
            fonts = new ContentLibrary<SpriteFont>(Content.Load<SpriteFont>("empty_font"), Content.Load<SpriteFont>("missing_font"));

            //TO-DO, Load Settings
            Settings.UIScale = 1.0f;
            Settings.Resolution = new Point(1000, 600);
            ApplySettings();

            //start the game after the content is loaded.
            ChangeGameState("menu");

#if DEBUG
            Console.WriteLine("Debug mode: on");
#else
            Console.WriteLine("Debug mode: off");
#endif
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            Content.Unload();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            InputManager.Update();
            gameState.HandleInput();
            gameState.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //SpriteBatch sb = new SpriteBatch(GraphicsDevice);
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, transformMatrix: Camera.TransformMatrix);
            gameState.Draw(_spriteBatch);
            _spriteBatch.End();

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
            gameState.DrawUI(_spriteBatch);
            _spriteBatch.End();
            //base.Draw(gameTime);
        }

#endregion

        #region Global Functions
        /// <summary>
        /// Gets a new unique game object ID.
        /// </summary>
        /// <returns>the new ID.</returns>
        internal int GetUniqueGameObjectID()
        {
            id++;
            return id;
        }

        internal void ApplySettings()
        {
            SoundEffect.MasterVolume = Settings.EffectVolume * Settings.MasterVolume;
            MediaPlayer.Volume = Settings.MusicVolume * Settings.MasterVolume;
            _graphics.PreferredBackBufferWidth = Settings.Resolution.X;
            _graphics.PreferredBackBufferHeight = Settings.Resolution.Y;
            _graphics.IsFullScreen = Settings.FullScreen;
            _graphics.ApplyChanges();
            Camera.Reset();
            Console.WriteLine("UIScale: " + Settings.UIScale);
        }

        /// <summary>
        /// Changes the gamestate to the specified state.
        /// </summary>
        /// <param name="stateName">the state which to state to.</param>
        internal void ChangeGameState(string stateName = "")
        {
            Content.Unload();
            textures.Clear();
            aseDocs.Clear();
            fonts.Clear();

            switch (stateName)
            {
                case "menu":
                    gameState = new GSMenu(screenSize.ToVector2()/2, screenSize.X, screenSize.Y);
                    break;
                case "game":
                    gameState = new GSPlaying(screenSize.ToVector2()/2, screenSize.X, screenSize.Y);
                    break;
                default:
                    //this.gameState = new ErrorController();
                    break;
            }
        }
#endregion
    }
}


