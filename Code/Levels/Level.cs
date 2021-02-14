using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using TileGame.Code.GameObjects;
using TileGame.Code.GameObjects.Default;
using TileGame.Code.GameObjects.Default.Drawing;
using TileGame.Code.Utils;

namespace TileGame.Levels
{
    internal class Level
    {
        internal readonly Player player;

        #region Object Lists
        private readonly List<GameObject> entities = new List<GameObject>();
        private readonly List<SpriteObject> spriteTiles = new List<SpriteObject>();
        private readonly List<CollisionObject> collisionTiles = new List<CollisionObject>();

        #endregion

        #region Camera Variables
        private Rectangle bounds;
        private Rectangle VisibleScreen;
        #endregion

        #region Grid Variables
        private readonly LevelGrid grid;
        #endregion

        /// <summary>
        /// the quadtree for collision
        /// </summary>
        private readonly Quadtree quadTree;
        
        /// <summary>
        /// creates the level object, it handles everything, from updating the collision, to drawing to the screen.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="player"></param>
        internal Level(string path, ref Player player)
        {
            #region Initial Setup
            this.player = player;
            AddEntity(player);

            grid = new LevelGrid(this, 32, 32, path);

            quadTree = new Quadtree(0, new Rectangle(0, 0, grid.totalWidth, grid.totalHeight));
            bounds = new Rectangle(0, 0, grid.totalWidth,grid.totalHeight);
            GenerateWallBounds();
            #endregion

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
            entities.Add(entity);
        }

        /// <summary>
        /// removes the entity provided from the list
        /// </summary>
        /// <param name="entity"></param>
        internal void RemoveEntity(GameObject entity)

        {
            entities.Remove(entity);
        }

        #endregion

        /// <summary>
        /// the function that is called to update certain things, such as the current player position.
        /// </summary>
        internal void Update()
        {
            #region Camera
            Camera.Location = GetCameraLocation(player.globalPosition, bounds, Camera.Bounds);
            VisibleScreen = Camera.VisibleArea;
            #endregion

            #region Collision

            quadTree.clear();

            //add the entities to the tree
            foreach(GameObject go in entities)
            {
                AddToQuadTree(go);
            }

            //add the collisiontiles to the tree
            foreach(CollisionObject ct in collisionTiles)
            {
                AddToQuadTree(ct);
            }

            //add the level tiles to the tree
            foreach (LevelTile lt in grid.GetCollisionTiles())
            {
                AddToQuadTree(lt);
            }

            //fire collision events based on location
            foreach (GameObject entity in entities)
            {
                GetTargetsForCollision(entity);
            }
            #endregion



        }

        #region Collision
        /// <summary>
        /// Gets the collision targets and all of its children recursively.
        /// </summary>
        /// <param name="entity"></param>
        internal void GetTargetsForCollision(GameObject entity)
        {
            List<GameObject> returnObjects = new List<GameObject>();

            quadTree.retrieve(returnObjects, entity);
            foreach (GameObject go in returnObjects)
            {
                go.FireCollisionEvent(entity);
            }
            if (entity.children.Count != 0)
            {
                foreach (GameObject gameObject in entity.children.ToArray())
                {
                    GetTargetsForCollision(gameObject);
                }
            }

        }

        /// <summary>
        /// Adds the gameobject and all its children to the quadtree recursively.
        /// </summary>
        /// <param name="go"></param>
        internal void AddToQuadTree(GameObject go)
        {
            if (TileOnScreen(go) && go.hasCollision)
            {
                quadTree.insert(go);
            }
            if(go.children.Count != 0)
            {
                foreach (GameObject gameObject in go.children.ToArray())
                {
                    AddToQuadTree(gameObject);
                }
            }
        }
        #endregion

        #region Drawing
        /// <summary>
        /// the function where everything is drawn before (under) the player
        /// </summary>
        internal void Pre_draw(SpriteBatch batch)
        {
            grid.Pre_draw(batch);
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
            grid.Post_draw(batch);
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
            return VisibleScreen.Intersects(st.BoundingBox);
        }
        /// <summary>
        /// Determines if the tile is on the screen at the moment, so it needs to be added to the collision list.
        /// </summary>
        /// <param name="st">the tile to check.</param>
        /// <returns>if it needs to be added.</returns>
        private bool TileOnScreen(GameObject st)
        {
            Rectangle bb = st.BoundingBox;
            bb.Inflate(1, 1);
            return VisibleScreen.Intersects(bb);
        }

        #endregion

        internal void HitTile(GameObject sender, MouseState state)
        {
            Vector2 playerPos = player.globalPosition;
            Point mousePos = state.Position;
            Vector2 offset = -(playerPos - mousePos.ToVector2());

            //change to cardinal direction
            if(Math.Abs(offset.X) > Math.Abs(offset.Y))
            {
                offset.X = Math.Sign(offset.X);
                offset.Y = 0;
            } else
            {
                offset.X = 0;
                offset.Y = Math.Sign(offset.Y);
            }

            if(player.tool.ready)
            {
                player.tool.Swing(offset);
            }
        }
    }
}
