using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.Models;

namespace DPA_Musicsheets.New.Compiler
{
    public class Compiler
    {
        public static void Run(LinkedList<LilypondToken> tokens)
        {
            var factory = new CompilerFactory();
            while (tokens.Count > 0)
            {
                var statement = factory.Create(tokens.First.Value.TokenKind.ToString());
                statement.Compile(tokens);
            }
        }
    }
}
