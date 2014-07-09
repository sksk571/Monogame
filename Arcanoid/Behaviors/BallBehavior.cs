﻿using System;
using Engine;

namespace Arcanoid
{
    public class BallBehavior : CollisionDispatcher
    {
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

