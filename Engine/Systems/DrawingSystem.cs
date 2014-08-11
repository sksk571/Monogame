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
            entities =
                world.Entities.WithComponent<PositionComponent>()
                    .WithComponent<TextComponent>();
            DrawText (entities);
            _spriteBatch.End();
        }

        private void DrawSprites(IEnumerable<Entity> entities)
        {
            foreach (Entity entity in entities)
            {
                Texture2D sprite = entity.GetComponent<SpriteComponent>().Sprite;
				PositionComponent positionComponent = entity.GetComponent<PositionComponent> ();
                Rectangle bounds = sprite.Bounds;
                if (entity.HasComponent<SpriteBoundsComponent> ())
                {
                    bounds = entity.GetComponent<SpriteBoundsComponent> ().Bounds;
                }
                Vector2 origin = new Vector2 (bounds.Width / 2f, bounds.Height / 2f);
                _spriteBatch.Draw(sprite, positionComponent.Position, null, bounds, origin, positionComponent.Rotation);
            }
        }

        private void DrawText(IEnumerable<Entity> entities)
        {
            foreach (Entity entity in entities)
            {
                var textComponent = entity.GetComponent<TextComponent> ();
                PositionComponent positionComponent = entity.GetComponent<PositionComponent> ();
                _spriteBatch.DrawString (textComponent.Font, textComponent.Text, positionComponent.Position, Color.White);
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
