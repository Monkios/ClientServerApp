using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Core.GameObjects
{
    public class Hero : BasicGameEntity
    {
        private static readonly int _width = 16;
        private static readonly int _height = 16;

        public readonly string Name;

        public Hero(string name, int x, int y, Direction direction) : base(GameEntityType.Hero, x, y, _width, _height)
        {
            Name = name;
            Direction = direction;
        }
    }
}
