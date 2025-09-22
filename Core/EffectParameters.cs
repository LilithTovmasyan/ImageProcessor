using System;
using System.Collections.Generic;


namespace ImageProcessor.Core
{
    public sealed class EffectParameters
    {
        private readonly Dictionary<string, object?> _values = new(StringComparer.OrdinalIgnoreCase);


        public void Set(string name, object? value) => _values[name] = value;


        public T Get<T>(string name, T? @default = default)
        {
            if (_values.TryGetValue(name, out var v) && v is not null)
            {
                if (v is T t) return t;
                return (T)Convert.ChangeType(v, typeof(T));
            }
            return @default!;
        }


        public override string ToString() => string.Join(", ", _values);
    }
}