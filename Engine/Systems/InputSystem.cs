using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Engine
{
    public class InputSystem : System
    {
        public override void Update (World world, GameTime gameTime)
        {
            KeyboardState keyboard = Keyboard.GetState ();
            MouseState mouse = Mouse.GetState ();
            foreach (Entity entity in world.Entities.WithComponent<InputBehaviorComponent>())
            {
                IInputBehavior inputBehavior = entity.GetComponent<InputBehaviorComponent> ().Behavior;
                inputBehavior.HandleInput (entity, keyboard, mouse);
            }
        }
    }
}

