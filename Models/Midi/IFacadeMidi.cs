using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Midi
{
    public interface IFacadeMidi
    {
        IEnumerable<string> LoadMidi(string path);
    }
}
