using System;
using Microsoft.Xna.Framework;

namespace Engine
{
    public struct SpriteBoundsComponent
    {
        public Rectangle Bounds;

        public SpriteBoundsComponent(Rectangle bounds)
        {
            Bounds = bounds;
        }
    }
}

