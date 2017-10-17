using System;
using System.Collections.Generic;
using DPA_Musicsheets.Models;
using DPA_Musicsheets.New.Compiler.Nodes;
using DPA_Musicsheets.New.Compiler.Nodes.Abstractions;

namespace DPA_Musicsheets.New.Compiler.Statements
{
    public class CompilerDot : ICompilerStatement
    {
        private const char Dot = '.';

        public BaseNode Compile(LinkedList<LilypondToken> tokens)
        {
            var node = new Node();

            var val = tokens.First.Value.ValueToCompile;
            var fChar = val[0];
            if (fChar == Dot)
                tokens.First.Value.ValueToCompile = val.Remove(0, 1);
            else
                throw new Exception(
                    $" \"{fChar}\" is not the expected Dot ('.') {tokens.First.Value.level} {tokens.First.Value.index}");

            node.Context = CompilerType.Dot;
            return node;
        }
    }
}