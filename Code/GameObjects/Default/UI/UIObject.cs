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
using TileGame.Code.Data;

namespace TileGame.Code.GameObjects.Default
{
    class UIObject : GameObject
    {
        /// <summary>
        /// The anchor on the screen. The position is relative to the anchor
        /// </summary>
        protected Anchor anchorMode;

        /// <summary>
        /// Override the globalPosition to apply the anchorOffset.
        /// </summary>
        internal override Vector2 globalPosition
        {
            get
            {
                if (parent != null)
                {
                    return centerPosition + parent.globalPosition + AnchorOffset.ToVector2();
                }
                else
                {
                    return centerPosition;
                }
            }
        }

        /// <summary>
        /// The Offset determined by the AnchorPreset
        /// </summary>
        Point AnchorOffset;

        /// <summary>
        /// Use this constructer to create a canvas UIObject.
        /// </summary>
        internal UIObject() : base(Game.screenSize.ToVector2() / 2, Game.screenSize.X, Game.screenSize.Y)
        {
            this.anchorMode = Anchor.Center;           
        }

        /// <summary>
        /// Creates A UiObject with a parent.
        /// </summary>
        /// <param name="pos">the relative position and size.</param>
        /// <param name="anchorMode">the Anchor preset used.</param>
        /// <param name="parent">the parent to anchor to</param>
        internal UIObject(Rectangle pos, Anchor anchorMode, UIObject parent) : base(parent)
        {
            this.anchorMode = anchorMode;
            SetupPos(pos);
        }

        /// <summary>
        /// Setup the basic GameObject properties and get the anchoroffset from the preset.
        /// </summary>
        /// <param name="pos"></param>
        void SetupPos(Rectangle pos)
        {
            width = (int)(pos.Width * Settings.UIScale);
            height = (int)(pos.Height * Settings.UIScale);
            centerPosition = pos.Center.ToVector2()*Settings.UIScale;
            switch (anchorMode)
            {
                case Anchor.Left:
                    AnchorOffset = new Point(-parent.width/2, 0);
                    break;
                case Anchor.Right:
                    AnchorOffset = new Point(parent.width / 2, 0);
                    break;
                case Anchor.Top:
                    AnchorOffset = new Point(0, -parent.height /2);
                    break;
                case Anchor.Bottom:
                    AnchorOffset = new Point(0, parent.height / 2);
                    break;
                case Anchor.TopLeft:
                    AnchorOffset = new Point(-parent.width / 2);
                    break;
                case Anchor.BottomRight:
                    AnchorOffset = new Point(parent.width / 2);
                    break;
                case Anchor.TopRight:
                    AnchorOffset = new Point(parent.width / 2, -parent.height / 2);
                    break;
                case Anchor.BottomLeft:
                    AnchorOffset = new Point(-parent.width / 2, parent.height / 2);
                    break;
                default:
                    break;
            }
        }

    }

    internal enum Anchor
    {
        Left,
        Right,
        Top,
        Bottom,
        TopLeft,
        TopRight,
        BottomRight,
        BottomLeft,
        Center
    }
}
