using Microsoft.Xna.Framework;

namespace Engine.Components
{
    public struct PositionComponent
    {
        public readonly Vector2 Position;
		public readonly float Rotation;

		public PositionComponent(Vector2 position)
			: this(position, 0f)
		{
		}

		public PositionComponent(Vector2 position, float rotation)
        {
            Position = position;
			Rotation = rotation;
        }
    }
}
