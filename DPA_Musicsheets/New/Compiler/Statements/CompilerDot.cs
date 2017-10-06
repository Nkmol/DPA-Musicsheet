﻿using System;
using System.Collections.Generic;
using DPA_Musicsheets.Models;
using DPA_Musicsheets.New.Compiler.Nodes;

namespace DPA_Musicsheets.New.Compiler.Statements
{
    public class CompilerDot : ICompilerStatement
    {
        private const char Dot = '.';

        public INode Compile(LinkedList<LilypondToken> tokens)
        {
            var node = new NodeDot();

            var val = tokens.First.Value.ValueToCompile;
            var fChar = val[0];
            if (fChar == Dot)
                tokens.First.Value.ValueToCompile = val.Remove(0, 1);
            else
                throw new Exception(
                    $" \"{fChar}\" is not the expected Dot ('.') {tokens.First.Value.level} {tokens.First.Value.index}");

            return node;
        }
    }
}