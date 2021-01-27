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
        private List<GameObject> entities = new List<GameObject>();
        private List<SpriteTile> spriteTiles = new List<SpriteTile>();
        private List<CollisionTile> collisionTiles = new List<CollisionTile>();

        /// <summary>
        /// the quadtree for collision
        /// </summary>
        private Quadtree quadTree = new Quadtree(0, new Rectangle(0,0, Game.screenSize.X, Game.screenSize.Y));

        internal Level(string path)
        {
            //TODO: load corresponding files for the sprites, collision etc
        }

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
                /* TODO: create the inbounds method for making sure the tile is on the screen
                if(inBounds(ct))
                {
                    quadTree.insert(ct);
                }
                */
                quadTree.insert(go);
            }

            //add the collisiontiles to the tree
            foreach(CollisionTile ct in this.collisionTiles)
            {
                /* TODO: create the inbounds method for making sure the tile is on the screen
                if(inBounds(ct))
                {
                    quadTree.insert(ct);
                }
                */
                quadTree.insert(ct);
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
        }

        #region Drawing
        /// <summary>
        /// the function where everything is drawn before (under) the player
        /// </summary>
        internal void pre_draw(SpriteBatch batch)
        {
            foreach(SpriteTile st in getDrawTiles(true))
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
                    if (st.centerPosition.Y < playerPosition.Y)
                    {
                        tiles.Add(st);
                    }
                }
            } else
            {
                foreach (SpriteTile st in spriteTiles)
                {
                    if (st.centerPosition.Y > playerPosition.Y)
                    {
                        tiles.Add(st);
                    }
                }
            }

            return tiles;
        }

        #endregion
    }
}
