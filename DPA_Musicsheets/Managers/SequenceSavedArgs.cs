using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Managers
{
    public class SequenceSavedArgs : EventArgs
    {
        public bool Result { get; set; }
        public string Message { get; set; }
    }
}
