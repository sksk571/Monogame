using Microsoft.Xna.Framework.Graphics;

namespace Engine.Components
{
    public struct SpriteComponent
    {
        public readonly Texture2D Sprite;

        public SpriteComponent(Texture2D sprite)
        {
            Sprite = sprite;
        }
    }
}
