using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Midi
{
    public interface IFacadeMidi
    {
        int Division { get; set; }
        int BeatNote { get; set; }
        int BeatsPerBar { get; set; }
        int Bmp { get; set; }
        double PercentageOfBarReached { get; set; }
        IList<string> Notes { get; set; }
        void LoadMidi(string path);
    }
}
