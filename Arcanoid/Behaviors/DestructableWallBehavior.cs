﻿using Engine;

namespace Arcanoid.Behaviors
{
    public class DestructableWallBehavior : ICollisionBehavior, ICollisionDispatch
    {
        public void HandleCollision(Entity entity, Entity other)
        {
            if (other.HasComponent<CollisionBehaviorComponent> ())
            {
                ICollisionDispatch collisionDispatch = other.GetComponent<CollisionBehaviorComponent> ()
                    .Behavior as ICollisionDispatch;
                if (collisionDispatch != null)
                {
                    collisionDispatch.CollideWall(other, entity);
                }
            }
        }

        public void CollideBall (Entity entity, Entity ball)
        {
            entity.Destroy ();
        }

        public void CollideWall (Entity entity, Entity wall)
        {
            // do nothing
        }
    }
}
