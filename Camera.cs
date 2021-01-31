using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileGame.GameObjects;

namespace TileGame { 
    internal static class Camera
    {

        //#####
        // screen-to-world space:
        // Vector2.Transform(mouseLocation, Matrix.Invert(Camera.TransformMatrix));
        //
        // world-to-screen space:
        // Vector2.Transform(mouseLocation, Camera.TransformMatrix);
        //#####

        /// <summary>
        /// The current zoom of the camera.
        /// </summary>
        internal static float Zoom { get; set; }
        /// <summary>
        /// The current location of the camera.
        /// </summary>
        internal static Vector2 Location { get; set; }
        /// <summary>
        /// The current Rotation of the camera.
        /// </summary>
        internal static float Rotation { get; set; }
        /// <summary>
        /// The current Bounds of the camera.
        /// </summary>
        internal static Rectangle Bounds { get; set; }

        /// <summary>
        /// the matrix that determines where to translate the camera to.
        /// </summary>
        internal static Matrix TransformMatrix
        {
            get {
                return
                    Matrix.CreateTranslation(new Vector3(-Location.X + Bounds.X/2, -Location.Y + Bounds.Y/1, 0)) *
                    Matrix.CreateRotationZ(Rotation) *
                    Matrix.CreateScale(Zoom) *
                    Matrix.CreateTranslation(new Vector3(Bounds.Width * 0.5f, Bounds.Height * 0.5f, 0));
            }
        }

        /// <summary>
        /// this method resets everything related to the camera.
        /// </summary>
        internal static void Reset()
        {
            Zoom = 1;
            Location = new Vector2(Game.screenSize.X/2,Game.screenSize.Y/2);
            Rotation = 0;
            Bounds = new Rectangle(0, 0, Game.screenSize.X, Game.screenSize.Y);
        }

        /// <summary>
        /// this method returns a rectangle which is the visible area. 
        /// </summary>
        internal static Rectangle VisibleArea
        {
            get
            {
                var inverseViewMatrix = Matrix.Invert(TransformMatrix);
                var tl = Vector2.Transform(Vector2.Zero, inverseViewMatrix);
                var tr = Vector2.Transform(new Vector2(Game.screenSize.X, 0), inverseViewMatrix);
                var bl = Vector2.Transform(new Vector2(0, Game.screenSize.Y), inverseViewMatrix);
                var br = Vector2.Transform(Game.screenSize.ToVector2(), inverseViewMatrix);
                var min = new Vector2(
                    MathHelper.Min(tl.X, MathHelper.Min(tr.X, MathHelper.Min(bl.X, br.X))),
                    MathHelper.Min(tl.Y, MathHelper.Min(tr.Y, MathHelper.Min(bl.Y, br.Y))));
                var max = new Vector2(
                    MathHelper.Max(tl.X, MathHelper.Max(tr.X, MathHelper.Max(bl.X, br.X))),
                    MathHelper.Max(tl.Y, MathHelper.Max(tr.Y, MathHelper.Max(bl.Y, br.Y))));
                return new Rectangle((int)min.X, (int)min.Y, (int)(max.X - min.X), (int)(max.Y - min.Y));
            }
        }
    }
}
