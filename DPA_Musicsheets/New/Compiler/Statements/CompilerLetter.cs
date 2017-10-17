using System;
using System.Collections.Generic;
using System.Linq;
using DPA_Musicsheets.Models;
using DPA_Musicsheets.New.Compiler.Nodes;
using DPA_Musicsheets.New.Compiler.Nodes.Abstractions;

namespace DPA_Musicsheets.New.Compiler.Statements
{
    public class CompilerLetter : ICompilerStatement
    {
        private static readonly char[] ValidLetters = {'a', 'b', 'c', 'd', 'e', 'f', 'g'};

        public BaseNode Compile(LinkedList<LilypondToken> tokens)
        {
            var node = new Node();

            var val = tokens.First.Value.ValueToCompile;
            var fChar = val[0];
            if (ValidLetters.Contains(fChar))
                tokens.First.Value.ValueToCompile = val.Remove(0, 1);
            else
                throw new Exception($" \"{fChar}\" is not a valid letter");

            node.Context = CompilerType.Letter;
            node.Value = fChar.ToString();
            return node;
        }
    }
}