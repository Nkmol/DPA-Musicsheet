using System;
using System.Collections.Generic;
using System.IO;
using Helpers.FileReaders;

namespace Helpers
{
    public class FileReader
    {
        private readonly string _path;
        private readonly string _extension;
        private readonly IFileReaderStrategy _readerStrategy;

        public FileReader(string path, IFileReaderStrategy readerStrategy)
        {
            _extension = path.SplitLastOf('.')[1];
            _path = path;

            _readerStrategy = readerStrategy;
        }

        public IEnumerable<string> ReadLine()
        {
            if (!Exists())
                throw new Exception("File does not exists.");

            if (IsEmpty())
                throw new Exception("This file is empty.");

            // Allow for yield return to break reading file early (if supported by the strategy)
            foreach (var yieldreturn in _readerStrategy.ReadFile(_path))
            {
                yield return yieldreturn;
            }
        }

        public bool HasExtension(string extension)
        {
            return Path.GetExtension(_path) == extension;
        }

        public bool IsEmpty()
        {
            return new FileInfo(_path).Length == 0;
        }

        public bool Exists()
        {
            return File.Exists(_path);
        }
    }
}