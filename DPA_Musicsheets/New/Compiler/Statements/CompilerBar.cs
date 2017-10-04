using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.Models;

namespace DPA_Musicsheets.New.Compiler.Statements
{
    public class CompilerBar : ICompilerStatement
    {
        private static string[] BarTypes = {"|"};
        public void Compile(LinkedList<LilypondToken> tokens)
        {
            var value = tokens.First.Value;
            if (value.PreviousToken.TokenKind != LilypondTokenKind.Note)
            {
                throw new Exception($"A Bar is expected to be after a Note");
            }

            if (!BarTypes.Contains(value.Value))
            {
                throw new Exception($"{value.Value} is not a supported Bar type");
            }

            tokens.RemoveFirst();
        }
    }
}
