using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models.Domain;

namespace Models
{
    public class TrunkNote : Note
    {
        public char Letter { get; set; }

        public Chromaticism ChromaticismType { get; set; } = Chromaticism.None;

        public int Pitch { get; set; }

        public static Chromaticism TranslateChromaticism(string value)
        {
            return value == "es" || value == "as" ? Chromaticism.Mol : Chromaticism.Cross;
        }

        private static readonly List<char> NoteOrder = new List<char> { 'c', 'd', 'e', 'f', 'g', 'a', 'b' };
        public static int GetNoteOrderIndex(char letter)
        {
            letter = char.ToLowerInvariant(letter);
            return NoteOrder.IndexOf(letter);
        }
    }
}