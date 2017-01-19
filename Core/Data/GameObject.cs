using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core.Data
{
    public enum GameObjectType
    {
        Basic
    }

    public abstract class GameObject
    {
        public static Vector2 Size;

        public GameObjectType Type { get; protected set; }
        public Vector2 Position;
        public Direction Direction;
    }

    public class BasicObject : GameObject
    {
        public BasicObject(int x, int y, Direction direction)
        {
            Size = new Vector2(15, 16);

            Type = GameObjectType.Basic;
            Position = new Vector2(x, y);
            Direction = direction;
        }
    }
}
