using System;
using System.Collections.Generic;
using System.Linq;
using DPA_Musicsheets.Models;
using DPA_Musicsheets.New.Compiler.Nodes;
using DPA_Musicsheets.New.Compiler.Nodes.Abstractions;

namespace DPA_Musicsheets.New.Compiler.Statements
{
    public class CompilerClef : ICompilerStatement
    {
        private const string Keyword = "\\clef";

        /// <summary>
        ///     More at: http://lilypond.org/doc/v2.19/Documentation/notation/clef-styles
        /// </summary>
        private static readonly string[] ValidClefs =
            {"G", "treble", "violin", "french", "GG", "tenorG", "soprano", "mezzosoprano"};

        public BaseNode Compile(LinkedList<LilypondToken> tokens)
        {
            var node = new Node();

            if (tokens.First.Value.ValueToCompile != Keyword)
                throw new Exception($"Expecting the start keyword {Keyword} for the Clef");
            tokens.RemoveFirst(); // Succesfully compiled

            var value = tokens.First.Value.ValueToCompile;
            if (!ValidClefs.Contains(value))
                throw new Exception($"{tokens.First.Value.ValueToCompile} is not a supported Clef value");
            tokens.RemoveFirst(); // Succesfully compiled

            node.Context = CompilerType.Clef;
            node.Value = value;
            return node;
        }
    }
}