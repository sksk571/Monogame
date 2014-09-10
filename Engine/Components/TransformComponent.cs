using Microsoft.Xna.Framework;

namespace Engine.Components
{
    public struct TransformComponent
    {
        public readonly Vector2 Position;
		public readonly float Rotation;
        public readonly Vector2 Scale;

		public TransformComponent(Vector2 position)
			: this(position, 0f)
		{
		}

        public TransformComponent(Vector2 position, float rotation)
            : this(position, rotation, Vector2.One)
        {
        }

        public TransformComponent(Vector2 position, float rotation, Vector2 scale)
        {
            Position = position;
            Rotation = rotation;
            Scale = scale;
        }
    }
}
