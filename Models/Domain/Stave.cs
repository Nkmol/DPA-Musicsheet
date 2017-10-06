using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class Stave
    {
        /// <summary>
        /// time signature
        /// </summary>
        public float Time
        {
            get => default(float);
            set
            {
            }
        }

        public IList<Note> Notes
        {
            get => default(IList<Note>);
            set
            {
            }
        }

        public Note RelativeNote { get; set; }
        public string Tempo { get; set; }
        public string Clef { get; set; }
    }
}