using System.Collections.Generic;
using System;

namespace Engine
{
    internal interface IComponentStorage
    {
        void Remove(int entityIndex);
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
			Dispose (entityIndex);
            _components[entityIndex] = component;
        }

        public void Remove(int entityIndex)
        {
            if (_components.Count < entityIndex)
            {
				Dispose (entityIndex);
                _components[entityIndex] = default(T);
            }
        }

		void Dispose (int entityIndex)
		{
			if (ComponentType<T>.Disposable)
			{
				((IDisposable)_components [entityIndex]).Dispose ();
			}
		}
    }
}
