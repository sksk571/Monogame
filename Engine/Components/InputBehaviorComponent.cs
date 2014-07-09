using System;

namespace Engine
{
    public struct InputBehaviorComponent
    {
        public readonly IInputBehavior Behavior;
        public InputBehaviorComponent(IInputBehavior behavior)
        {
            Behavior = behavior;
        }
    }
}

