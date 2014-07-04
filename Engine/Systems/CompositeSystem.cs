using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Engine.Systems
{
    public class CompositeSystem : System
    {
        private readonly IEnumerable<System> _systems;

        public CompositeSystem(IEnumerable<System> systems)
        {
            _systems = systems;
        }

        public override void Update(World world, GameTime gameTime)
        {
            foreach (System system in _systems)
            {
                system.Update(world, gameTime);
            }
        }

        public override void Draw(World world, GameTime gameTime)
        {
            foreach (System system in _systems)
            {
                system.Draw(world, gameTime);
            }
        }

        protected internal override void Dispose(bool disposing)
        {
            foreach (System system in _systems)
            {
                system.Dispose(disposing);
            }
        }
    }
}
