using System;

namespace Engine.Components
{
    public struct RigidBodyComponent : IDisposable
    {
		public readonly Body Body;

		public RigidBodyComponent(Body body)
		{
			Body = body;
		}

		public void Dispose()
		{
			if (Body != null)
				Body.Dispose ();
		}
    }
}
