using System;
using System.Linq;
using Engine;
using Engine.Components;
using Engine.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Arcanoid.Behaviors;

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
            _world.Physics.Gravity = new Vector2 (0, 500);
            CreateCage ();
			CreateBall (new Vector2 (100, 100), new Vector2 (100, 500));
            CreateWall (new Vector2 (50, 50));
            CreateWall (new Vector2 (150, 50));
            CreateWall (new Vector2 (250, 50));
            CreateWall (new Vector2 (350, 50));
            CreateWall (new Vector2 (450, 50));
            CreateWall (new Vector2 (550, 50));
            CreateWall (new Vector2 (650, 50));
            CreateRacket (new Vector2 (400, 400));
            return _world;
        }

		private void CreateBall(Vector2 position, Vector2 movement)
		{
			_world.Entities.Add("ball").AddComponent(new SpriteComponent
                (_content.Load<Texture2D>("Sprites/combined2.png")))
                .AddComponent(new SpriteBoundsComponent(new Rectangle(0,0,22,22)))
				.AddComponent(new PositionComponent
                    (position))
				.AddComponent(new MoveComponent(movement))
                .AddComponent(new CollisionBehaviorComponent(new BallBehavior()))
				.AddComponent(new RigidBodyComponent(_world.Physics.AddCircle(11)
                    .WithRestitution(1)
                    .Bullet()
                ));
		}

        private void CreateWall(Vector2 position)
        {
            _world.Entities.Add().AddComponent(new SpriteComponent
                (_content.Load<Texture2D>("Sprites/combined2.png")))
                .AddComponent(new SpriteBoundsComponent(new Rectangle(240,552,64,32)))
                .AddComponent(new PositionComponent
                    (position))
                .AddComponent(new CollisionBehaviorComponent(new WallBehavior()))
				.AddComponent(new RigidBodyComponent(_world.Physics.AddRectangle(new RectangleF(0, 0, 64, 32))));
        }

        private void CreateCage ()
        {
            _world.Entities.Add()
                .AddComponent(new PositionComponent (new Vector2 (0, 0)))
                .AddComponent(new RigidBodyComponent(_world.Physics.AddLoopShape (new[] 
                    {
                        new Vector2 (0, 0),
                        new Vector2 (0, _viewPort.Height),
                        new Vector2 (_viewPort.Width, _viewPort.Height),
                        new Vector2 (_viewPort.Width, 0)
                    }).WithRestitution(1)));
        }

        private void CreateRacket(Vector2 position)
        {
            _world.Entities.Add("racket").AddComponent(new SpriteComponent
                (_content.Load<Texture2D>("Sprites/combined2.png")))
                .AddComponent(new SpriteBoundsComponent(new Rectangle(480, 1773, 104, 26)))
                .AddComponent(new PositionComponent(position))
                .AddComponent(new MoveComponent(Vector2.Zero))
                .AddComponent(new InputBehaviorComponent(new RacketBehavior()))
                .AddComponent(new RigidBodyComponent(_world.Physics.AddRoundedRectangle(new RectangleF(0, 0, 104, 26), 11, 11, 2)
                    .WithFixedRotation().IgnoreGravity()));
        }
    }
}
