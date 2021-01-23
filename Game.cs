using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TileGame.Controllers;
using TileGame.Input;

namespace TileGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        #region Monogame Variables
        internal static Game game;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D empty_texture;
        #endregion

        #region Global variables
        internal static Point screenSize = new Point(1000, 800);
        internal Controller gameState;
        private int id;
        #endregion

        public Game()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //have a reference to the instance that can be accessed from anywhere.
            game = this;
            IsMouseVisible = true;
        }

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
            empty_texture = new Texture2D(GraphicsDevice, 1, 1);
            empty_texture.SetData(new Color[] { new Color(0, 0, 0, 0) });


            //start the game after the content is loaded.
            changeGameState("menu");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            InputManager.update();
            gameState.update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            SpriteBatch sb = new SpriteBatch(this.GraphicsDevice);
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            gameState.draw(sb);
            sb.End();

            base.Draw(gameTime);
        }

        internal int getUniqueGameObjectID()
        {
            id++;
            return id;
        }

        //get a sprite from the assetname texture
        //TODO: use spritesheets etc
        internal Texture2D getSprite(string assetName)
        {
            // return empty texture if an empty string
            if (assetName.Length == 0)
            {
                return empty_texture;
            }

            Texture2D sprite;
            try
            {
                sprite = this.Content.Load<Texture2D>(assetName);
            }
            catch (ContentLoadException)
            {
                sprite = this.Content.Load<Texture2D>("missing_texture");
            }

            return sprite;
        }

        internal void changeGameState(string stateName = "")
        {
            switch(stateName)
            {
                case "menu":
                    this.gameState = new MenuController();
                    break;
                case "game":
                    //this.gameState = new GameController();
                    break;
                default:
                    //this.gameState = new ErrorController();
                    break;
            }
        }
    }
}
