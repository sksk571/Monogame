using Microsoft.Xna.Framework;

namespace Engine
{
    public class World
    {
        private readonly EntityManager _entities;
		private FarseerPhysics.Dynamics.World _farseerWorld;

        public World ()
			: this (Vector2.Zero)
		{
		}

		public World(Vector2 gravity)
		{
			Vector2 simGravity = FarseerPhysics.ConvertUnits.ToSimUnits (gravity);
			_farseerWorld = new FarseerPhysics.Dynamics.World (simGravity);
			_entities = new EntityManager();
		}

        public EntityManager Entities
        {
            get { return _entities; }
        }

		internal FarseerPhysics.Dynamics.World FarseerWorld
		{
			get { return _farseerWorld; }
		}
    }
}
