using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class TrunkNote : Note
    {
        public char Letter { get; set; }

        public Chromaticism ChromaticismType { get; set; }

        public int Pitch { get; set; }

        public static Chromaticism TranslateChromaticism(string value)
        {
            return value == "es" || value == "as" ? Chromaticism.Mol : Chromaticism.Cross;
        }

        public int ChromaticismAlter => ChromaticismType == Chromaticism.Mol ? -1 : 1;
    }
}