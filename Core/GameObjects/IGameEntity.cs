using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.GameObjects
{
    public interface IGameEntity
    {
        GameEntityType EntityType { get; set; }
        Vector2 Size { get; set; }
        Vector2 Position { get; set; }
        Direction Direction { get; set; }
    }
}
