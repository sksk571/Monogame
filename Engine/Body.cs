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

		public Body WithRestitution(float restitution)
		{
			_farseerBody.Restitution = restitution;
			return this;
		}

		public Body IgnoreGravity()
		{
			_farseerBody.IgnoreGravity = true;
			return this;
		}

		internal Vector2 Position
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

		internal float Rotation
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

		internal Vector2 LinearVelocity
		{
			get 
			{
				return FarseerPhysics.ConvertUnits.ToDisplayUnits(
					_farseerBody.LinearVelocity); 
			}
			set { _farseerBody.LinearVelocity = FarseerPhysics.ConvertUnits.ToSimUnits (value); }
		}

		internal BodyType BodyType
		{
			get { return (BodyType)_farseerBody.BodyType; }
			set { _farseerBody.BodyType = (FarseerPhysics.Dynamics.BodyType)value; }
		}

		public void Dispose()
		{
			_farseerBody.Dispose ();
		}
	}
}

