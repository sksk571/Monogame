using System;
using System.Collections.Generic;

namespace Engine
{
    internal class ComponentType
    {
        private static int _count;
        private const int _max = 64;

        static ComponentType()
        {
            _count = 0;
        }

        protected static int Add()
        {
            if (_count == _max)
            {
                throw new NotSupportedException("Max amount of component types is reached.");
            }
            return _count++;
        }

        public static IList<int> GetIndexes(long mask)
        {
            List<int> indexes = new List<int> ();
            for (int index = 0; index < _count; ++index)
            {
                if (((mask >> index) & 1L) == 1L)
                    indexes.Add (index);
            }
            return indexes;
        }
    }

    internal class ComponentType<T> : ComponentType
    {
        private static readonly int _index;
        private static readonly long _mask;
		private static readonly bool _disposable;

        static ComponentType()
        {
            _index = Add();
            _mask = 1L << _index;
			_disposable = typeof(IDisposable).IsAssignableFrom (typeof(T));
        }

        public static int Index
        {
            get { return _index; }
        }

        public static long Mask
        {
            get { return _mask; }
        }

		public static bool Disposable
		{
			get  { return _disposable; }
		}
    }
}
