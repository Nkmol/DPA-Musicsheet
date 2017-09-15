using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Midi
{
    public interface IFacadeMidi
    {
        EasyMidi LoadMidi(string path);
    }
}
