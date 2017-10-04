using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.New;
using Helpers;
using Sanford.Multimedia.Midi;
using DPA_Musicsheets.Models;
using System.Text.RegularExpressions;

namespace DPA_Musicsheets.New
{
    class Tokenizer
    {
        private Dictionary<string, LilypondTokenKind> lookuptable = new Dictionary<string, LilypondTokenKind>();


        public Tokenizer()
        {
            lookuptable.Add("|", LilypondTokenKind.Bar);
            lookuptable.Add("\\clef", LilypondTokenKind.Clef);
            lookuptable.Add("rest", LilypondTokenKind.Rest);
            lookuptable.Add("\\relative", LilypondTokenKind.Staff);
            lookuptable.Add("\\tempo", LilypondTokenKind.Tempo);
            lookuptable.Add("\\time", LilypondTokenKind.Time);
            lookuptable.Add("n", LilypondTokenKind.Note);
            lookuptable.Add("u", LilypondTokenKind.Unknown);

        }

        public LilypondToken Tokenize(string s)
        {
            LilypondToken t;
            if (lookuptable.ContainsKey(s))
            {
                 t = new LilypondToken(lookuptable[s], s);
            }
            else
            {
                if (new Regex(@"[a-g][,'eis]*[0-9]+[.]*").IsMatch(s))
                {
                    t = new LilypondToken(lookuptable["n"], s);
                }
                else if (new Regex(@"r.*?[0-9][.]*").IsMatch(s))
                {
                    t = new LilypondToken(lookuptable["rest"], s);
                }
                else
                {
                    t = new LilypondToken(lookuptable["u"], s);
                }


            }
            return t;
        }




    }
}


