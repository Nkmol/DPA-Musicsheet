using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class Repeat
    {
        public int repeatCount
        {
            get => default(int);
            set
            {
            }
        }

        public Models.Note[] Notes
        {
            get => default(Models.Note[]);
            set
            {
            }
        }

        public Models.Volta[] Voltas
        {
            get => default(Models.Volta[]);
            set
            {
            }
        }
    }
}