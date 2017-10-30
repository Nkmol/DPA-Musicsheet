using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DPA_Musicsheets.Models;
using DPA_Musicsheets.New.Compiler.Nodes;
using DPA_Musicsheets.New.Compiler.Nodes.Abstractions;

namespace DPA_Musicsheets.New.Compiler.Statements
{
    public class CompileLength : ICompilerStatement
    {
        private static readonly int[] ValidNumbers = {1, 2, 4, 8, 16, 32};

        public BaseNode Compile(LinkedList<LilypondToken> tokens)
        {
            var node = new Node();

            var val = tokens.First.Value.ValueToCompile;
            var firstNumbers = Regex.Match(val, "^[0-9]*").Value;
            if (ValidNumbers.Contains(int.Parse(firstNumbers)))
                tokens.First.Value.ValueToCompile = val.Remove(0, firstNumbers.Length);
            else
                throw new Exception($" \"{firstNumbers}\" is not a valid number");

            node.Context = CompilerType.Length;
            node.Value = firstNumbers;
            return node;
        }
    }
}