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
            CreateBall (new Vector2 (_viewPort.Width / 2, 430), new Vector2(300, -500));
            // w = 64, h = 32
            for (int i = 0; i < 12; ++i)
                for (int j = 0; j < 10; ++j)
                    CreateWall (new Vector2 (32 + 64 * i, 16 + 32 * j));
            CreateRacket (new Vector2 (_viewPort.Width / 2, 450));
            return _world;
        }

        void CreateBall (Vector2 position, Vector2 velocity)
        {
            _world.Entities.Add ("ball")
                .AddComponent(new PositionComponent(position))
                .AddComponent(new MoveComponent(velocity))
                .AddComponent (new SpriteComponent (_content.Load<Texture2D> ("Sprites/combined2.png")))
                .AddComponent (new SpriteBoundsComponent (new Rectangle (0, 0, 22, 22)))
                .AddComponent (new CollisionBehaviorComponent (new BallBehavior ()))
                .AddComponent (new RigidBodyComponent (_world.Physics.AddCircle (11)
                    .WithRestitution (1)
                    .WithFriction(0)
                    .WithFixedRotation()
                    .Bullet ()));
        }

        void CreateWall (Vector2 position)
        {
            _world.Entities.Add()
                .AddComponent(new PositionComponent(position))
                .AddComponent (new SpriteComponent (_content.Load<Texture2D> ("Sprites/combined2.png")))
                .AddComponent (new SpriteBoundsComponent (new Rectangle (240, 552, 64, 32)))
                .AddComponent (new CollisionBehaviorComponent (new WallBehavior ()))
                .AddComponent (new RigidBodyComponent (_world.Physics.AddRectangle (new RectangleF (0, 0, 64, 32))));
        }

        void CreateRacket (Vector2 position)
        {
            _world.Entities.Add()
                .AddComponent(new PositionComponent(position))
                .AddComponent (new SpriteComponent (_content.Load<Texture2D> ("Sprites/combined2.png")))
                .AddComponent (new SpriteBoundsComponent (new Rectangle (480, 1773, 104, 26)))
                .AddComponent (new MoveComponent (Vector2.Zero))
                .AddComponent (new InputBehaviorComponent (new RacketBehavior (_viewPort.Bounds)))
                .AddComponent (new RigidBodyComponent (_world.Physics.AddRoundedRectangle (new RectangleF (0, 0, 104, 26), 11, 11, 2)
                    .WithFixedRotation ()
                    .IgnoreGravity ()));
        }

        void CreateCage ()
        {
            _world.Entities.Add ().AddComponent (new PositionComponent (new Vector2 (0, 0))).AddComponent (new RigidBodyComponent (_world.Physics.AddLoopShape (new[] {
                new Vector2 (0, 0),
                new Vector2 (0, _viewPort.Height),
                new Vector2 (_viewPort.Width, _viewPort.Height),
                new Vector2 (_viewPort.Width, 0)
            })));
        }
    }
}
