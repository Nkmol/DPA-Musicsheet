using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.Models;

namespace DPA_Musicsheets.New.Compiler.Statements
{
    public class CompilerClef : ICompilerStatement
    {
        /// <summary>
        ///  More at: http://lilypond.org/doc/v2.19/Documentation/notation/clef-styles
        /// </summary>
        private static readonly string[] ValidClefs = { "G", "treble", "violin", "french", "GG", "tenorG", "soprano", "mezzosoprano" };

        private const string Keyword = "\\clef";

        public void Compile(LinkedList<LilypondToken> tokens)
        {
            if (tokens.First.Value.ValueToCompile != Keyword)
            {
                throw new Exception($"Expecting the start keyword {Keyword} for the Clef");
            }
            tokens.RemoveFirst(); // Succesfully compiled

            if (!ValidClefs.Contains(tokens.First.Value.ValueToCompile))
            {
                throw new Exception($"{tokens.First.Value.ValueToCompile} is not a supported Clef value");
            }
            tokens.RemoveFirst(); // Succesfully compiled
        }
    }
}
