using System.Collections.Generic;

namespace Engine
{
    internal interface IComponentStorage
    {
        void Remove(int entityIndex);
        void Clear();
    }

    internal class ComponentStorage<T> : IComponentStorage
            where T : struct
    {
        private readonly List<T> _components;

        public ComponentStorage()
        {
            _components = new List<T>();
        }

        public T Get(int entityIndex)
        {
            return _components[entityIndex];
        }

        public void Set(int entityIndex, T component)
        {
            while (_components.Count <= entityIndex)
            {
                _components.Add(default(T));
            }
            _components[entityIndex] = component;
        }

        public void Remove(int entityIndex)
        {
            if (_components.Count < entityIndex)
            {
                _components[entityIndex] = default(T);
            }
        }

        public void Clear()
        {
            _components.Clear();
        }
    }
}
