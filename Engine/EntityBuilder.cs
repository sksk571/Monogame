using System;
using Microsoft.Xna.Framework;

namespace Engine
{
    public struct EntityBuilder
    {
        private readonly Entity _entity;
        private readonly PhysicsManager _physics;

        internal EntityBuilder(Entity entity, PhysicsManager physics)
        {
            _entity = entity;
            _physics = physics;
        }

        public Entity Entity 
        {
            get { return _entity; }
        }

        public PhysicsManager Physics
        {
            get { return _physics; }
        }
    }
}

