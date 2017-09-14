using System;
using System.Collections.Generic;

namespace Datatypes
{
    public class Factory<T> : IFactory<T>
        where T : class
    {
        protected readonly Dictionary<string, Type> Types =
            new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);

        public void AddType(string typenaming, Type type)
        {
            Types[typenaming] = type;
        }

        public T Create(string type)
        {
            if (Types.TryGetValue(type, out var t))
                return (T)Activator.CreateInstance(t);

            return null;
        }
    }
}