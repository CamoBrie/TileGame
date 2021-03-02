using Microsoft.Xna.Framework;
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
        /// Override the globalPosition to apply the anchorOffset and UIScale.
        /// </summary>
        internal override Vector2 globalPosition
        {
            get
            {
                if (parent != null)
                {
                    return centerPosition * Settings.UIScale + parent.globalPosition + AnchorOffset.ToVector2();
                }
                else
                {
                    return centerPosition;
                }
            }
        }

        /// <summary>
        /// Apply the UIScale to the Drawposition
        /// </summary>
        /// <returns></returns>
        internal override Rectangle GetDrawPos()
        {
            return new Rectangle((int)(globalPosition.X - (width * Settings.UIScale) / 2), (int)(globalPosition.Y - (height * Settings.UIScale) / 2), (int)(width * Settings.UIScale), (int)(height*Settings.UIScale));
        }

        /// <summary>
        /// The Offset determined by the AnchorPreset
        /// </summary>
        Point AnchorOffset
        {
            get
            {
                switch (anchorMode)
                {
                    case Anchor.Left:
                        return new Point(-parent.width / 2, 0);
                    case Anchor.Right:
                        return new Point(parent.width / 2, 0);
                    case Anchor.Top:
                        return new Point(0, -parent.height / 2);
                    case Anchor.Bottom:
                        return new Point(0, parent.height / 2);
                    case Anchor.TopLeft:
                        return new Point(-parent.width / 2, -parent.height / 2);
                    case Anchor.BottomRight:
                        return new Point(parent.width / 2, parent.height / 2);
                    case Anchor.TopRight:
                        return new Point(parent.width / 2, -parent.height / 2);
                    case Anchor.BottomLeft:
                        return new Point(-parent.width / 2, parent.height / 2);
                    default:
                        return Point.Zero;
                }
            }
        }

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
