using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TileGame.Code.GameObjects;
using TileGame.Code.GameObjects.Default;
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
        internal GSPlaying(Vector2 center, int width, int height) : base(center, width, height)
        {
            //testing code
            player = new Player(new Vector2(128, 128), 64, 64, "missing_aseDoc");
            player.PlayAnimation("DE-fly", false);
            children.Add(player);
            //testing code
            level = new Level("path/to/level", ref player);

            OnMouseUp += level.Player_SwingInDirection;

        }

        /// <summary>
        /// Updates the level.
        /// </summary>
        /// <param name="time">the time object.</param>
        internal override void Update(GameTime time)
        {
            base.Update(time);
            level.Update(time);
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

        }

        /// <summary>
        /// Handles the input for this view, and send the output to the children.
        /// </summary>
        internal override void HandleInput()
        {
            base.HandleInput();
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
