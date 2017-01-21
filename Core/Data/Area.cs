using Core.GameObjects;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Core.Data
{
    public class Area
    {
        public Vector2 Size { get; set; }
        public List<IGameEntity> Entities { get; set; }

        public Area()
        {
            Entities = new List<IGameEntity>();
        }

        public Area(string jsonData)
        {
            Area a = JsonConvert.DeserializeObject<Area>(jsonData);

            Size = a.Size;
            Entities = a.Entities;
        }

        public string ToJSON()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
