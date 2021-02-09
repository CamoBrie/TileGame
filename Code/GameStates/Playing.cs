﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TileGame.Code.GameObjects;
using TileGame.Code.GameObjects.Default;
using TileGame.Code.GameObjects.Default.Drawing;
using TileGame.Code.Utils;
using TileGame.Levels;

namespace TileGame.Code.GameStates
{
    internal class GSPlaying : GameObject
    {
        /// <summary>
        /// The instance of the current level.
        /// </summary>
        private readonly Level level;
        /// <summary>
        /// The instance of the player.
        /// </summary>
        private readonly Player player;

        private readonly TextObject to;
        internal GSPlaying(Vector2 center, int width, int height) : base(center, width, height)
        {
            //testing code
            player = new Player(new Vector2(110, 110), 100, 100, "missing_aseDoc");
            player.PlayAnimation("DE-fly", false);
            children.Add(player);
            //testing code
            level = new Level("path/to/level", ref player);

            to = new TextObject(Game.game.Content.Load<SpriteFont>("test"), "\\color(Red)Welcome to our \\color(LightGreen)Game! \\r.color\\r.scale\\img(missing_texture) <- this still needs an icon.");
        }

        /// <summary>
        /// Updates the level.
        /// </summary>
        /// <param name="time">the time object.</param>
        internal override void Update(GameTime time)
        {
            base.Update(time);
            level.Update();
        }

        /// <summary>
        /// Draws the first part of the level, then the player, and then the other part.
        /// </summary>
        /// <param name="batch"></param>
        internal override void Draw(SpriteBatch batch)
        {
            //first draw the tiles under the player,
            level.Pre_draw(batch);
            //then draw the player,
            player.Draw(batch);
            //and only then draw the tiles on top of the player.
            level.Post_draw(batch);

            to.Draw(batch, new Rectangle(100, 100, 1000, 300));

        }

        /// <summary>
        /// Handles the input for this view, and send the output to the children.
        /// </summary>
        internal override void HandleInput()
        {
            /// Player keys
            Vector2 intentDir = Vector2.Zero;
            if(InputManager.KeyDown(Keys.W))
            {
                intentDir.Y--;
            }
            if (InputManager.KeyDown(Keys.A))
            {
                intentDir.X--;
            }
            if (InputManager.KeyDown(Keys.S))
            {
                intentDir.Y++;
            }
            if (InputManager.KeyDown(Keys.D))
            {
                intentDir.X++;
            }
            player.HandleInput(intentDir);
        }
    }
}