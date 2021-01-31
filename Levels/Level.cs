using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileGame.Collision;
using TileGame.GameObjects;

namespace TileGame.Levels
{
    class Level
    {
        private Player player;

        #region Object Lists
        private List<GameObject> entities = new List<GameObject>();
        private List<SpriteObject> spriteTiles = new List<SpriteObject>();
        private List<CollisionObject> collisionTiles = new List<CollisionObject>();

        #endregion

        #region Camera Variables
        private Rectangle bounds;
        private Rectangle VisibleScreen;
        #endregion

        /// <summary>
        /// the quadtree for collision
        /// </summary>
        private Quadtree quadTree;
        
        /// <summary>
        /// creates the level object, it handles everything, from updating the collision, to drawing to the screen.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="player"></param>
        internal Level(string path, ref Player player)
        {
            #region Initial Setup
            this.player = player;
            this.AddEntity(this.player);
            this.quadTree = new Quadtree(0, new Rectangle(0, 0, Game.screenSize.X, Game.screenSize.Y));
            this.bounds = new Rectangle(-100,-100, Game.screenSize.X+100,Game.screenSize.Y+100);
            GenerateWallBounds();
            #endregion

            //TODO: load corresponding files for the sprites, collision etc
            //testing code
            spriteTiles.Add(new SpriteObject(new Vector2(400, 100), 20, 20, "views/game/coll"));
            spriteTiles.Add(new SpriteObject(new Vector2(300, 300), 20, 20, "views/game/coll"));
            spriteTiles.Add(new SpriteObject(new Vector2(500, 500), 20, 20, "views/game/coll"));
            collisionTiles.Add(new CollisionObject(new Vector2(400, 100), 20, 20));
            collisionTiles.Add(new CollisionObject(new Vector2(300, 300), 20, 20));
            collisionTiles.Add(new CollisionObject(new Vector2(500, 500), 20, 20));

        }

        /// <summary>
        /// generates and adds walls to the collisiontiles where the level boundary is.
        /// </summary>
        internal void GenerateWallBounds()
        {
            collisionTiles.Add(new CollisionObject(new Vector2(bounds.X + bounds.Width/2, bounds.Y - 10), bounds.Width, 20));
            collisionTiles.Add(new CollisionObject(new Vector2(bounds.X + bounds.Width/2, bounds.Bottom + 10), bounds.Width, 20));
            collisionTiles.Add(new CollisionObject(new Vector2(bounds.X - 10, bounds.Y + bounds.Height/2), 20, bounds.Height));
            collisionTiles.Add(new CollisionObject(new Vector2(bounds.Right + 10, bounds.Y + bounds.Height / 2), 20, bounds.Height));
        }

        #region EntityList
        /// <summary>
        /// adds the entity provided to the list
        /// </summary>
        /// <param name="entity"></param>
        internal void AddEntity(GameObject entity)
        {
            this.entities.Add(entity);
        }

        /// <summary>
        /// removes the entity provided from the list
        /// </summary>
        /// <param name="entity"></param>
        internal void RemoveEntity(GameObject entity)

        {
            this.entities.Remove(entity);
        }

        #endregion

        /// <summary>
        /// the function that is called to update certain things, such as the current player position.
        /// </summary>
        internal void Update()
        {
            #region Collision

            quadTree.clear();

            //add the entities to the tree
            foreach(GameObject go in this.entities)
            {


                if (TileOnScreen(go))
                {
                    quadTree.insert(go);
                }

            }

            //add the collisiontiles to the tree
            foreach(CollisionObject ct in this.collisionTiles)
            {
                if (TileOnScreen(ct))
                {
                    quadTree.insert(ct);
                }
            }

            //fire collision events based on location
            foreach (GameObject entity in this.entities)
            {
                List<GameObject> returnObjects = new List<GameObject>();

                quadTree.retrieve(returnObjects, entity);
                foreach (GameObject go in returnObjects)
                {
                    go.FireCollisionEvent(entity);
                }
            }
            #endregion

            #region Camera
            Camera.Location = GetCameraLocation(player.centerPosition, this.bounds, Camera.Bounds);
            this.VisibleScreen = Camera.VisibleArea;
            #endregion
        }

        #region Drawing
        /// <summary>
        /// the function where everything is drawn before (under) the player
        /// </summary>
        internal void Pre_draw(SpriteBatch batch)
        {
            foreach (SpriteObject st in GetDrawTiles(true))
            {
                st.Draw(batch);
            }
        }

        /// <summary>
        /// the function where everything is drawn after (above) the player
        /// </summary>
        internal void Post_draw(SpriteBatch batch)
        {
            foreach (SpriteObject st in GetDrawTiles(false))
            {
                st.Draw(batch);
            }

#if DEBUG
            quadTree.Draw(batch);
#endif
        }

        /// <summary>
        /// get the tiles which need to be drawn corresponding to the players position.
        /// </summary>
        /// <param name="before"></param>
        /// <returns></returns>
        private List<SpriteObject> GetDrawTiles(bool before)
        {
            List<SpriteObject> tiles = new List<SpriteObject>();

            if (before)
            {
                foreach (SpriteObject st in spriteTiles)
                {
                    if (st.centerPosition.Y < player.centerPosition.Y && TileOnScreen(st))
                    {
                        tiles.Add(st);
                    }
                }
            } else
            {
                foreach (SpriteObject st in spriteTiles)
                {
                    if (st.centerPosition.Y > player.centerPosition.Y && TileOnScreen(st))
                    {
                        tiles.Add(st);
                    }
                }
            }
            return tiles;
        }

        #endregion

        #region Camera Utils

        /// <summary>
        /// gets the camera location for the next cycle
        /// </summary>
        /// <param name="playerPosition">the position of the player</param>
        /// <param name="levelBounds">the bounds of the level</param>
        /// <param name="cameraBounds">the bounds of the camera view</param>
        /// <returns></returns>
        private Vector2 GetCameraLocation(Vector2 playerPosition, Rectangle levelBounds, Rectangle cameraBounds)
        {
            Vector2 min = new Vector2(levelBounds.X + cameraBounds.Width/2, levelBounds.Y + cameraBounds.Height/2);
            Vector2 max = new Vector2(levelBounds.Right - cameraBounds.Width/2, levelBounds.Bottom - cameraBounds.Height/2);
            return Vector2.Clamp(playerPosition, min, max);
        }

        /// <summary>
        /// Determines if the tile is on the screen at the moment, so it needs to be drawn.
        /// </summary>
        /// <param name="st">the tile to check.</param>
        /// <returns></returns>
        private bool TileOnScreen(SpriteObject st)
        {
            return VisibleScreen.Intersects(st.GetBoundingBox());
        }
        /// <summary>
        /// Determines if the tile is on the screen at the moment, so it needs to be added to the collision list.
        /// </summary>
        /// <param name="st">the tile to check.</param>
        /// <returns>if it needs to be added.</returns>
        private bool TileOnScreen(GameObject st)
        {
            Rectangle bb = st.GetBoundingBox();
            bb.Inflate(1, 1);
            return VisibleScreen.Intersects(bb);
        }

        #endregion
    }
}
