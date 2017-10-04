using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DPA_Musicsheets.Models;

namespace DPA_Musicsheets.New.Compiler.Statements
{
    class CompilerTempo : ICompilerStatement
    {
        /// <summary>
        ///  More at: http://lilypond.org/doc/v2.18/Documentation/notation/displaying-rhythms#metronome-marks
        /// </summary>

        private const string Keyword = "\\tempo";

        private const string RegexPattern = "^4=[\\d]{1,2}";

        public void Compile(LinkedList<LilypondToken> tokens)
        {
            if (tokens.First.Value.ValueToCompile != Keyword)
            {
                throw new Exception($"Expecting the start keyword {Keyword} for the Tempo");
            }
            tokens.RemoveFirst(); // Succesfully compiled

            if (Regex.Match(tokens.First.Value.ValueToCompile, RegexPattern).Length == 0)
            {
                throw new Exception($"{tokens.First.Value.ValueToCompile} is not a supported Tempo value");
            }
            tokens.RemoveFirst(); // Succesfully compiled
        }
    }
}
