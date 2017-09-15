using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Midi
{
    public class EasyMidi
    {
        // unit of time for delta timing. If the value is positive, then it represents the units per beat. 
        // For example, +96 would mean 96 ticks per beat. If the value is negative, delta times are in SMPTE compatible units.
        public int Division { get; set; }
        public int BeatNote { get; set; }
        public int BeatsPerBar { get; set; }
        public int Bmp { get; set; }
        public double PercentageOfBarReached { get; set; }
        /// <summary>
        /// All the in-order notes in a representation of duration + dot notation. For example: "8.", "16"
        /// </summary>
        public IList<string> Notes { get; set; }

        public EasyMidi()
        {
            Notes = new List<string>();
        }
    }
}
