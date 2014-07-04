using System;

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
    }

    internal class ComponentType<T> : ComponentType
    {
        private static readonly int _index;
        private static readonly long _mask;

        static ComponentType()
        {
            _index = Add();
            _mask = 1L << _index;
        }

        public static int Index
        {
            get { return _index; }
        }

        public static long Mask
        {
            get { return _mask; }
        }
    }
}
