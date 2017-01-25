using Microsoft.Xna.Framework;

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
