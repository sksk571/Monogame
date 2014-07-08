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
			EntityManager.EntityQuery movableEntities = world.Entities.WithComponent<PositionComponent> ()
				.WithComponent<MoveComponent> ();
			EntityManager.EntityQuery rigidEntities = world.Entities.WithComponent<PositionComponent> ()
				.WithComponent<RigidBodyComponent> ();
			foreach (Entity entity in rigidEntities) 
			{
				RigidBodyComponent rigidBody = entity.GetComponent<RigidBodyComponent> ();
				PositionComponent positionComponent = entity.GetComponent<PositionComponent> ();
				rigidBody.Body.Position = positionComponent.Position;
				rigidBody.Body.Rotation = positionComponent.Rotation;
			}
			EntityManager.EntityQuery rigidMovableEntities = rigidEntities.WithComponent<MoveComponent> ();
			foreach (Entity entity in rigidMovableEntities) 
			{
				RigidBodyComponent rigidBody = entity.GetComponent<RigidBodyComponent> ();
				rigidBody.Body.BodyType = BodyType.Dynamic;
				rigidBody.Body.LinearVelocity = entity.GetComponent<MoveComponent> ().MoveVector;
			}

			var scaleFactor = (float)gameTime.ElapsedGameTime.TotalSeconds;
			world.Physics.SimulateStep (scaleFactor);

			foreach (Entity entity in rigidMovableEntities) 
			{
				RigidBodyComponent rigidBody = entity.GetComponent<RigidBodyComponent> ();
				entity.SetComponent<PositionComponent> (new PositionComponent (rigidBody.Body.Position, rigidBody.Body.Rotation));
				entity.SetComponent<MoveComponent> (new MoveComponent (rigidBody.Body.LinearVelocity));
			}

			IEnumerable<Entity> otherMovableEntities = movableEntities.Except (rigidMovableEntities);
			foreach (Entity entity in otherMovableEntities) 
			{
				PositionComponent positionComponent = entity.GetComponent<PositionComponent> ();
				Vector2 moveVector = entity.GetComponent<MoveComponent>().MoveVector;
				entity.SetComponent(new PositionComponent(positionComponent.Position + moveVector * scaleFactor, positionComponent.Rotation));
			}
        }
    }
}
