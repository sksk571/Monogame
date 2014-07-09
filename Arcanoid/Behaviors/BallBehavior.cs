using System;
using Engine;

namespace Arcanoid
{
    public class BallBehavior : ICollisionBehavior, ICollisionDispatch
    {
        public void HandleCollision (Entity entity, Entity other)
        {
            if (other.HasComponent<CollisionBehaviorComponent> ())
            {
                ICollisionDispatch collisionDispatch = other.GetComponent<CollisionBehaviorComponent> ()
                    .Behavior as ICollisionDispatch;
                if (collisionDispatch != null)
                {
                    collisionDispatch.CollideBall(other, entity);
                }
            }
        }

        public void CollideBall (Entity entity, Entity ball)
        {
            // omg! do nothing
        }

        public void CollideWall (Entity entity, Entity wall)
        {
            // do nothing
        }
    }
}

