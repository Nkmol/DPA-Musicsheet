using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DPA_Musicsheets.Models;
using DPA_Musicsheets.New.Compiler.Nodes;

namespace DPA_Musicsheets.New.Compiler.Statements
{
    public class CompilerTime : ICompilerStatement
    {
        /// <summary>
        ///  More at: http://lilypond.org/doc/v2.18/Documentation/notation/displaying-rhythms#time-signature
        /// </summary>

        private const string Keyword = "\\time";

        private const string RegexPattern = "^[0-9]/[0-9]";

        public INode Compile(LinkedList<LilypondToken> tokens)
        {
            NodeTime node = new NodeTime();

            if (tokens.First.Value.ValueToCompile != Keyword)
            {
                throw new Exception($"Expecting the start keyword {Keyword} for the Time");
            }
            tokens.RemoveFirst(); // Succesfully compiled

            var valueMatch = Regex.Match(tokens.First.Value.ValueToCompile, RegexPattern);
            if (valueMatch.Length == 0)
            {
                throw new Exception($"{tokens.First.Value.ValueToCompile} is not a supported Clef value");
            }
            tokens.RemoveFirst(); // Succesfully compiled

            node.Value = valueMatch.Value;
            return node;
        }
    }
}
