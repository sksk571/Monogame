using System;
using Microsoft.Xna.Framework;

namespace Engine
{
    public class System : IDisposable
    {
        private bool _disposed;

        public System()
        {
            _disposed = false;
        }

        public virtual void Update(World world, GameTime gameTime)
        {}

        public virtual void Draw(World world, GameTime gameTime)
        {}

        public void Dispose()
        {
            if (!_disposed)
            {
                Dispose(true);
                _disposed = true;
            }
            GC.SuppressFinalize(this);
        }

        protected internal virtual void Dispose(bool disposing)
        {
        }

        ~System()
        {
            Dispose(false);
        }
    }
}
