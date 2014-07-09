using System;
using Engine;

namespace Arcanoid
{
    public interface ICollisionDispatch
    {
        void CollideBall(Entity entity, Entity ball);
        void CollideWall(Entity entity, Entity wall);
    }
}

