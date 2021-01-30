using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileGame.Collision;
using TileGame.GameObjects;
using TileGame.Levels.Tiles;

namespace TileGame.Levels
{
    class Level
    {
        private Player player;

        #region Object Lists
        private List<GameObject> entities = new List<GameObject>();
        private List<SpriteTile> spriteTiles = new List<SpriteTile>();
        private List<CollisionTile> collisionTiles = new List<CollisionTile>();

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
            this.addEntity(this.player);
            this.quadTree = new Quadtree(0, new Rectangle(0, 0, Game.screenSize.X, Game.screenSize.Y));
            this.bounds = new Rectangle(-100,-100, Game.screenSize.X+100,Game.screenSize.Y+100);
            generateWallBounds();
            #endregion

            //TODO: load corresponding files for the sprites, collision etc
            //testing code
            spriteTiles.Add(new SpriteTile(new Vector2(400, 100), Vector2.Zero, "views/game/coll"));
            spriteTiles.Add(new SpriteTile(new Vector2(300, 300), Vector2.Zero, "views/game/coll"));
            spriteTiles.Add(new SpriteTile(new Vector2(500, 500), Vector2.Zero, "views/game/coll"));
            collisionTiles.Add(new CollisionTile(new Vector2(400, 100), 20, 20));
            collisionTiles.Add(new CollisionTile(new Vector2(300, 300), 20, 20));
            collisionTiles.Add(new CollisionTile(new Vector2(500, 500), 20, 20));

        }

        /// <summary>
        /// generates and adds walls to the collisiontiles where the level boundary is.
        /// </summary>
        internal void generateWallBounds()
        {
            collisionTiles.Add(new CollisionTile(new Vector2(bounds.X + bounds.Width/2, bounds.Y - 10), bounds.Width, 20));
            collisionTiles.Add(new CollisionTile(new Vector2(bounds.X + bounds.Width/2, bounds.Bottom + 10), bounds.Width, 20));
            collisionTiles.Add(new CollisionTile(new Vector2(bounds.X - 10, bounds.Y + bounds.Height/2), 20, bounds.Height));
            collisionTiles.Add(new CollisionTile(new Vector2(bounds.Right + 10, bounds.Y + bounds.Height / 2), 20, bounds.Height));
        }

        #region EntityList
        /// <summary>
        /// adds the entity provided to the list
        /// </summary>
        /// <param name="entity"></param>
        internal void addEntity(GameObject entity)
        {
            this.entities.Add(entity);
        }

        /// <summary>
        /// removes the entity provided from the list
        /// </summary>
        /// <param name="entity"></param>
        internal void removeEntity(GameObject entity)

        {
            this.entities.Remove(entity);
        }

        #endregion

        /// <summary>
        /// the function that is called to update certain things, such as the current player position.
        /// </summary>
        internal void update()
        {
            #region Collision

            quadTree.clear();

            //add the entities to the tree
            foreach(GameObject go in this.entities)
            {


                if (tileOnScreen(go))
                {
                    quadTree.insert(go);
                }

            }

            //add the collisiontiles to the tree
            foreach(CollisionTile ct in this.collisionTiles)
            {
                if (tileOnScreen(ct))
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
                    go.fireCollisionEvent(entity);
                }
            }
            #endregion

            #region Camera
            Camera.Location = getCameraLocation(player.centerPosition, this.bounds, Camera.Bounds);
            this.VisibleScreen = Camera.VisibleArea;
            #endregion
        }

        #region Drawing
        /// <summary>
        /// the function where everything is drawn before (under) the player
        /// </summary>
        internal void pre_draw(SpriteBatch batch)
        {
            foreach (SpriteTile st in getDrawTiles(true))
            {
                st.draw(batch);
            }
        }

        /// <summary>
        /// the function where everything is drawn after (above) the player
        /// </summary>
        internal void post_draw(SpriteBatch batch)
        {
            foreach (SpriteTile st in getDrawTiles(false))
            {
                st.draw(batch);
            }
            
        }

        /// <summary>
        /// get the tiles which need to be drawn corresponding to the players position.
        /// </summary>
        /// <param name="before"></param>
        /// <returns></returns>
        private List<SpriteTile> getDrawTiles(bool before)
        {
            List<SpriteTile> tiles = new List<SpriteTile>();

            if (before)
            {
                foreach (SpriteTile st in spriteTiles)
                {
                    if (st.centerPosition.Y < player.centerPosition.Y && tileOnScreen(st))
                    {
                        tiles.Add(st);
                    }
                }
            } else
            {
                foreach (SpriteTile st in spriteTiles)
                {
                    if (st.centerPosition.Y > player.centerPosition.Y && tileOnScreen(st))
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
        private Vector2 getCameraLocation(Vector2 playerPosition, Rectangle levelBounds, Rectangle cameraBounds)
        {
            Vector2 min = new Vector2(levelBounds.X + cameraBounds.Width/2, levelBounds.Y + cameraBounds.Height/2);
            Vector2 max = new Vector2(levelBounds.Right - cameraBounds.Width/2, levelBounds.Bottom - cameraBounds.Height/2);
            return Vector2.Clamp(playerPosition, min, max);
        }

        /// <summary>
        /// determines if the tile is on the screen at the moment, so it needs to be drawn
        /// </summary>
        /// <param name="st">the tile to check</param>
        /// <returns></returns>
        private bool tileOnScreen(SpriteTile st)
        {
            /*Vector2 tl = new Vector2(st.centerPosition.X - st.width / 2, st.centerPosition.Y - st.height / 2);

            return tl.X + st.width > VisibleScreen.Location.X && tl.Y + st.height > VisibleScreen.Location.Y &&
                tl.X < VisibleScreen.Location.X + VisibleScreen.Width && tl.Y < VisibleScreen.Location.Y + VisibleScreen.Height;*/
            return VisibleScreen.Intersects(st.getBoundingBox());
        }
        private bool tileOnScreen(GameObject st)
        {
            Vector2 tl = new Vector2(st.centerPosition.X - st.width / 2, st.centerPosition.Y - st.height / 2);

            //we need this extra pixel for the borders of the level.
            return tl.X + st.width + 1 > VisibleScreen.Location.X && tl.Y + st.height + 1 > VisibleScreen.Location.Y &&
            tl.X - 1 < VisibleScreen.Location.X + VisibleScreen.Width && tl.Y - 1 < VisibleScreen.Location.Y + VisibleScreen.Height;
            
        }

        #endregion
    }
}
