using System;
using System.Collections;
using System.Collections.Generic;

namespace Engine
{
    public class EntityManager
    {
        private readonly List<Entity> _entities;
        private readonly Queue<Tuple<int, Entity>> _entityPool;
        private readonly List<IComponentStorage> _components;

        public EntityManager()
        {
            _entities = new List<Entity>();
            _entityPool = new Queue<Tuple<int, Entity>>();
            _components = new List<IComponentStorage>();
        }

        public Entity Add()
        {
            Entity entity = null;
            if (_entityPool.Count != 0)
            {
                Tuple<int, Entity> pooledEntity = _entityPool.Dequeue();
                _entities[pooledEntity.Item1] = pooledEntity.Item2;
                entity = pooledEntity.Item2;
            }
            else
            {
                int index = _entities.Count;
                entity = new Entity(index, this);
                _entities.Add(entity);
            }
            return entity;
        }

        public void Remove(Entity entity)
        {
            int index = entity.Index;
            _entities[index] = null;
            RemoveComponents(index);
            _entityPool.Enqueue(new Tuple<int, Entity>(index, entity));
        }

        public EntityQuery WithComponent<T>()
        {
            EntityQuery query = new EntityQuery(this);
            return query.WithComponent<T>();
        }

        internal void AddComponent<T>(int entityIndex, T component)
            where T : struct
        {
            int componentIndex = ComponentType<T>.Index;
            while (_components.Count <= componentIndex)
            {
                _components.Add(null);
            }
            if (_components[componentIndex] == null)
            {
                _components[componentIndex] = new ComponentStorage<T>();
            }
            ((ComponentStorage<T>)_components[componentIndex]).Set(entityIndex, component);
        }

        internal void RemoveComponents(int entityIndex)
        {
            foreach (IComponentStorage componentStorage in _components)
            {
                componentStorage.Remove(entityIndex);
            }
        }

        internal T GetComponent<T>(int entityIndex)
            where T : struct
        {
            return ((ComponentStorage<T>)_components[ComponentType<T>.Index]).Get(entityIndex);
        }

        internal void SetComponent<T>(int entityIndex, T component)
            where T : struct
        {
            ((ComponentStorage<T>)_components[ComponentType<T>.Index]).Set(entityIndex, component);
        }

        public class EntityQuery : IEnumerable<Entity>
        {
            private readonly EntityManager _manager;
            private long _mask;

            public EntityQuery(EntityManager manager)
            {
                _manager = manager;
                _mask = 0L;
            }

            public EntityQuery WithComponent<T>()
            {
                _mask |= ComponentType<T>.Mask;
                return this;
            }

            public IEnumerator<Entity> GetEnumerator()
            {
                foreach (Entity entity in _manager._entities)
                {
                    if (entity != null && (entity.Mask & _mask) == _mask)
                    {
                        yield return entity;
                    }
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}
