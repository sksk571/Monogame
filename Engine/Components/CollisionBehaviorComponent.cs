using System;

namespace Engine
{
    public struct CollisionBehaviorComponent
    {
        public readonly ICollisionBehavior Behavior;
        public CollisionBehaviorComponent(ICollisionBehavior behavior)
        {
            Behavior = behavior;
        }
    }
}

