using Engine;

namespace Arcanoid.Behaviors
{
    public class WallBehavior : CollisionDispatcher
    {
        public override void DispatchCollision (CollisionDispatcher dispatcher, Entity entity, Entity other)
        {
            dispatcher.CollideWall (other, entity);
        }

        public override void CollideBall (Entity entity, Entity ball)
        {
            entity.Destroy ();
        }

        public override void CollideWall (Entity entity, Entity wall)
        {
            // do nothing
        }
    }
}
