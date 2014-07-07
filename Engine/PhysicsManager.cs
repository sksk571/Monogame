using System;
using Microsoft.Xna.Framework;
using Engine.Util;

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

		internal void SimulateStep(float dt)
		{
			_farseerWorld.Step (dt);
		}
	}
}

