﻿using System;
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
        public Double Time
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
    }
}