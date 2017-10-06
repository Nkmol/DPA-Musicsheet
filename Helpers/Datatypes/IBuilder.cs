using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.Datatypes
{
    public interface IBuilder<T>
    {
        T Build();
    }
}
