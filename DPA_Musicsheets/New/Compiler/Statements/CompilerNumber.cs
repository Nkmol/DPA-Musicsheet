using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DPA_Musicsheets.Models;

namespace DPA_Musicsheets.New.Compiler.Statements
{
    public class CompilerNumber : ICompilerStatement
    {
        private static readonly int[] ValidNumbers = { 1, 2, 4, 8, 16 };

        public void Compile(ref LinkedList<LilypondToken> tokens)
        {
            var val = tokens.First.Value.ValueToCompile;
            var firstNumbers = Regex.Match(val, "^[0-9]*").Value;
            if (ValidNumbers.Contains(int.Parse(firstNumbers)))
            {
                tokens.First.Value.ValueToCompile = val.Remove(0, firstNumbers.Length);
            }
            else
            {
                throw new Exception($" \"{firstNumbers}\" is not a valid number");
            }
        }
    }
}
