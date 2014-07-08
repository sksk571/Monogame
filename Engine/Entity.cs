using System;

namespace Engine
{
    public class Entity
    {
        private readonly int _index;
        private readonly EntityManager _entities;
        private string _name;
        private long _mask;

        internal Entity(int index, EntityManager entities)
        {
            _index = index;
            _entities = entities;
            _mask = 0L;
        }

        internal int Index
        {
            get { return _index; }
        }

        internal long Mask
        {
            get { return _mask; }
        }

        internal string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public Entity AddComponent<T>(T component)
            where T : struct
        {
            _entities.AddComponent(_index, component);
            _mask |= ComponentType<T>.Mask;
            return this;
        }

        public bool HasComponent<T>()
        {
            return (_mask & ComponentType<T>.Mask) == ComponentType<T>.Mask;
        }

        public T GetComponent<T>()
            where T : struct
        {
            return _entities.GetComponent<T>(_index);
        }

        public void SetComponent<T>(T component)
            where T : struct
        {
            _entities.SetComponent(_index, component);
        }

        public void RemoveComponent<T>()
        {
            _mask &= ~ComponentType<T>.Mask;
            _entities.RemoveComponent<T> (_index);
        }

        public void Destroy()
        {
            _entities.Remove(this);
        }
    }
}
