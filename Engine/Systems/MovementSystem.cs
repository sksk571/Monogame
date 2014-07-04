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
            float scaleFactor = (float)gameTime.ElapsedGameTime.TotalSeconds;
            IEnumerable<Entity> entities =
                world.Entities.WithComponent<PositionComponent>();
            var collidables = ProcessNonCollidableEntities(entities, scaleFactor);
            ProcessCollidableEntities(collidables, scaleFactor);
        }

        private static List<Collidable> ProcessNonCollidableEntities(IEnumerable<Entity> entities, float scaleFactor)
        {
            List<Collidable> collidables = new List<Collidable>();
            foreach (Entity entity in entities)
            {
                Vector2 moveVector = Vector2.Zero;
                if (entity.HasComponent<MoveComponent>())
                {
                    moveVector = entity.GetComponent<MoveComponent>().MoveVector;
                }
                Vector2 position = entity.GetComponent<PositionComponent>().Position;
                if (entity.HasComponent<BoundsComponent>())
                {
                    BoundingRect boundingBox = entity.GetComponent<BoundsComponent>().BoundingBox;
                    collidables.Add(new Collidable
                    {
                        Entity = entity,
                        BoundingBox = boundingBox,
                        MoveVector = moveVector,
                        Position = position,
                    });
                }
                else if (entity.HasComponent<MoveComponent>())
                {
                    entity.SetComponent(new PositionComponent(position + moveVector * scaleFactor));
                }
            }
            return collidables;
        }

        private static BoundingRect CalculateAxisAlignedBoundingBox(BoundingRect boundingBox, Vector2 position, 
            Vector2 moveFactor)
        {
            boundingBox = new BoundingRect(boundingBox.Min + position, boundingBox.Max + position);
            BoundingRect movedBoundingBox = new BoundingRect(boundingBox.Min + moveFactor, 
                boundingBox.Max + moveFactor);
            BoundingRect aabb = new BoundingRect(Vector2.Min(boundingBox.Min, movedBoundingBox.Min), 
                Vector2.Max(boundingBox.Max, movedBoundingBox.Max));
            return aabb;
        }

        private static void ProcessCollidableEntities(List<Collidable> collidables, float scaleFactor)
        {
            var aabbs = new BoundingRect[collidables.Count];
            for (int i = 0; i < collidables.Count; ++i)
            {
                aabbs[i] = CalculateAxisAlignedBoundingBox(collidables[i].BoundingBox, collidables[i].Position,
                    collidables[i].MoveVector * scaleFactor);
            }
            var hasIntersection = new bool[collidables.Count];
            for (int i = 0; i < collidables.Count; ++i)
            {
                for (int j = i + 1; j < collidables.Count; ++j)
                {
                    if (aabbs[i].Intersects(aabbs[j]))
                    {
                        hasIntersection[i] = hasIntersection[j] = true;
                    }
                }
            }
            var indexes = new List<int>();
            for (int i = 0; i < collidables.Count; ++i)
            {
                if (hasIntersection[i])
                {
                    indexes.Add(i);
                }
                else
                {
                    var collidable = collidables[i];
                    collidable.Position = collidable.Position + collidable.MoveVector * scaleFactor;
                    collidables[i] = collidable;
                }
            }
            IEnumerable<Tuple<int, int>> collisions = ProcessAabbIntersection(collidables, scaleFactor, indexes);
            for (int i = 0; i < collidables.Count; ++i)
            {
                collidables[i].Entity.SetComponent(new PositionComponent(collidables[i].Position));
                collidables[i].Entity.SetComponent(new MoveComponent(collidables[i].MoveVector));
            }
            foreach (Tuple<int, int> collision in collisions)
            {
                TriggerCollision(collidables, collision.Item1, collision.Item2);
                TriggerCollision(collidables, collision.Item2, collision.Item1);
            }
        }

        private static void TriggerCollision(List<Collidable> collidables, int i, int j)
        {
            if (collidables[i].Entity.HasComponent<CollisionBehaviorComponent>())
            {
                collidables[i].Entity.GetComponent<CollisionBehaviorComponent>()
                    .Behavior.HandleCollision(
                        collidables[i].Entity, collidables[j].Entity);
            }
        }

        private static IEnumerable<Tuple<int, int>> ProcessAabbIntersection(List<Collidable> collidables, float scaleFactor, List<int> indexes)
        {
            var collisions = new List<Tuple<int, int>>();
            if (indexes.Count == 0)
            {
                return collisions;
            }
            Collidable iCollidable = collidables[indexes[0]], jCollidable = collidables[indexes[1]];
            int numSteps = (int)Math.Ceiling(((iCollidable.MoveVector - jCollidable.MoveVector) * scaleFactor).Length()) * 2;
            float stepScaleFactor = (float)1 / numSteps * scaleFactor;
            Vector2 iStepMove = iCollidable.MoveVector * stepScaleFactor;
            Vector2 jStepMove = jCollidable.MoveVector * stepScaleFactor;
            const float moveEpsilon = 0.5f; // half of pixel
            int step = 1;
            for (; step <= numSteps; ++step)
            {
                iCollidable.Position += iStepMove;
                jCollidable.Position += jStepMove;
                BoundingRect iStepBoundingBox = iCollidable.BoundingBox + iCollidable.Position + iStepMove;
                BoundingRect jStepBoundingBox = jCollidable.BoundingBox + jCollidable.Position + jStepMove;
                if (iStepBoundingBox.Intersects(jStepBoundingBox))
                {
                    // collision
                    if (Math.Abs(iStepBoundingBox.Min.X - jStepBoundingBox.Max.X) <= moveEpsilon ||
                        Math.Abs(iStepBoundingBox.Max.X - jStepBoundingBox.Min.X) <= moveEpsilon)
                    {
                        ProcessXCollision(ref iCollidable, ref jCollidable);
                    }
                    if (Math.Abs(iStepBoundingBox.Min.Y - jStepBoundingBox.Max.Y) <= moveEpsilon ||
                        Math.Abs(iStepBoundingBox.Max.Y - jStepBoundingBox.Min.Y) <= moveEpsilon)
                    {
                        ProcessYCollision(ref iCollidable, ref jCollidable);
                    }
                    iStepMove = iCollidable.MoveVector * stepScaleFactor;
                    jStepMove = jCollidable.MoveVector * stepScaleFactor;
                    collisions.Add(Tuple.Create(indexes[0], indexes[1]));
                }
            }
            collidables[indexes[0]] = iCollidable;
            collidables[indexes[1]] = jCollidable;
            return collisions;
        }

        private static void ProcessXCollision(ref Collidable iCollidable, ref Collidable jCollidable)
        {
            if (iCollidable.Entity.HasComponent<RigidBodyComponent>())
            {
                InvertXMovement(ref iCollidable);
            }
            if (jCollidable.Entity.HasComponent<RigidBodyComponent>())
            {
                InvertXMovement(ref jCollidable);
            }
        }

        private static void ProcessYCollision(ref Collidable iCollidable, ref Collidable jCollidable)
        {
            if (iCollidable.Entity.HasComponent<RigidBodyComponent>())
            {
                InvertYMovement(ref iCollidable);
            }
            if (jCollidable.Entity.HasComponent<RigidBodyComponent>())
            {
                InvertYMovement(ref jCollidable);
            }
        }

        private static void InvertXMovement(ref Collidable collidable)
        {
            collidable.MoveVector = new Vector2(-collidable.MoveVector.X, collidable.MoveVector.Y);
        }

        private static void BorrowXMovement(ref Collidable collidable, ref Collidable otherCollidable)
        {
            collidable.MoveVector = new Vector2(otherCollidable.MoveVector.X, collidable.MoveVector.Y);
        }

        private static void InvertYMovement(ref Collidable collidable)
        {
            collidable.MoveVector = new Vector2(collidable.MoveVector.X, -collidable.MoveVector.Y);
        }

        private static void BorrowYMovement(ref Collidable collidable, ref Collidable otherCollidable)
        {
            collidable.MoveVector = new Vector2(collidable.MoveVector.X, otherCollidable.MoveVector.Y);
        }

        private struct Collidable
        {
            public Entity Entity;
            public BoundingRect BoundingBox;
            public Vector2 Position;
            public Vector2 MoveVector;
        }
    }

    //public class MovementSystem : System
    //{
    //    public override void Update(World world, GameTime gameTime)
    //    {
    //        float scaleFactor = (float)gameTime.ElapsedGameTime.TotalSeconds;
    //        var allBounds = world.Components.GetComponents<BoundsComponent>().ToArray();
    //        foreach (MoveComponent moveComponent in world.Components.GetComponents<MoveComponent>())
    //        {
    //            Vector2 direction = moveComponent.MoveVector;
    //            Vector2 moveFactor = Vector2.Multiply(direction, scaleFactor);
    //            Entity entity = moveComponent.Entity;
    //            var positionComponent = entity.GetComponent<PositionComponent>();
    //            if (ProcessCollision(entity, moveFactor, allBounds, positionComponent))
    //            {
    //                return;
    //            }
    //            positionComponent.Position = positionComponent.Position + moveFactor;
    //        }
    //    }

    //    private bool ProcessCollision(Entity entity, Vector2 moveFactor, BoundsComponent[] allBounds,
    //        PositionComponent positionComponent)
    //    {
    //        var entityBounds = entity.GetComponents<BoundsComponent>();
    //        foreach (BoundsComponent bounds in entityBounds)
    //        {
    //            foreach (BoundsComponent otherEntityBounds in allBounds)
    //            {
    //                PositionComponent otherPositionComponent = otherEntityBounds.Entity.GetComponent<PositionComponent>();
    //                if (bounds.Entity == otherEntityBounds.Entity)
    //                {
    //                    continue;
    //                }
    //                BoundingRect boundingRect = bounds.BoundingRect + positionComponent.Position;
    //                BoundingRect movedBoundingRect = boundingRect + moveFactor;
    //                BoundingRect otherBoundingRect = otherEntityBounds.BoundingRect + otherPositionComponent.Position;
    //                if (!boundingRect.Intersects(otherBoundingRect) &&
    //                    movedBoundingRect.Intersects(otherBoundingRect))
    //                {
    //                    Vector2 collisionVector = CalculateCollision(boundingRect, movedBoundingRect, otherBoundingRect);
    //                    var collisionBehaviors = entity.GetComponents<CollisionBehaviorComponent>();
    //                    foreach (CollisionBehaviorComponent behavior in collisionBehaviors)
    //                    {
    //                        behavior.Behavior.HandleCollision(entity, otherEntityBounds.Entity);
    //                    }
    //                    if (bounds.Solid)
    //                    {
    //                        entity.GetComponent<MoveComponent>().MoveVector *= collisionVector;
    //                    }
    //                    return true;
    //                }
    //            }
    //        }
    //        return false;
    //    }

    //    private Vector2 CalculateCollision(BoundingRect boundingRect, BoundingRect movedBoundingRect, BoundingRect otherBoundingRect)
    //    {
    //        Vector2 v = Vector2.One;
    //        if (boundingRect.Max.X < otherBoundingRect.Min.X &&
    //            movedBoundingRect.Max.X > otherBoundingRect.Min.X)
    //        {
    //            v *= new Vector2(-1, 1);
    //        }
    //        if (boundingRect.Min.X < otherBoundingRect.Max.X &&
    //            movedBoundingRect.Min.X > otherBoundingRect.Max.X)
    //        {
    //            v *= new Vector2(-1, 1);
    //        }
    //        if (boundingRect.Min.X > otherBoundingRect.Max.X &&
    //            movedBoundingRect.Min.X < otherBoundingRect.Max.X)
    //        {
    //            v *= new Vector2(-1, 1);
    //        }
    //        if (boundingRect.Max.Y < otherBoundingRect.Min.Y &&
    //            movedBoundingRect.Max.Y > otherBoundingRect.Min.Y)
    //        {
    //            v *= new Vector2(1, -1);
    //        }
    //        if (boundingRect.Min.Y < otherBoundingRect.Max.Y &&
    //            movedBoundingRect.Min.Y > otherBoundingRect.Max.Y)
    //        {
    //            v *= new Vector2(1, -1);
    //        }
    //        if (boundingRect.Min.Y > otherBoundingRect.Max.Y &&
    //            movedBoundingRect.Min.Y < otherBoundingRect.Max.Y)
    //        {
    //            v *= new Vector2(1, -1);
    //        }
    //        return v;
    //    }
    //}
}
