using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using TileGame.Code.Utils;
using TileGame.Code.Utils.Convenience;
using static TileGame.Code.Events.CollisionEvent;
using static TileGame.Code.Events.MouseEvent;

namespace TileGame.Code.GameObjects.Default
{
    internal class GameObject
    {
        #region Base Properties
        /// <summary>
        /// The center position of this object.
        /// </summary>
        internal Vector2 centerPosition;
        /// <summary>
        /// The position of the element combined with the parent.
        /// </summary>
        internal Vector2 globalPosition
        {
            get
            {
                if (parent != null)
                {
                    return centerPosition + parent.globalPosition;
                }
                else
                {
                    return centerPosition;
                }
            }
        }
        /// <summary>
        /// The integers storing the width and the height of the object.
        /// </summary>
        internal int width, height;
        /// <summary>
        /// The clicks associated with this object.
        /// </summary>
        protected List<int> associatedClicks = new List<int>();
        #endregion

        #region ID & Collision/Inheritance
        /// <summary>
        /// The global ID of the object.
        /// </summary>
        internal int ID;
        /// <summary>
        /// The global ID of the highest parent, and this ID otherwise.
        /// </summary>
        internal int parentID
        {
            get {
                if (parent != null) {
                    return parent.parentID;
                } else
                {
                    return ID;
                }
            }
        }
        internal virtual bool hasCollision
        {
            get
            {
                if (collides)
                {
                    return true;
                }
                if (children.Count == 0)
                {
                    return false;
                }
                foreach (GameObject go in children)
                {
                    if (go.hasCollision)
                    {
                        return true;
                    }

                }
                return false;
            }
        }
        protected bool collides;

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
        /// The list of all the children of this object.
        /// </summary>
        internal List<GameObject> children = new List<GameObject>();
        /// <summary>
        /// The parent of the gameobject.
        /// Centerposition is now relative to the parent.
        /// </summary>
        internal GameObject parent;
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

        /// <summary>
        /// creates a gameobject with the parameters.
        /// </summary>
        /// <param name="center">center position of the object</param>
        /// <param name="width">the width of the object</param>
        /// <param name="height">the height of the object</param>
        /// <param name="collides">if the object should collide or not</param>
        internal GameObject(Vector2 center, int width, int height, bool collides = false)
        {
            centerPosition = center;
            this.width = width;
            this.height = height;
            ID = Game.game.GetUniqueGameObjectID();
            this.collides = collides;
        }

        /// <summary>
        /// only set the ID.
        /// </summary>
        internal GameObject()
        {
            ID = Game.game.GetUniqueGameObjectID();
        }

        /// <summary>
        /// Use this constructor to create a child with the same width and height and position as the parent
        /// </summary>
        /// <param name="parent">parent</param>
        internal GameObject(GameObject parent, bool collides = false)
        {
            parent.AddToChildren(this);
            centerPosition = Vector2.Zero;
            width = parent.width;
            height = parent.height;
            this.collides = collides;
            ID = Game.game.GetUniqueGameObjectID();
        }

        #region General Functions
        /// <summary>
        /// Updates the children of this object.
        /// </summary>
        /// <param name="time">the time object.</param>
        internal virtual void Update(GameTime time)
        {
            foreach (GameObject child in children.ToArray())
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
            foreach(GameObject go in children.ToArray())
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
            return new Rectangle((int)globalPosition.X - width / 2, (int)globalPosition.Y - height / 2, width, height);
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
        /// removes a gameobject from the list of children.
        /// </summary>
        /// <param name="child"></param>
        internal void RemoveFromChildren(GameObject child)
        {
            children.Remove(child);
        }

        /// <summary>
        /// Handles the input for this object.
        /// </summary>
        internal virtual void HandleInput() {
            if (InputManager.didTheMouseClick)
            {
                MouseDown(InputManager.MouseState, InputManager.GetClickID());
            }

            if (InputManager.didTheMouseRelease)
            {
                MouseUp(InputManager.MouseState, InputManager.GetClickID());
            }
        }

        /// <summary>
        /// Set a timer and to excecute an action after a certain time.
        /// </summary>
        /// <param name="duration">Time to wait until performing the action</param>
        /// <param name="method">The action to perform after that time</param>
        internal void SetTimer(float duration, Action method)
        {
            _ = new Timer(this, duration, method);
        }
        #endregion

        #region Event Functions
        /// <summary>
        /// Fires a collision event at the other object.
        /// </summary>
        /// <param name="other">the object where we collide with.</param>
        internal void FireCollisionEvent(GameObject other)
        {
            if (other.parentID != parentID)
            {
                if (BoundingBox.Intersects(other.BoundingBox))
                {
                    OnIntersect?.Invoke(this, other);
                }
            }
        }

        /// <summary>
        /// Fires the mouse down event.
        /// </summary>
        /// <param name="mouse">the state of the mouse.</param>
        /// <param name="clickID">the associated clickID.</param>
        protected void MouseDown(MouseState mouse, int clickID)
        {
            if (BoundingBox.Contains(mouse.Position))
            {
                associatedClicks.Add(clickID);
                OnMouseDown?.Invoke(this, mouse);
            }
        }
        /// <summary>
        /// Fires the mouse up event.
        /// </summary>
        /// <param name="mouse">the state of the mouse.</param>
        /// <param name="clickID">the associated clickID.</param>
        protected void MouseUp(MouseState mouse, int clickID)
        {
            if (associatedClicks.Contains(clickID))
            {
                associatedClicks.Remove(clickID);
                OnMouseUp?.Invoke(this, mouse);
            }
        }
        #endregion
    }
}
