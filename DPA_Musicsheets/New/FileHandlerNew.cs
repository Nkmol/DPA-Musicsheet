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
        // TODO: Make sure "yield" sequence is preserved at build stage (so beyond this). At the moment we simply append them all to one result
        public string OpenFile(string path)
        {
            var strategy = FileReaderStrategyFactory.Instance.Create(Path.GetExtension(path));
            FileReader fr = new FileReader(path, strategy);

            var result = "";
            foreach (var s in fr.ReadLine())
            {
                result += s;
            }

            return result;
        }
    }
}
