using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DPA_Musicsheets.Models;

namespace DPA_Musicsheets.New.Compiler.Statements
{
    public class CompilerTime : ICompilerStatement
    {
        /// <summary>
        ///  More at: http://lilypond.org/doc/v2.18/Documentation/notation/displaying-rhythms#time-signature
        /// </summary>

        private const string Keyword = "\\time";

        private const string RegexPattern = "^[0-9]/[0-9]";

        public void Compile(ref LinkedList<LilypondToken> tokens)
        {
            if (tokens.First.Value.Value != Keyword)
            {
                throw new Exception($"Expecting the start keyword {Keyword} for the Time");
            }
            tokens.RemoveFirst(); // Succesfully compiled

            if (Regex.Match(tokens.First.Value.Value, RegexPattern).Length == 0)
            {
                throw new Exception($"{tokens.First.Value.Value} is not a supported Clef value");
            }
            tokens.RemoveFirst(); // Succesfully compiled
        }
    }
}
