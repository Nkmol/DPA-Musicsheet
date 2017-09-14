using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.FileReaders
{
    public class TextReaderStrategy : IFileReaderStrategy
    {
        public IEnumerable<string> ReadFile(string path)
        {
            using (var reader = File.OpenText(path))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                    yield return line;
            }
        }
    }
}
