﻿using System;
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

        public void Compile(ref LinkedList<LilypondToken> tokens)
        {
            if (tokens.First.Value.Value != Keyword)
            {
                throw new Exception($"Expecting the start keyword {Keyword} for the Clef");
            }
            tokens.RemoveFirst(); // Succesfully compiled

            if (!ValidClefs.Contains(tokens.First.Value.Value))
            {
                throw new Exception($"{tokens.First.Value.Value} is not a supported Clef value");
            }
            tokens.RemoveFirst(); // Succesfully compiled
        }
    }
}
