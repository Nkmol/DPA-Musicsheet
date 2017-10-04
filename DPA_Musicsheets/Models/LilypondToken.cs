using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models
{
    public class LilypondToken
    {
        public LilypondTokenKind TokenKind { get; set; }
        public string Value { get; set; }
        // Value that still has to be compiled
        public string ValueToCompile { get; set; }

        public LilypondToken NextToken { get; set; }

        public LilypondToken PreviousToken { get; set; }

        public int index { get; set; }
        public int level { get; set; }


        public LilypondToken(LilypondTokenKind l, string v)
        {
            ValueToCompile = Value = v;
            TokenKind = l;
        }
    }

    
}
