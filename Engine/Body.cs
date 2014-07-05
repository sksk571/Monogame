using System;
using Microsoft.Xna.Framework;

namespace Engine
{
	public class Body : IDisposable
	{
		private readonly FarseerPhysics.Dynamics.Body _farseerBody;

		internal Body (FarseerPhysics.Dynamics.Body farseerBody)
		{
			_farseerBody = farseerBody;
		}

		public Vector2 Position
		{
			get
			{
				return FarseerPhysics.ConvertUnits.ToDisplayUnits(
					_farseerBody.Position);
			}
			set
			{
				_farseerBody.Position = FarseerPhysics.ConvertUnits.ToSimUnits (value);
			}
		}

		public float Rotation
		{
			get
			{
				return _farseerBody.Rotation;
			}
			set
			{
				_farseerBody.Rotation = value;
			}
		}

		public Vector2 LocalCenter
		{
			get
			{
				return FarseerPhysics.ConvertUnits.ToDisplayUnits(
					_farseerBody.LocalCenter);
			}
		}

		public Vector2 LinearVelocity
		{
			get 
			{
				return FarseerPhysics.ConvertUnits.ToDisplayUnits(
					_farseerBody.LinearVelocity); 
			}
			set { _farseerBody.LinearVelocity = FarseerPhysics.ConvertUnits.ToSimUnits (value); }
		}

		public Body SetPosition(Vector2 position)
		{
			Position = position;
			return this;
		}

		public Body SetRotation(float rotation)
		{
			Rotation = rotation;
			return this;
		}

		public Body SetLinearVelocity(Vector2 velocity)
		{
			LinearVelocity = velocity;
			return this;
		}

		public void Dispose()
		{
			_farseerBody.Dispose ();
		}
	}
}

