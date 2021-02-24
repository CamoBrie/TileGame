using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileGame.Code.GameObjects.Data
{
    class ContentLibrary<T>
    {
        private Dictionary<string, T> library = new Dictionary<string, T>();

        T emptyObject;

        T missingObject;
        internal ContentLibrary(T emptyObject, T missingObject)
        { 
            this.emptyObject = emptyObject;
            this.missingObject = missingObject;
        }

        internal T Get(string path)
        {
            if (path.Length == 0)
            {
                Console.WriteLine($"(ContentLibrary:) {path} is empty.");
                return emptyObject;
            }

            //get texture from dict, and if it is not in it, add it to the dict.
            if (library.TryGetValue(path, out T content))
            {
                Console.WriteLine($"(ContentLibrary:) {path} retrieved.");
                return content;
            }
            else
            {
                T newContent;
                try
                {
                    newContent = Game.game.Content.Load<T>(path);
                    library.Add(path, newContent);
                }
                catch (ContentLoadException)
                {
                    Console.WriteLine($"(ContentLibrary:) {path} is missing.");
                    newContent = missingObject;
                }
                return newContent;
            }
        }

        internal void Clear()
        {
            library.Clear();
        }
    }
}
