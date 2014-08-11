using System;
using Microsoft.Xna.Framework.Graphics;

namespace Engine
{
    public struct TextComponent
    {
        public readonly SpriteFont Font;
        public readonly string Text;

        public TextComponent(SpriteFont font, string text)
        {
            Font = font;
            Text = text;
        }
    }
}

