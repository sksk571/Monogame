using System;
using System.Linq;
using Engine;
using Engine.Components;
using Engine.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Arcanoid
{
    public class GameLoader
    {
        private World _world;
        private readonly ContentManager _content;
        private readonly Viewport _viewPort;

        public GameLoader(ContentManager content, Viewport viewPort)
        {
            _content = content;
            _viewPort = viewPort;
        }

        public World LoadLevel()
        {
			_world = new World();
			_world.Physics.Gravity = new Vector2 (0, 100);
			for (int i = 0; i < 10; ++i) {
				CreateWall (new Vector2 (24 + 48 * i, 30));
			}
			for (int i = 0; i < 10; ++i) {
				CreateWall (new Vector2 (24 + 48 * i, 400));
			}
			CreateBall (new Vector2 (300, 300), new Vector2 (0, -150));
            return _world;
        }

		private void CreateBall(Vector2 position, Vector2 movement)
		{
			_world.Entities.Add().AddComponent(new SpriteComponent
				(_content.Load<Texture2D>("Sprites/ball.png")))
				.AddComponent(new PositionComponent
					(position))
				.AddComponent(new MoveComponent(movement))
				.AddComponent(new RigidBodyComponent(_world.Physics.AddRectangle(new RectangleF(0, 0, 48, 48))));
		}

        private void CreateWall(Vector2 position)
        {
            _world.Entities.Add().AddComponent(new SpriteComponent
                (_content.Load<Texture2D>("Sprites/wall.png")))
                .AddComponent(new PositionComponent
                    (position))
				.AddComponent(new RigidBodyComponent(_world.Physics.AddRectangle(new RectangleF(0, 0, 48, 48))));
        }
    }
}
