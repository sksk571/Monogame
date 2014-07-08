#region Using Statements

using Arcanoid.Behaviors;
using Engine;
using Engine.Components;
using Engine.Systems;
using Engine.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System = Engine.System;

#endregion

namespace Arcanoid
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class ArcanoidGame : Game
    {
        private GraphicsDeviceManager _graphicsDeviceManager;
        private World _world;
        private GameSystems _systems;

        public ArcanoidGame()
            : base()
        {
            _graphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            _systems = new GameSystems(GraphicsDevice);
            var gameLoader = new GameLoader(Content, GraphicsDevice.Viewport);
            _world = gameLoader.LoadLevel();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            _systems.Dispose();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            int x = Mouse.GetState ().Position.X;
            Entity racket = _world.Entities.Get ("racket");
            racket.SetComponent (new PositionComponent (new Vector2(x, racket.GetComponent<PositionComponent> ().Position.Y)));

            _systems.Update(_world, gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            _systems.Draw(_world, gameTime);

            base.Draw(gameTime);
        }
    }
}
