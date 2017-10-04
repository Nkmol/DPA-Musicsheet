using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.Models;

namespace DPA_Musicsheets.New.Compiler.Statements
{
    public class CompilerChroma : ICompilerStatement
    {
        private static readonly string[] PrefixChroma = { "es", "is" };


        public void Compile(ref LinkedList<LilypondToken> tokens)
        {
            var value = tokens.First.Value.Value;
            var check = value.Substring(0, 2);
            if (value.Length >= 2 && PrefixChroma.Contains(check))
            {
                tokens.First.Value.Value = value.Remove(0, 2);
            }
            else
            {
                throw new Exception($" \"{check}\" is not a valid chromaticis value");
            }
        }
    }
}
