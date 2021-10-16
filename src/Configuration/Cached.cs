#if UNITY_EDITOR

namespace Appalachia.Utility.Editor.AutoSave.Configuration
{
    internal abstract class Cached<T, TD>
    {
        private readonly TD _defaultValue;
        private readonly string _key;
        private T _lastValue;

        protected Cached(string key, TD defaultValue)
        {
            _key = key;
            _defaultValue = defaultValue;
            _lastValue = default;
        }

        public T Current
        {
            get
            {
                if (_lastValue == null)
                {
                    _lastValue = Get(_key, _defaultValue);
                }

                return _lastValue ?? default;
            }
            set
            {
                if (_lastValue == null)
                {
                    _lastValue = Get(_key, _defaultValue);
                }

                if (_lastValue.Equals(value))
                {
                    return;
                }

                Set(_key, value);
                _lastValue = value;
            }
        }

        protected abstract T Get(string key, TD defaultValue);
        protected abstract void Set(string key, T value);

        public static implicit operator T(Cached<T, TD> d)
        {
            return d.Current;
        }
    }
}

#endif
