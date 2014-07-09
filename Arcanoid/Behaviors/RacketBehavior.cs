using System;
using Engine;
using Microsoft.Xna.Framework.Input;
using Engine.Components;
using Microsoft.Xna.Framework;

namespace Arcanoid
{
    public class RacketBehavior : IInputBehavior
    {
        public void HandleInput (Entity entity, KeyboardState keyboard, MouseState mouse)
        {
            var position = entity.GetComponent<PositionComponent> ().Position;
            Point mousePosition = mouse.Position;

            entity.SetComponent<MoveComponent> (new MoveComponent (
                new Vector2 (mousePosition.X - position.X, mousePosition.Y - position.Y) * 10));
        }
    }
}

