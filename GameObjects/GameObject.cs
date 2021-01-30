using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileGame.Input;

namespace TileGame.GameObjects
{
    class GameObject
    {
        #region Base Properties
        internal Vector2 centerPosition;
        internal int width, height;
        internal List<GameObject> children = new List<GameObject>();
        internal int ID;
        protected List<int> associatedClicks = new List<int>();
        #endregion

        #region Events
        internal delegate void collisionEvent(GameObject sender, GameObject collider);
        internal delegate void mouseEvent(GameObject sender, MouseState state);
        internal event collisionEvent OnCollisionDetected;
        internal event mouseEvent OnMouseUp;
        internal event mouseEvent OnMouseDown;
        #endregion

        internal GameObject(Vector2 center, int width, int height)
        {
            this.centerPosition = center;
            this.width = width;
            this.height = height;
            this.ID = Game.game.GetUniqueGameObjectID();
        }

        #region General Functions
        internal virtual void Update(GameTime time)
        {
            foreach (GameObject child in this.children.ToArray())
            {
                child.HandleInput();
                child.Update(time);
            }
        }

        internal virtual void Draw(SpriteBatch batch) {
            foreach(GameObject go in this.children.ToArray())
            {
                go.Draw(batch);
            }
        }

        internal virtual Rectangle GetBoundingBox()
        {
            return new Rectangle((int)this.centerPosition.X - width / 2, (int)this.centerPosition.Y - height / 2, width, height);
        }

        internal virtual Rectangle GetDrawPos()
        {
            return new Rectangle();
        }

        internal virtual void HandleInput() {
            if (InputManager.didTheMouseClick)
                this.MouseDown(InputManager.MouseState, InputManager.GetClickID());

            if (InputManager.didTheMouseRelease)
                this.MouseUp(InputManager.MouseState, InputManager.GetClickID());
        }
        #endregion

        #region Event Functions
        internal void FireCollisionEvent(GameObject other)
        {
            if (other.ID != this.ID && this.GetBoundingBox().Intersects(other.GetBoundingBox()))
                this.OnCollisionDetected?.Invoke(this, other);
        }

        protected void MouseDown(MouseState mouse, int clickID)
        {
            if (this.GetBoundingBox().Contains(mouse.Position))
            {
                this.associatedClicks.Add(clickID);
                this.OnMouseDown?.Invoke(this, mouse);
            }
        }
        protected void MouseUp(MouseState mouse, int clickID)
        {
            if (this.associatedClicks.Contains(clickID))
            {
                this.associatedClicks.Remove(clickID);
                this.OnMouseUp?.Invoke(this, mouse);
            }
        }
        #endregion
    }
}
