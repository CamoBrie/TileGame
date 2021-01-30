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
    class GameObject :AGameObject
    {
        internal List<GameObject> children = new List<GameObject>();
        internal GameObject parent;
        protected List<int> associatedClicks = new List<int>();

        

        #region Events
        internal delegate void collisionEvent(GameObject sender, GameObject collider);
        internal delegate void MouseEvent(GameObject sender, MouseState mouse);
        internal delegate void KeyEvent(GameObject sender);
        internal event collisionEvent OnCollisionDetected;
        internal event MouseEvent OnMouseDown;
        internal event MouseEvent OnMouseMoved;
        internal event MouseEvent OnMouseUp;
        internal event MouseEvent OnMouseDrag;
        internal event KeyEvent OnKeyChange;
        #endregion

        /// <summary>
        /// This is the standard constructor of the GameObject.
        /// Currently, every GameObject that is created, uses this constructor to get created.
        /// This function sets the basic atrributes of the GameObject and assigns it a unique ID.
        /// </summary>
        /// <param name="center"> A point that sets the center of this object. This center can be used for movement of the object. </param>
        /// <param name="width"> An integer that specifies the width of the object. </param>
        /// <param name="height"> An integer that specifies the height of the object </param>
        /// <param name="assetName"> The name of the sprite asset that this GameObject will use. </param>
        /// <param name="draggable"> A boolean that specifies if this Game Object can be dragged. </param>
        internal GameObject(Vector2 center, int width, int height, string assetName)
        {
            // GameObjects are responsible for their own data, not the data stored in the entire game.
            // This is why the GameObject asks the current Game Instance if it could please get a sprite.
            this.sprite = Game.game.GetSprite(assetName);
            this.centerPosition = center;
            this.width = width;
            this.height = height;
            this.ID = Game.game.GetUniqueGameObjectID();
        }

        #region General Functions
        internal override void HandleInput()
        {
            // Handle input in all the children from the template method.
            foreach (GameObject child in this.children.ToArray())
            {
                child.HandleInput();
            }

            if (InputManager.didAKeyChange)
                this.OnKeyChange?.Invoke(this);

            if (InputManager.didTheMouseClick)
                this.MouseDown(InputManager.MouseState, InputManager.getClickID());

            if (InputManager.didTheMouseMove)
                this.MouseMoved(InputManager.MouseState);

            if (InputManager.doesTheMouseDrag)
                this.MouseDrag(InputManager.MouseState);

            if (InputManager.didTheMouseRelease)
                this.MouseUp(InputManager.MouseState, InputManager.getClickID());
        }
        internal override void Update(GameTime time)
        {
            foreach (GameObject child in this.children.ToArray())
            {
                child.Update(time);
            }
        }
        internal override void Draw(SpriteBatch batch)
        {
            //First draw your own sprite
            this.DrawOwnSprite(batch);

            // Then draw your children
            foreach (GameObject child in this.children)
            {
                child.Draw(batch);
            }
        }
        internal override Rectangle GetBoundingBox()
        {
            return new Rectangle((int)this.centerPosition.X - width / 2, (int)this.centerPosition.Y - height / 2, width, height);
        }
        internal override void DrawOwnSprite(SpriteBatch batch)
        {
            batch.Draw(this.sprite, this.GetBoundingBox(), Color.White);
        }
        internal override void DrawOwnSprite(SpriteBatch batch, Color color)
        {
            batch.Draw(this.sprite, this.GetBoundingBox(), color);
        }
        #endregion

        #region Event Functions
        internal void FireCollisionEvent(GameObject other)
        {
            if (other.ID != this.ID && this.GetBoundingBox().Intersects(other.GetBoundingBox()))
                this.OnCollisionDetected?.Invoke(this, other);
        }
        protected void MouseUp(MouseState mouse, int clickID)
        {
            if (this.associatedClicks.Contains(clickID))
            {
                this.associatedClicks.Remove(clickID);
                this.OnMouseUp?.Invoke(this, mouse);
            }
        }
        protected void MouseDrag(MouseState mouse)
        {
            if (InputManager.dragObjects.Contains(this.ID))
                this.OnMouseDrag?.Invoke(this, mouse);
        }
        protected void MouseMoved(MouseState mouse)
        {
            this.OnMouseMoved?.Invoke(this, mouse);
        }
        protected void MouseDown(MouseState mouse, int clickID)
        {
            // Check if the touch is inside the bounding box.
            if (this.GetBoundingBox().Contains(mouse.Position))
            {
                // Invoke the event
                this.associatedClicks.Add(clickID);
                this.OnMouseDown?.Invoke(this, mouse);
            }
        }
        #endregion
    }
}
