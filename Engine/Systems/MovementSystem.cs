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
			EntityManager.EntityQuery movableEntities = world.Entities.WithComponent<TransformComponent> ()
				.WithComponent<MoveComponent> ();
			EntityManager.EntityQuery rigidEntities = world.Entities.WithComponent<TransformComponent> ()
				.WithComponent<RigidBodyComponent> ();
			foreach (Entity entity in rigidEntities) 
			{
				RigidBodyComponent rigidBody = entity.GetComponent<RigidBodyComponent> ();
                TransformComponent transform = entity.GetComponent<TransformComponent> ();
                rigidBody.Body.Position = transform.Position;
                rigidBody.Body.Rotation = transform.Rotation;
                rigidBody.Body.Scale = transform.Scale;
                rigidBody.Body.Entity = entity;
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
				entity.SetComponent<TransformComponent> (new TransformComponent (rigidBody.Body.Position, rigidBody.Body.Rotation, rigidBody.Body.Scale));
				entity.SetComponent<MoveComponent> (new MoveComponent (rigidBody.Body.LinearVelocity));
			}

			IEnumerable<Entity> otherMovableEntities = movableEntities.Except (rigidMovableEntities);
			foreach (Entity entity in otherMovableEntities) 
			{
                TransformComponent transform = entity.GetComponent<TransformComponent> ();
				Vector2 moveVector = entity.GetComponent<MoveComponent>().MoveVector;
                entity.SetComponent(new TransformComponent(transform.Position + moveVector * scaleFactor, transform.Rotation));
			}
        }
    }
}
