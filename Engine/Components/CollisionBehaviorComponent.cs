namespace Engine.Components
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
