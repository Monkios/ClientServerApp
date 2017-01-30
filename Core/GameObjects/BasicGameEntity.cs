using Microsoft.Xna.Framework;

namespace Core.GameObjects
{
    public abstract class BasicGameEntity : IGameEntity
    {
        public GameEntityType EntityType { get; set; }
        public Vector2 Size { get; set; }
        public Vector2 Position { get; set; }
        public Direction Direction { get; set; }

        public BasicGameEntity(GameEntityType type, int x, int y, int width, int height)
        {
            EntityType = type;
            Size = new Vector2(width, height);
            Position = new Vector2(x, y);
            Direction = Direction.Down;
        }
    }
}
