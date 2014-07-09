using System;
using Microsoft.Xna.Framework;
using Engine.Util;
using System.Collections.Generic;
using System.Linq;

namespace Engine
{
	public class PhysicsManager
	{
		FarseerPhysics.Dynamics.World _farseerWorld;

		public PhysicsManager()
		{
			_farseerWorld = new FarseerPhysics.Dynamics.World (Vector2.Zero);
		}
			
		public Vector2 Gravity
		{
			get 
			{ 
				return FarseerPhysics.ConvertUnits.ToDisplayUnits(
				_farseerWorld.Gravity); 
			}
			set
			{
				_farseerWorld.Gravity = FarseerPhysics.ConvertUnits.ToSimUnits (value);
			}
		}

		public Body AddRectangle(RectangleF bounds)
		{
			var farseerBody = FarseerPhysics.Factories.BodyFactory.CreateRectangle (_farseerWorld, 
				FarseerPhysics.ConvertUnits.ToSimUnits (bounds.Width), 
				FarseerPhysics.ConvertUnits.ToSimUnits (bounds.Height), 1.0f, 
				Vector2.Zero, 0f);
			return new Body (farseerBody);
		}

        public Body AddRoundedRectangle(RectangleF bounds, float xRadius, float yRadius, int segments)
        {
            var farseerBody = FarseerPhysics.Factories.BodyFactory.CreateRoundedRectangle(_farseerWorld,
                FarseerPhysics.ConvertUnits.ToSimUnits(bounds.Width),
                FarseerPhysics.ConvertUnits.ToSimUnits(bounds.Height),
                FarseerPhysics.ConvertUnits.ToSimUnits(xRadius), 
                FarseerPhysics.ConvertUnits.ToSimUnits(yRadius), segments, 1.0f);
            return new Body (farseerBody);
        }

		public Body AddPolygon(IEnumerable<Vector2> vertices)
		{
			var farseerBody = FarseerPhysics.Factories.BodyFactory.CreatePolygon (_farseerWorld, 
				  new FarseerPhysics.Common.Vertices (
			          vertices.Select (v => FarseerPhysics.ConvertUnits.ToSimUnits (v))), 1.0f);
			return new Body (farseerBody);
		}

		public Body AddCircle(float radius)
		{
			var farseerBody = FarseerPhysics.Factories.BodyFactory.CreateCircle (_farseerWorld, 
				FarseerPhysics.ConvertUnits.ToSimUnits (radius), 
				1.0f);
			return new Body (farseerBody);
		}

		public Body AddLoopShape(IEnumerable<Vector2> vertices)
		{
			var farseerBody = FarseerPhysics.Factories.BodyFactory.CreateLoopShape (_farseerWorld, 
				new FarseerPhysics.Common.Vertices (
					vertices.Select(v => FarseerPhysics.ConvertUnits.ToSimUnits(v))));
			return new Body (farseerBody);
		}

		public Body AddChainShape(IEnumerable<Vector2> vertices)
		{
			var farseerBody = FarseerPhysics.Factories.BodyFactory.CreateChainShape (_farseerWorld, 
				new FarseerPhysics.Common.Vertices(
					vertices.Select (v => FarseerPhysics.ConvertUnits.ToSimUnits (v))));
			return new Body (farseerBody);
		}

		internal void SimulateStep(float dt)
		{
			_farseerWorld.Step (dt);
		}

        internal IList<Tuple<Entity, Entity>> Collisions()
        {
            IList<Tuple<Entity, Entity>> collisions = new List<Tuple<Entity, Entity>> ();
            foreach (var contact in _farseerWorld.ContactList) 
            {
                var entityA = (Entity)contact.FixtureA.Body.UserData;
                var entityB = (Entity)contact.FixtureA.Body.UserData;
                collisions.Add (Tuple.Create (entityA, entityB));
            }
            return collisions;
        }
	}
}

