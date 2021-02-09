using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TileGame.Code.GameObjects.Default;

namespace TileGame.Code.Utils
{
    internal class Quadtree
    {
        private readonly int MAX_OBJECTS = 20;
        private readonly int MAX_LEVELS = 5;

        private readonly int level;
        private readonly List<GameObject> objects;
        private Rectangle bounds;
        private readonly Quadtree[] nodes;

        /// <summary>
        /// creates a quadtree based on the depth of the current quad and with the current bounds.
        /// </summary>
        /// <param name="pLevel">the depth of the node.</param>
        /// <param name="pBounds">the bounds of the node.</param>
        public Quadtree(int pLevel, Rectangle pBounds)
        {
            level = pLevel;
            objects = new List<GameObject>();
            bounds = pBounds;
            nodes = new Quadtree[4];
        }
        /// <summary>
        /// clears the quadtree, and recursively removes the children of this node.
        /// </summary>
        internal void clear()
        {
            objects.Clear();

            for (int i = 0; i < nodes.Length; i++)
            {
                if (nodes[i] != null)
                {
                    nodes[i].clear();
                    nodes[i] = null;
                }
            }
        }
        private void split()
        {
            int subWidth = bounds.Width / 2;
            int subHeight = bounds.Height / 2;
            int x = bounds.X;
            int y = bounds.Y;

            nodes[0] = new Quadtree(level + 1, new Rectangle(x + subWidth, y, subWidth, subHeight));
            nodes[1] = new Quadtree(level + 1, new Rectangle(x, y, subWidth, subHeight));
            nodes[2] = new Quadtree(level + 1, new Rectangle(x, y + subHeight, subWidth, subHeight));
            nodes[3] = new Quadtree(level + 1, new Rectangle(x + subWidth, y + subHeight, subWidth, subHeight));
        }
        private int getIndex(GameObject go)
        {
            int index = -1;
            double verticalMidpoint = bounds.X + (bounds.Width / 2);
            double horizontalMidpoint = bounds.Y + (bounds.Height / 2);

            // Object can completely fit within the top quadrants
            bool topQuadrant = (go.BoundingBox.Y < horizontalMidpoint && go.BoundingBox.Y + go.BoundingBox.Height < horizontalMidpoint);
            // Object can completely fit within the bottom quadrants
            bool bottomQuadrant = (go.BoundingBox.Y > horizontalMidpoint);

            // Object can completely fit within the left quadrants
            if (go.BoundingBox.X < verticalMidpoint && go.BoundingBox.X + go.BoundingBox.Width < verticalMidpoint)
            {
                if (topQuadrant)
                {
                    index = 1;
                }
                else if (bottomQuadrant)
                {
                    index = 2;
                }
            }
            // Object can completely fit within the right quadrants
            else if (go.BoundingBox.X > verticalMidpoint)
            {
                if (topQuadrant)
                {
                    index = 0;
                }
                else if (bottomQuadrant)
                {
                    index = 3;
                }
            }

            return index;
        }

        //code that returns the index of the quadrants where the gameobject is located
        private List<int> getDoubleIndex(GameObject go)
        {
            Rectangle bb = go.BoundingBox;
            //set midpoints of quad
            double verticalMidpoint = bounds.X + (bounds.Width / 2);
            double horizontalMidpoint = bounds.Y + (bounds.Height / 2);

            List<int> output = new List<int>();
            //top-left
            if (bb.X < verticalMidpoint && bb.Y < horizontalMidpoint)
            {
                output.Add(1);
            }
            //top-right
            if (bb.X + bb.Width > verticalMidpoint && bb.Y < horizontalMidpoint)
            {
                output.Add(0);
            }
            //bottom-left
            if (bb.X < verticalMidpoint && bb.Y + bb.Height > horizontalMidpoint)
            {
                output.Add(2);
            }
            //bottom-right
            if (bb.X + bb.Width > verticalMidpoint && bb.Y + bb.Height > horizontalMidpoint)
            {
                output.Add(3);
            }

            return output;
        }

        /// <summary>
        /// inserts an object into the quadtree.
        /// </summary>
        /// <param name="go">the object to be inserted.</param>
        internal void insert(GameObject go)
        {
            if (nodes[0] != null)
            {
                int index = getIndex(go);

                if (index != -1)
                {
                    nodes[index].insert(go);

                    return;
                }
                else
                {
                    //add the nodes from the quadrants if it spans multiple
                    List<int> doubleIndex = getDoubleIndex(go);
                    foreach (int x in doubleIndex)
                    {
                        if (x < nodes.Length)
                        {
                            nodes[x]?.insert(go);
                        }
                    }
                }
            }

            objects.Add(go);

            if (objects.Count > MAX_OBJECTS && level < MAX_LEVELS)
            {
                if (nodes[0] == null)
                {
                    split();
                }

                int i = 0;
                while (i < objects.Count)
                {
                    int index = getIndex(objects[i]);
                    if (index != -1)
                    {
                        nodes[index].insert(objects[i]);
                        objects.RemoveAt(i);
                    }
                    else
                    {
                        i++;
                    }
                }
            }
        }

        /// <summary>
        /// gets the possible collision objects with the current object.
        /// </summary>
        /// <param name="returnObjects">the list of objects that can have collision.</param>
        /// <param name="go">the gameobject to compare to.</param>
        /// <returns></returns>
        internal List<GameObject> retrieve(List<GameObject> returnObjects, GameObject go)
        {
            int index = getIndex(go);
            if (index != -1 && nodes[0] != null)
            {
                nodes[index].retrieve(returnObjects, go);
            }
            else
            {
                //add the nodes from the quadrants if it spans multiple
                List<int> doubleIndex = getDoubleIndex(go);
                foreach (int x in doubleIndex)
                {
                    nodes[x]?.retrieve(returnObjects, go);
                }
            }


            returnObjects.AddRange(objects);

            return returnObjects;
        }

#if DEBUG
        internal void Draw(SpriteBatch batch)
        {
            foreach(GameObject g in objects)
            {
                batch.Draw(Game.game.empty_texture, g.BoundingBox, Color.Yellow * 0.5f);
            }
        }
#endif
    }
}
