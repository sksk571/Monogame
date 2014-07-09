using System;
using Engine;

namespace Arcanoid
{
    public abstract class CollisionDispatcher : ICollisionBehavior
    {
        public void HandleCollision(Entity entity, Entity other)
        {
            if (other.HasComponent<CollisionBehaviorComponent> ())
            {
                CollisionDispatcher dispatcher = other.GetComponent<CollisionBehaviorComponent> ()
                    .Behavior as CollisionDispatcher;
                if (dispatcher != null)
                {
                    dispatcher.DispatchCollision (this, other, entity);
                }
            }
        }

        public abstract void DispatchCollision(CollisionDispatcher dispatcher, Entity entity, Entity other);

        public abstract void CollideBall (Entity entity, Entity ball);

        public abstract void CollideWall (Entity entity, Entity wall);
    }
}

