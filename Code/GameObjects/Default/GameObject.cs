using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileGame.Code.Interfaces;
using TileGame.Code.Utils;
using static TileGame.Code.Events.CollisionEvent;
using static TileGame.Code.Events.MouseEvent;

namespace TileGame.Code.GameObjects.Default
{
    class GameObject
    {
        #region Base Properties
        /// <summary>
        /// The center position of this object.
        /// </summary>
        internal Vector2 centerPosition;
        /// <summary>
        /// The position on the screen.
        /// </summary>
        internal Vector2 globalPosition
        {
            get
            {
                if (parent != null)
                    return centerPosition + parent.centerPosition;
                else
                    return centerPosition;
            }
        }

        /// <summary>
        /// Gets the collision box of the object.
        /// </summary>
        /// <returns>the rectangle where the object should collide.</returns>
        public virtual Rectangle BoundingBox
        {
            get => GetDrawPos();
            set => boundingBox = value;
        }
        private Rectangle boundingBox;

        /// <summary>
        /// The integers storing the width and the height of the object.
        /// </summary>
        internal int width, height;
        /// <summary>
        /// The list of all the children of this object.
        /// </summary>
        internal List<GameObject> children = new List<GameObject>();
        /// <summary>
        /// The parent of the gameobject.
        /// Centerposition is now relative to the parent.
        /// </summary>
        internal GameObject parent;
        /// <summary>
        /// A list of collisionobjects this gameobject has.
        /// </summary>
        internal List<CollisionObject> collisionObjects = new List<CollisionObject>();
        /// <summary>
        /// returns true, if a gameobject has or is a collisionobject.
        /// </summary>
        internal virtual bool doesCollision { get { return collisionObjects.Count > 0; } }
        /// <summary>
        /// The global ID of the object.
        /// </summary>
        internal int ID;
        /// <summary>
        /// The clicks associated with this object.
        /// </summary>
        protected List<int> associatedClicks = new List<int>();
        #endregion

        #region Events
        /// <summary>
        /// This event fires whenever a collision is detected.
        /// </summary>
        public event collisionEvent OnIntersect;
        /// <summary>
        /// This event fires whenever the mouse goes up.
        /// </summary>
        internal event mouseEvent OnMouseUp;
        /// <summary>
        /// This event fires whenever the mouse goes down.
        /// </summary>
        internal event mouseEvent OnMouseDown;
        #endregion

        internal GameObject(Vector2 center, int width, int height)
        {
            this.centerPosition = center;
            this.width = width;
            this.height = height;
            this.ID = Game.game.GetUniqueGameObjectID();
        }

        /// <summary>
        /// Use this constructor to create a child with the same width and height and position as the parent
        /// </summary>
        /// <param name="parent">parent</param>
        internal GameObject(GameObject parent)
        {
            parent.AddToChildren(this);
            this.centerPosition = Vector2.Zero;
            this.width = parent.width;
            this.height = parent.height;
            this.ID = Game.game.GetUniqueGameObjectID();
        }

        #region General Functions
        /// <summary>
        /// Updates the children of this object.
        /// </summary>
        /// <param name="time">the time object.</param>
        internal virtual void Update(GameTime time)
        {
            foreach (GameObject child in this.children.ToArray())
            {
                child.HandleInput();
                child.Update(time);
            }
        }

        /// <summary>
        /// Draws the children of this object.
        /// </summary>
        /// <param name="batch">the batch object where to draw to.</param>
        internal virtual void Draw(SpriteBatch batch) {
            foreach(GameObject go in this.children.ToArray())
            {
                go.Draw(batch);
            }
        }


        /// <summary>
        /// Gets the rectangle where to draw to.
        /// </summary>
        /// <returns>the rectangle where to draw to.</returns>
        internal virtual Rectangle GetDrawPos()
        {
            return new Rectangle((int)this.globalPosition.X - width / 2, (int)this.globalPosition.Y - height / 2, width, height);
        }

        /// <summary>
        /// Add a gameobject to the list of children and set this as parent
        /// </summary>
        /// <param name="child">the gameobject to add</param>
        internal void AddToChildren(GameObject child)
        {
            children.Add(child);
            child.parent = this;
        }

        /// <summary>
        /// Handles the input for this object.
        /// </summary>
        internal virtual void HandleInput() {
            if (InputManager.didTheMouseClick)
                this.MouseDown(InputManager.MouseState, InputManager.GetClickID());

            if (InputManager.didTheMouseRelease)
                this.MouseUp(InputManager.MouseState, InputManager.GetClickID());
        }
        #endregion

        #region Event Functions
        /// <summary>
        /// Fires a collision event at the other object.
        /// </summary>
        /// <param name="other">the object where we collide with.</param>
        internal void FireCollisionEvent(GameObject other)
        {
            if (other.ID != this.ID && this.BoundingBox.Intersects(other.BoundingBox))
                this.OnIntersect?.Invoke(this, other);
        }

        /// <summary>
        /// Fires the mouse down event.
        /// </summary>
        /// <param name="mouse">the state of the mouse.</param>
        /// <param name="clickID">the associated clickID.</param>
        protected void MouseDown(MouseState mouse, int clickID)
        {
            if (this.BoundingBox.Contains(mouse.Position))
            {
                this.associatedClicks.Add(clickID);
                this.OnMouseDown?.Invoke(this, mouse);
            }
        }
        /// <summary>
        /// Fires the mouse up event.
        /// </summary>
        /// <param name="mouse">the state of the mouse.</param>
        /// <param name="clickID">the associated clickID.</param>
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
