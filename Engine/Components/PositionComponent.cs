using Microsoft.Xna.Framework;

namespace Engine.Components
{
    public struct PositionComponent
    {
        public readonly Vector2 Position;

        public PositionComponent(Vector2 position)
        {
            Position = position;
        }
    }
}
