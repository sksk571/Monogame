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
                world.Entities.WithComponent<TransformComponent>()
                .WithComponent<SpriteComponent>();
            DrawSprites(entities);
            entities =
                world.Entities.WithComponent<TransformComponent>()
                    .WithComponent<TextComponent>();
            DrawText (entities);
            _spriteBatch.End();
        }

        private void DrawSprites(IEnumerable<Entity> entities)
        {
            foreach (Entity entity in entities)
            {
                Texture2D sprite = entity.GetComponent<SpriteComponent>().Sprite;
				TransformComponent transform = entity.GetComponent<TransformComponent> ();
                Rectangle bounds = sprite.Bounds;
                if (entity.HasComponent<SpriteBoundsComponent> ())
                {
                    bounds = entity.GetComponent<SpriteBoundsComponent> ().Bounds;
                }
                Vector2 origin = new Vector2 (bounds.Width / 2f, bounds.Height / 2f);
                _spriteBatch.Draw(sprite, transform.Position, bounds, Color.White, transform.Rotation, origin, transform.Scale, SpriteEffects.None, 0f);
            }
        }

        private void DrawText(IEnumerable<Entity> entities)
        {
            foreach (Entity entity in entities)
            {
                var textComponent = entity.GetComponent<TextComponent> ();
                TransformComponent transform = entity.GetComponent<TransformComponent> ();
                _spriteBatch.DrawString (textComponent.Font, textComponent.Text, transform.Position, Color.White, transform.Rotation, Vector2.Zero, transform.Scale, SpriteEffects.None, 0f);
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
