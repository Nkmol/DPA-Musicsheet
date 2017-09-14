using System;

namespace Helpers.Datatypes
{
    public class Singleton<T>
        where T : class
    {
        private static readonly Lazy<T> InstanceHolder =
            new Lazy<T>(() => (T)Activator.CreateInstance(typeof(T), true));

        protected Singleton()
        {
        }

        public static T Instance => InstanceHolder.Value;
    }
}