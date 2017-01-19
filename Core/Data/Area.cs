using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Core.Data
{
    public class Area
    {
        public Vector2 size;
        public List<GameObject> gameObjects;

        public Area()
        {
            gameObjects = new List<GameObject>();
        }

        public Area(string jsonData)
        {
            Area a = JsonConvert.DeserializeObject<Area>(jsonData);
            this.size = a.size;
            this.gameObjects = a.gameObjects;
        }

        public string ToJSON()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
