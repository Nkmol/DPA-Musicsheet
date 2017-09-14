using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.New;
using Helpers;
using Sanford.Multimedia.Midi;

namespace DPA_Musicsheets.Managers
{
    public class FileHandlerNew
    {
        public string OpenFile(string path)
        {
            var strategy = FileReaderStrategyFactory.Instance.Create(Path.GetExtension(path));
            FileReader fr = new FileReader(path, strategy);

            return fr.ReadLine().First();
        }
    }
}
