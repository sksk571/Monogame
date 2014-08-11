using System;
using Engine;
using Microsoft.Xna.Framework.Input;
using Engine.Components;
using Microsoft.Xna.Framework;

namespace Arcanoid
{
    public class RacketBehavior : IInputBehavior
    {
        private Rectangle _viewBounds;

        public RacketBehavior(Rectangle viewBounds)
        {
            _viewBounds = viewBounds;
        }

        public void HandleInput (Entity entity, KeyboardState keyboard, MouseState mouse)
        {
            Rectangle bounds = entity.GetComponent<SpriteBoundsComponent> ().Bounds;
            var position = entity.GetComponent<PositionComponent> ().Position;
            int halfX = bounds.Width / 2;
            int targetX = mouse.Position.X > halfX ? 
                (mouse.Position.X < _viewBounds.Width - halfX ? 
                    mouse.Position.X : _viewBounds.Width - halfX) 
                : halfX;
            int targetY = _viewBounds.Height - bounds.Height;
            //entity.SetComponent<PositionComponent> (new PositionComponent (new Vector2 (targetX, targetY)));

            entity.SetComponent<MoveComponent> (new MoveComponent (
                new Vector2 (targetX - position.X, targetY - position.Y) * 5));
        }
    }
}

