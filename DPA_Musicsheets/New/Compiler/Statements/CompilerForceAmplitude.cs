using System;
using System.Collections.Generic;
using System.Linq;
using DPA_Musicsheets.Models;
using DPA_Musicsheets.New.Compiler.Nodes;
using DPA_Musicsheets.New.Compiler.Nodes.Abstractions;

namespace DPA_Musicsheets.New.Compiler.Statements
{
    public class CompilerForceAmplitude : ICompilerStatement
    {
        private static readonly char[] ApastrofComma = {',', '\''};

        public BaseNode Compile(LinkedList<LilypondToken> tokens)
        {
            var node = new Node();

            var val = tokens.First.Value.ValueToCompile;
            var fChar = val[0];
            if (ApastrofComma.Contains(fChar))
                tokens.First.Value.ValueToCompile = val.Remove(0, 1);
            else
                throw new Exception($" \"{fChar}\" is not a valid force keyword");

            node.Context = CompilerType.ForceAmplitude;
            //node.Value = fChar.ToString();

            if (!char.IsWhiteSpace(fChar))
            {
                node.Value = fChar == ',' ? (--Compiler.Octave).ToString() : (++Compiler.Octave).ToString();
            }

            return node;
        }
    }
}