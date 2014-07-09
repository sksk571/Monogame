using System;
using Microsoft.Xna.Framework.Input;

namespace Engine
{
    public interface IInputBehavior
    {
        void HandleInput(Entity entity, KeyboardState keyboard, MouseState mouse);
    }
}

