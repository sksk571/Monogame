using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Components;
using Engine.Util;
using Microsoft.Xna.Framework;

namespace Engine.Systems
{
    public class MovementSystem : System
    {
        public override void Update(World world, GameTime gameTime)
        {
            IEnumerable<Entity> entities = world.Entities.WithComponent<PositionComponent>();
			var scaleFactor = (float)gameTime.ElapsedGameTime.TotalSeconds;
			world.Physics.SimulateStep (scaleFactor);
			foreach (Entity entity in entities)
			{
				PositionComponent positionComponent = entity.GetComponent<PositionComponent> ();
				if (entity.HasComponent<RigidBodyComponent> ()) 
				{
					RigidBodyComponent rigidBody = entity.GetComponent<RigidBodyComponent> ();
					entity.SetComponent (new PositionComponent (rigidBody.Body.Position, rigidBody.Body.Rotation));
					if (entity.HasComponent<MoveComponent> ()) {
						entity.SetComponent (new MoveComponent (rigidBody.Body.LinearVelocity));
					}
				}
				else if (entity.HasComponent<MoveComponent>())
				{
					Vector2 moveVector = entity.GetComponent<MoveComponent>().MoveVector;
					entity.SetComponent(new PositionComponent(positionComponent.Position + moveVector * scaleFactor, positionComponent.Rotation));
				}
			}
        }
    }
}
