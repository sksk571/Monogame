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
        private readonly Dictionary<string, int> _nameToIndex;
        private readonly Dictionary<int, string> _indexToName;

        public EntityManager()
        {
            _entities = new List<Entity>();
            _entityPool = new Queue<Tuple<int, Entity>>();
            _nameToIndex = new Dictionary<string, int> ();
            _indexToName = new Dictionary<int, string> ();
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

        public Entity Add(string name)
        {
            Entity entity = Add ();
            _nameToIndex.Add (name, entity.Index);
            _indexToName.Add (entity.Index, name);
            return entity;
        }

        public Entity Get(string name)
        {
            return _entities [_nameToIndex [name]];
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

        internal void RemoveComponent<T>(int entityIndex)
        {
            int componentIndex = ComponentType<T>.Index;
            _components [componentIndex].Remove (entityIndex);
        }

        internal void RemoveComponents(int entityIndex, IList<int> componentIndexes)
        {
            foreach (int index in componentIndexes) 
            {
                _components[index].Remove (entityIndex);
            }
        }

        internal void Remove(Entity entity)
        {
            int index = entity.Index;
            _entities[index] = null;
            if (_indexToName.ContainsKey(index)) 
            {
                _nameToIndex.Remove (_indexToName [index]);
                _indexToName.Remove (index);
            }
            _entityPool.Enqueue(new Tuple<int, Entity>(index, entity));
            RemoveComponents (index, 
                ComponentType.GetIndexes(entity.Mask));
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
