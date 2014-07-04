using System.Collections.Generic;
using Engine.Systems;
using Microsoft.Xna.Framework.Graphics;

namespace Engine
{
    public class GameSystems : CompositeSystem
    {
        public GameSystems(GraphicsDevice graphicsDevice)
            :base(new List<System>()
            {
                new MovementSystem(), 
                new DrawingSystem(graphicsDevice)
            })
        {}
    }
}
