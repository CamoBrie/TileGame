﻿using Microsoft.Xna.Framework;

namespace TileGame.Code.Utils
{
    internal static class Camera
    {

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
        internal static Matrix TransformMatrix => Matrix.CreateTranslation(new Vector3(-Location.X + Bounds.X / 2, -Location.Y + Bounds.Y / 1, 0)) *
                    Matrix.CreateRotationZ(Rotation) *
                    Matrix.CreateScale(Zoom) *
                    Matrix.CreateTranslation(new Vector3(Bounds.Width * 0.5f, Bounds.Height * 0.5f, 0));

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
        /// Transforms the vector2 from screen space to world space.
        /// </summary>
        /// <param name="input">the vector to be transformed.</param>
        /// <returns>the transformed vector</returns>
        internal static Vector2 stw(Vector2 input)
        {
            return Vector2.Transform(input, Matrix.Invert(Camera.TransformMatrix));
        }

        /// <summary>
        /// Transforms the vector2 from world space to screen space.
        /// </summary>
        /// <param name="input">the vector to be transformed.</param>
        /// <returns>the transformed vector</returns>
        internal static Vector2 wts(Vector2 input)
        {
            return Vector2.Transform(input, Camera.TransformMatrix);
        }

        /// <summary>
        /// this method returns a rectangle which is the visible area. 
        /// </summary>
        internal static Rectangle VisibleArea
        {
            get
            {
                Matrix inverseViewMatrix = Matrix.Invert(TransformMatrix);
                Vector2 tl = Vector2.Transform(Vector2.Zero, inverseViewMatrix);
                Vector2 tr = Vector2.Transform(new Vector2(Game.screenSize.X, 0), inverseViewMatrix);
                Vector2 bl = Vector2.Transform(new Vector2(0, Game.screenSize.Y), inverseViewMatrix);
                Vector2 br = Vector2.Transform(Game.screenSize.ToVector2(), inverseViewMatrix);
                Vector2 min = new Vector2(
                    MathHelper.Min(tl.X, MathHelper.Min(tr.X, MathHelper.Min(bl.X, br.X))),
                    MathHelper.Min(tl.Y, MathHelper.Min(tr.Y, MathHelper.Min(bl.Y, br.Y))));
                Vector2 max = new Vector2(
                    MathHelper.Max(tl.X, MathHelper.Max(tr.X, MathHelper.Max(bl.X, br.X))),
                    MathHelper.Max(tl.Y, MathHelper.Max(tr.Y, MathHelper.Max(bl.Y, br.Y))));
                return new Rectangle((int)min.X, (int)min.Y, (int)(max.X - min.X), (int)(max.Y - min.Y));
            }
        }
    }
}
