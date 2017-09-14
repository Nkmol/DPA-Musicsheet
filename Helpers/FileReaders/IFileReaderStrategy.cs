using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.FileReaders
{
    public interface IFileReaderStrategy
    {
        IEnumerable<string> ReadFile(string path);
    }
}
