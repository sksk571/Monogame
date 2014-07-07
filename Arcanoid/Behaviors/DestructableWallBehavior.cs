using Engine;

namespace Arcanoid.Behaviors
{
    public class DestructableWallBehavior// : ICollisionBehavior
    {
        private int _hp;

        public DestructableWallBehavior(int hp)
        {
            _hp = hp;
        }

        public void HandleCollision(Entity entity, Entity otherEntity)
        {
            _hp--;
            if (_hp == 0)
            {
                entity.Destroy();
            }
        }
    }
}
