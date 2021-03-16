using System;
using Microsoft.Xna.Framework;
using TileGame.Code.GameStates;

namespace TileGame.Code.GameObjects.Default.UI
{
    class InventoryScreen : UIAnimatedObject
    {
        UITextObject woodDisplay, stoneDisplay;


        internal InventoryScreen() : base(new Rectangle(-256, -128, 512, 256), "views/menu/wood_button_2", Anchor.Center, GameState.canvas)
        {
            active = false;
            woodDisplay = new UITextObject(new Rectangle(30, 10, width - 40, 32), Anchor.TopLeft, this, "wood", Color.White, GSMenu.font);
            stoneDisplay = new UITextObject(new Rectangle(30, 52, width - 40, 32), Anchor.TopLeft, this, "stone", Color.White, GSMenu.font);
        }

        internal void Open()
        {
            active = true;
            UpdateValues();
        }

        internal void UpdateValues()
        {
            woodDisplay.textObject.ChangeTo("Wood: " + Player.inventory.wood);
            stoneDisplay.textObject.ChangeTo("Stone: " + Player.inventory.stone);
        }
    }
}
