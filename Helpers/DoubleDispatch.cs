using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    public abstract class DoubleDispatch<T>
    {
        // Solution for double dispatching type based, without use of "dynamic"
        protected Dictionary<Type, T> DispatchByType = new Dictionary<Type, T>();

        public virtual T Dispatch(Type type)
        {
            DispatchByType.TryGetValue(type, out var result);
            return result;
        }
    }
}
