using Microsoft.Xna.Framework;
using TileGame.Code.GameObjects.Default;
using TileGame.Code.GameObjects.Default.Items;
using TileGame.Code.Systems.Inventory;

namespace TileGame.Code.GameObjects
{
    internal class Player : GameEntity
    {
        /// <summary>
        /// the tool the player uses.
        /// </summary>
        internal Tool tool;

        internal static Inventory inventory;

        /// <summary>
        /// creates a player with the parameters
        /// </summary>
        /// <param name="center">the center of the player</param>
        /// <param name="width">the width of the player</param>
        /// <param name="height">the height of the player</param>
        /// <param name="assetName">the assetname for the player</param>
        internal Player(Vector2 center, int width, int height, string assetName) : base(center, width, height, assetName)
        {
            collides = true;
            tool = new Tool(this);
            inventory = new Inventory();
        }

        /// <summary>
        /// updates the player and all of its children.
        /// </summary>
        /// <param name="time"></param>
        internal override void Update(GameTime time)
        {
            tool.Update(time);
            base.Update(time);

        }

        /// <summary>
        /// handle the input provided by the controller
        /// </summary>
        /// <param name="intentDir"></param>
        internal void HandleInput(Vector2 intentDir)
        {
            acceleration = intentDir;
        }
    }
}
