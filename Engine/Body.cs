using System;
using Microsoft.Xna.Framework;

namespace Engine
{
	public class Body : IDisposable
	{
		private readonly FarseerPhysics.Dynamics.Body _farseerBody;
        private Vector2 _scale;

		internal Body (FarseerPhysics.Dynamics.Body farseerBody)
		{
			_farseerBody = farseerBody;
            _scale = Vector2.One;
            SubscribeToCollisions ();
		}

		public Body WithRestitution(float restitution)
		{
			_farseerBody.Restitution = restitution;
			return this;
		}

        public Body WithMass(float mass)
        {
            _farseerBody.Mass = mass;
            return this;
        }

        public Body WithFriction(float friction)
        {
            _farseerBody.Friction = friction;
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

        internal Vector2 Scale 
        {
            get
            {
                return _scale;
            }
            set
            {
                if (_scale != value)
                {
                    foreach (var fixture in _farseerBody.FixtureList)
                    {
                        if (fixture.Shape is FarseerPhysics.Collision.Shapes.PolygonShape)
                            ((FarseerPhysics.Collision.Shapes.PolygonShape)fixture.Shape).Vertices.Scale (ref value);
                        else if (fixture.Shape is FarseerPhysics.Collision.Shapes.CircleShape)
                            ((FarseerPhysics.Collision.Shapes.CircleShape)fixture.Shape).Radius *= Math.Max(value.X, value.Y);
                    }
                }
                _scale = value;
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

