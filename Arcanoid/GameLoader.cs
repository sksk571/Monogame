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
            var random = new Random();
			//CreateBall(new Vector2(100, 100), new Vector2(-1000, 0));
            CreateBall(new Vector2(200, 100), new Vector2(1000, 0));
            CreateBall(new Vector2(100, 200), new Vector2(0, -1000));
            CreateBall(new Vector2(200, 200), new Vector2(0, 1000));
            //foreach (int i in Enumerable.Range(0, 10))
            //{
            //    CreateBall(new Vector2(random.Next(100, 500), random.Next(100, 500)), 
            //        new Vector2(random.Next(-100, 100), random.Next(-100, 100)));
            //}
            CreateCage();
            return _world;
        }

        private void CreateBall(Vector2 position, Vector2 moveDirection)
        {
            _world.Entities.Add().AddComponent(new SpriteComponent
                (_content.Load<Texture2D>("Sprites/wall.png")))
                .AddComponent(new PositionComponent
                    (position))
                .AddComponent(new MoveComponent(moveDirection))
                .AddComponent(new BoundsComponent(new BoundingRect(0, 0, 48, 48)))
                .AddComponent(new RigidBodyComponent());
        }

        private void CreateCage()
        {
            _world.Entities.Add()
                .AddComponent(new PositionComponent(new Vector2(0, 0)))
                .AddComponent(new BoundsComponent(new BoundingRect(0, 0, 1, _viewPort.Height)));
            _world.Entities.Add()
                .AddComponent(new PositionComponent(new Vector2(_viewPort.Width - 1, 0)))
                .AddComponent(new BoundsComponent(new BoundingRect(0, 0, 1, _viewPort.Height)));
            _world.Entities.Add()
                .AddComponent(new PositionComponent(new Vector2(1, 0)))
                .AddComponent(new BoundsComponent(new BoundingRect(0, 0, _viewPort.Width - 2, 1)));
            _world.Entities.Add()
                .AddComponent(new PositionComponent(new Vector2(1, _viewPort.Height - 1)))
                .AddComponent(new BoundsComponent(new BoundingRect(0, 0, _viewPort.Width - 2, 1)));
        }
    }
}
