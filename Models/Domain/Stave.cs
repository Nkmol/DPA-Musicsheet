using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models.Domain;

namespace Models
{
    public class Stave : IObject
    {
        public Stave()
        {
            Notes = new List<Note>();
        }

        /// <summary>
        /// time signature
        /// </summary>
        public string Time { get; set; }

        public IList<Note> Notes { get; set; }

        public Note RelativeNote { get; set; }
        public string Tempo { get; set; }
        public string Clef { get; set; }
    }
}