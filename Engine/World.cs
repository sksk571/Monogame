using Microsoft.Xna.Framework;

namespace Engine
{
    public class World
    {
        private readonly EntityManager _entities;
		private readonly PhysicsManager _bodies;

        public World ()
		{
			_entities = new EntityManager();
			_bodies = new PhysicsManager ();
		}

        public EntityManager Entities
        {
            get { return _entities; }
        }

		public PhysicsManager Physics
		{
			get { return _bodies; }
		}
    }
}
