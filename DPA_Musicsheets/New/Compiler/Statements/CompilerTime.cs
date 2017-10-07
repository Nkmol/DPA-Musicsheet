using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DPA_Musicsheets.Models;
using DPA_Musicsheets.New.Compiler.Nodes;
using DPA_Musicsheets.New.Compiler.Nodes.Abstractions;

namespace DPA_Musicsheets.New.Compiler.Statements
{
    public class CompilerTime : ICompilerStatement
    {
        /// <summary>
        ///     More at: http://lilypond.org/doc/v2.18/Documentation/notation/displaying-rhythms#time-signature
        /// </summary>
        private const string Keyword = "\\time";

        private const string RegexPattern = "^[0-9]/[0-9]";

        public INode Compile(LinkedList<LilypondToken> tokens)
        {
            var node = new Node();

            if (tokens.First.Value.ValueToCompile != Keyword)
                throw new Exception($"Expecting the start keyword {Keyword} for the Time");
            tokens.RemoveFirst(); // Succesfully compiled

            var valueMatch = Regex.Match(tokens.First.Value.ValueToCompile, RegexPattern);
            if (valueMatch.Length == 0)
                throw new Exception($"{tokens.First.Value.ValueToCompile} is not a supported Clef value");
            tokens.RemoveFirst(); // Succesfully compiled

            node.Context = CompilerType.Time;
            node.Value = valueMatch.Value;
            return node;
        }
    }
}