using Microsoft.Xna.Framework;

namespace Engine.Components
{
    public struct MoveComponent
    {
        public readonly Vector2 MoveVector;

        public MoveComponent(Vector2 moveVector)
        {
            MoveVector = moveVector;
        }
    }
}
