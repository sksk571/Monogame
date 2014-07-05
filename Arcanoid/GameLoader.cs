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
			_world = new World(new Vector2(0, 20));
			for (int i = 0; i < 10; ++i) {
				CreateWall (new Vector2 (48 * i, 300));
			}
			CreateBall (new Vector2 (470, 100), new Vector2 (0, 100));
            //CreateCage();
            return _world;
        }

		private void CreateBall(Vector2 position, Vector2 velocity)
		{
			_world.Entities.Add().AddComponent(new SpriteComponent
				(_content.Load<Texture2D>("Sprites/wall.png")))
				.AddComponent(new PositionComponent
					(position))
				.AddComponent(new MoveComponent(velocity))
				.AddComponent(new BoundsComponent(new BoundingRect(0, 0, 48, 48)))
				.AddComponent(new RigidBodyComponent());
		}

        private void CreateWall(Vector2 position)
        {
            _world.Entities.Add().AddComponent(new SpriteComponent
                (_content.Load<Texture2D>("Sprites/wall.png")))
                .AddComponent(new PositionComponent
                    (position))
                .AddComponent(new BoundsComponent(new BoundingRect(0, 0, 48, 48)))
                .AddComponent(new RigidBodyComponent());
        }
    }
}
