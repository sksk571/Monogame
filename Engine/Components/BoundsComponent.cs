using Engine.Util;

namespace Engine.Components
{
    public struct BoundsComponent
    {
        public readonly BoundingRect BoundingBox;

        public BoundsComponent(BoundingRect boundingBox)
        {
            BoundingBox = boundingBox;
        }
    }
}
