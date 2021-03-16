using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TileGame.Code.Animations;

namespace TileGame.Code.GameObjects.Default.UI
{
    class UIButton : UIAnimatedObject
    {
        internal UIButton(Rectangle pos, string assetName, string soundName, Anchor anchorMode, UIObject parent) : base(pos, assetName, anchorMode, parent)
        {
            //TO-DO: Insert sound code

            this.OnMouseUp += Button_OnMouseUp;
            this.OnMouseDown += Button_OnMouseDown;
        }

        private void Button_OnMouseDown(GameObject sender, Microsoft.Xna.Framework.Input.MouseState state)
        {
            //TO-DO: play sound

            PlayAnimation("pressed");
        }

        private void Button_OnMouseUp(GameObject sender, Microsoft.Xna.Framework.Input.MouseState state)
        {
            PlayAnimation("default");
        }
    }
}
