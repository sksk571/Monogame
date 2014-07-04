namespace Engine
{
    public class World
    {
        private readonly EntityManager _entities;

        public World()
        {
            _entities = new EntityManager();
        }

        public EntityManager Entities
        {
            get { return _entities; }
        }
    }
}
