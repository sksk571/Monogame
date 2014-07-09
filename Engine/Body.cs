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
            SubscribeToCollisions ();
		}

		public Body WithRestitution(float restitution)
		{
			_farseerBody.Restitution = restitution;
			return this;
		}

        public Body WithFixedRotation(bool fixedRotation = true)
        {
            _farseerBody.FixedRotation = fixedRotation;
            return this;
        }

        public Body IgnoreGravity(bool ignoreGravity = true)
		{
            _farseerBody.IgnoreGravity = ignoreGravity;
			return this;
		}

        public Body Bullet(bool isBullet = true)
        {
            _farseerBody.IsBullet = isBullet;
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

        internal Entity Entity
        {
            get { return (Entity)_farseerBody.UserData; }
            set { _farseerBody.UserData = value; }
        }

		public void Dispose()
		{
			_farseerBody.Dispose ();
		}

        void SubscribeToCollisions ()
        {
            _farseerBody.OnCollision += (
                FarseerPhysics.Dynamics.Fixture fixtureA, 
                FarseerPhysics.Dynamics.Fixture fixtureB, 
                FarseerPhysics.Dynamics.Contacts.Contact contact) => 
            {
                if (Entity != null && Entity.HasComponent<CollisionBehaviorComponent> ())
                {
                    Entity entityA = (Entity)fixtureA.Body.UserData;
                    Entity entityB = (Entity)fixtureB.Body.UserData;
                    Entity other = null;
                    if (!ReferenceEquals (entityA, Entity))
                        other = entityA;
                    if (!ReferenceEquals (entityB, Entity))
                        other = entityB;
                    if (other != null)
                    {
                        ICollisionBehavior collisionBehavior = Entity.GetComponent<CollisionBehaviorComponent> ().Behavior;
                        collisionBehavior.HandleCollision (Entity, other);
                    }
                }
                return true;
            };
        }
	}
}

