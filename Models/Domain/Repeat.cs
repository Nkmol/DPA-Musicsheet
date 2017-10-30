using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models.Domain;

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

        public Note[] Notes
        {
            get => default(Note[]);
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