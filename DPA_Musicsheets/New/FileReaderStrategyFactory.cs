using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datatypes;
using Helpers.Datatypes;
using Helpers.Datatypes.Factories;
using Helpers.FileReaders;

namespace DPA_Musicsheets.New
{
    public class FileReaderStrategyFactory : Singleton<FileReaderStrategyFactory>, IFactory<IFileReaderStrategy>
    {
        private readonly IFactory<IFileReaderStrategy> _factory = new Factory<IFileReaderStrategy>();

        public FileReaderStrategyFactory()
        {
            AddType(".mid", typeof(MidiReadStrategy));
            AddType(".ly", typeof(TextReaderStrategy));
        }
        public void AddType(string typenaming, Type type)
        {
            _factory.AddType(typenaming, type);
        }

        public IFileReaderStrategy Create(string type)
        {
            return _factory.Create(type);
        }
    }
}
