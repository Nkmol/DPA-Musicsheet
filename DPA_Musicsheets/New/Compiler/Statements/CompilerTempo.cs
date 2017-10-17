using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DPA_Musicsheets.Models;
using DPA_Musicsheets.New.Compiler.Nodes;
using DPA_Musicsheets.New.Compiler.Nodes.Abstractions;

namespace DPA_Musicsheets.New.Compiler.Statements
{
    internal class CompilerTempo : ICompilerStatement
    {
        /// <summary>
        ///     More at: http://lilypond.org/doc/v2.18/Documentation/notation/displaying-rhythms#metronome-marks
        /// </summary>
        private const string Keyword = "\\tempo";

        private const string RegexPattern = "^4=[\\d]{1,2}";

        public BaseNode Compile(LinkedList<LilypondToken> tokens)
        {
            var node = new Node();

            if (tokens.First.Value.ValueToCompile != Keyword)
                throw new Exception($"Expecting the start keyword {Keyword} for the Tempo");
            tokens.RemoveFirst(); // Succesfully compiled

            var valueMatch = Regex.Match(tokens.First.Value.ValueToCompile, RegexPattern);
            if (valueMatch.Length == 0)
                throw new Exception($"{tokens.First.Value.ValueToCompile} is not a supported Tempo value");
            tokens.RemoveFirst(); // Succesfully compiled

            node.Context = CompilerType.Tempo;
            node.Value = valueMatch.Value;
            return node;
        }
    }
}