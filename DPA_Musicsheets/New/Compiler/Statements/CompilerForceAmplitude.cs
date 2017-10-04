using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.Models;

namespace DPA_Musicsheets.New.Compiler.Statements
{
    public class CompilerForceAmplitude : ICompilerStatement
    {
        private static readonly char[] ApastrofComma = { ',', '\'' };

        public void Compile(ref LinkedList<LilypondToken> tokens)
        {
            var val = tokens.First.Value.Value;
            var fChar = val[0];
            if (ApastrofComma.Contains(fChar))
            {
                tokens.First.Value.Value = val.Remove(0, 1);
            }
            else
            {
                throw new Exception($" \"{fChar}\" is not a valid force keyword");
            }
        }
    }
}
