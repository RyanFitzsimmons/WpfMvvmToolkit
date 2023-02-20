using System.Collections.Generic;

namespace WpfMvvmToolkit
{
    public class NavigationParameters : Dictionary<string, object>
    {
        public bool TryGetValueOrDefault<T>(string key, out T? value)
        {
            if (!ContainsKey(key))
            {
                value = default;
                return false;
            }

            if (this[key] is not T t)
            {
                value = default;
                return false;
            }

            value = t;
            return true;
        }

        public T? GetValueOrDefault<T>(string key)
        {
            if (!ContainsKey(key))
            {
                return default;
            }

            if (this[key] is not T t)
            {
                return default;
            }

            return t;
        }

        public T GetValue<T>(string key)
        {
            return (T)this[key];
        }
    }
}
