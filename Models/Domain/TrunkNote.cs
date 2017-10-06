using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class TrunkNote : Note
    {
        public char Letter
        {
            get => default(char);
            set
            {
            }
        }

        public Chromaticism ChromaticismType
        {
            get => default(Chromaticism);
            set
            {
            }
        }

        public int Pitch { get; set; }
    }
}