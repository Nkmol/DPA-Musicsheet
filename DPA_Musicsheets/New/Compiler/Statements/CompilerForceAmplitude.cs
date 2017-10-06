using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.Models;
using DPA_Musicsheets.New.Compiler.Nodes;

namespace DPA_Musicsheets.New.Compiler.Statements
{
    public class CompilerForceAmplitude : ICompilerStatement
    {
        private static readonly char[] ApastrofComma = { ',', '\'' };

        public INode Compile(LinkedList<LilypondToken> tokens)
        {
            var val = tokens.First.Value.ValueToCompile;
            var fChar = val[0];
            if (ApastrofComma.Contains(fChar))
            {
                tokens.First.Value.ValueToCompile = val.Remove(0, 1);
            }
            else
            {
                throw new Exception($" \"{fChar}\" is not a valid force keyword");
            }

            return new NodeAmplitude {Value = fChar.ToString()};
        }
    }
}
