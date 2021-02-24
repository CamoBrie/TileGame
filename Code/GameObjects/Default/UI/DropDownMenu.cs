using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using TileGame.Code.Utils;
using TileGame.Code.Utils.Convenience;
using TileGame.Code.GameObjects.Default.Drawing;
using static TileGame.Code.Events.CollisionEvent;
using static TileGame.Code.Events.MouseEvent;

namespace TileGame.Code.GameObjects.Default.UI
{
    class DropDownMenu : UIObject
    {
        string[] values;

        TextObject[] textObjects;

        int selectedIndex;

        internal bool selected;

        internal string selectedValue => values[selectedIndex];

        internal TextObject selectedTextObject => textObjects[selectedIndex];

        Texture2D backGround, frame;


        internal DropDownMenu(Rectangle pos, string assetName, string fontName, string[] values, Anchor anchorMode, UIObject parent) : base(pos, anchorMode, parent)
        {
            selectedIndex = 0;
            backGround = Game.textures.Get(assetName);
            frame = Game.textures.Get("views/menu/exit_button");

            textObjects = new TextObject[values.Length];
            for(int i = 0; i < values.Length; i++)
            {
                textObjects[i] = new TextObject(fontName, values[i]);
            }

            this.OnMouseUp += SelectHandler;
        }

        private void SelectHandler(GameObject sender, MouseState mouse)
        {
            selected = !selected;
        }

        internal override void Draw(SpriteBatch batch)
        {
            if (selected)
            {

            }
            else
            {
                batch.Draw(backGround, GetDrawPos(), Color.White);
                batch.Draw(frame, GetDrawPos(), Color.White);
                selectedTextObject.Draw(batch, GetDrawPos());
            }
            base.Draw(batch);
        }

    }
}
