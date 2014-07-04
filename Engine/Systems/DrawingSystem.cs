using System.Collections.Generic;
using Engine.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Systems
{
    public class DrawingSystem : System
    {
        private readonly GraphicsDevice _graphicsDevice;
        private readonly SpriteBatch _spriteBatch;

        public DrawingSystem(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
            _spriteBatch = new SpriteBatch(graphicsDevice);
        }

        public override void Draw(World world, GameTime gameTime)
        {
            _graphicsDevice.Clear(Color.White);

            _spriteBatch.Begin();
            IEnumerable<Entity> entities =
                world.Entities.WithComponent<PositionComponent>()
                .WithComponent<SpriteComponent>();
            DrawSprites(entities);
            _spriteBatch.End();
        }

        private void DrawSprites(IEnumerable<Entity> entities)
        {
            foreach (Entity entity in entities)
            {
                Texture2D sprite = entity.GetComponent<SpriteComponent>().Sprite;
                Vector2 position = entity.GetComponent<PositionComponent>().Position;
                _spriteBatch.Draw(sprite, position, Color.White);
            }
        }

        protected internal override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _spriteBatch.Dispose();
            }
        }
    }
}
