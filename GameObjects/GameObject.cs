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
        internal List<GameObject> children = new List<GameObject>();
        internal Vector2 centerPosition;
        internal int width, height;
        internal Texture2D sprite;
        internal int ID = 0;
        internal GameObject parent;
        protected List<int> associatedClicks = new List<int>();

        

        #region Events
        internal delegate void collisionEvent(GameObject sender, GameObject collider);
        internal delegate void MouseEvent(GameObject sender, MouseState mouse);
        internal delegate void KeyEvent(GameObject sender);
        internal event collisionEvent onCollisionDetected;
        internal event MouseEvent onMouseDown;
        internal event MouseEvent onMouseMoved;
        internal event MouseEvent onMouseUp;
        internal event MouseEvent onMouseDrag;
        internal event KeyEvent onKeyChange;
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
            this.sprite = Game.game.getSprite(assetName);
            this.centerPosition = center;
            this.width = width;
            this.height = height;
            this.ID = Game.game.getUniqueGameObjectID();
        }

        #region General Functions
        internal virtual void handleInput()
        {
            // Handle input in all the children from the template method.
            foreach (GameObject child in this.children.ToArray())
            {
                child.handleInput();
            }

            if (InputManager.didAKeyChange)
                this.onKeyChange?.Invoke(this);

            if (InputManager.didTheMouseClick)
                this.mouseDown(InputManager.MouseState, InputManager.getClickID());

            if (InputManager.didTheMouseMove)
                this.mouseMoved(InputManager.MouseState);

            if (InputManager.doesTheMouseDrag)
                this.mouseDrag(InputManager.MouseState);

            if (InputManager.didTheMouseRelease)
                this.mouseUp(InputManager.MouseState, InputManager.getClickID());
        }
        internal virtual void update(GameTime time)
        {
            foreach (GameObject child in this.children.ToArray())
            {
                child.update(time);
            }
        }
        internal virtual void draw(SpriteBatch batch)
        {
            //First draw your own sprite
            this.drawOwnSprite(batch);

            // Then draw your children
            foreach (GameObject child in this.children)
            {
                child.draw(batch);
            }
        }
        internal virtual Rectangle getBoundingBox()
        {
            return new Rectangle((int)this.centerPosition.X - width / 2, (int)this.centerPosition.Y - height / 2, width, height);
        }
        internal virtual void drawOwnSprite(SpriteBatch batch)
        {
            batch.Draw(this.sprite, this.getBoundingBox(), Color.White);
        }
        internal virtual void drawOwnSprite(SpriteBatch batch, Color color)
        {
            batch.Draw(this.sprite, this.getBoundingBox(), color);
        }
        #endregion

        #region Event Functions
        internal void fireCollisionEvent(GameObject other)
        {
            if (other.ID != this.ID && this.getBoundingBox().Intersects(other.getBoundingBox()))
                this.onCollisionDetected?.Invoke(this, other);
        }
        protected void mouseUp(MouseState mouse, int clickID)
        {
            if (this.associatedClicks.Contains(clickID))
            {
                this.associatedClicks.Remove(clickID);
                this.onMouseUp?.Invoke(this, mouse);
            }
        }
        protected void mouseDrag(MouseState mouse)
        {
            if (InputManager.dragObjects.Contains(this.ID))
                this.onMouseDrag?.Invoke(this, mouse);
        }
        protected void mouseMoved(MouseState mouse)
        {
            this.onMouseMoved?.Invoke(this, mouse);
        }
        protected void mouseDown(MouseState mouse, int clickID)
        {
            // Check if the touch is inside the bounding box.
            if (this.getBoundingBox().Contains(mouse.Position))
            {
                // Invoke the event
                this.associatedClicks.Add(clickID);
                this.onMouseDown?.Invoke(this, mouse);
            }
        }
        #endregion
    }
}
