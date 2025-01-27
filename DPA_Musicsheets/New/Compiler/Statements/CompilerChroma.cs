﻿using System;
using System.Collections.Generic;
using System.Linq;
using DPA_Musicsheets.Models;
using DPA_Musicsheets.New.Compiler.Nodes;
using DPA_Musicsheets.New.Compiler.Nodes.Abstractions;

namespace DPA_Musicsheets.New.Compiler.Statements
{
    public class CompilerChroma : ICompilerStatement
    {
        private static readonly string[] PrefixChroma = {"es", "is"};


        public BaseNode Compile(LinkedList<LilypondToken> tokens)
        {
            var node = new Node();

            var value = tokens.First.Value.ValueToCompile;
            var check = value.Substring(0, 2);
            if (value.Length >= 2 && PrefixChroma.Contains(check))
                tokens.First.Value.ValueToCompile = value.Remove(0, 2);
            else
                throw new Exception($" \"{check}\" is not a valid chromaticis value");

            node.Context = CompilerType.Chroma;
            node.Value = check;
            return node;
        }
    }
}