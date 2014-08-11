using System;
using Engine;
using Engine.Components;
using Microsoft.Xna.Framework;

namespace Arcanoid
{
    public class BallBehavior : CollisionDispatcher
    {
        private Random _random;
        public BallBehavior()
        {
            _random = new Random ();
        }

        public override void DispatchCollision (CollisionDispatcher dispatcher, Entity entity, Entity other)
        {
            dispatcher.CollideBall (other, entity);
        }

        public override void CollideBall (Entity entity, Entity ball)
        {
            // omg! do nothing
        }

        public override void CollideWall (Entity entity, Entity wall)
        {
            // do nothing
        }
    }
}

