using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.Models;

namespace DPA_Musicsheets.New.Compiler.Statements
{
    public class CompilerStave : ICompilerStatement
    {
        private const string Keyword = "\\relative";
        private const string OpenBody = "{";
        private const string CloseBody = "}";
        public void Compile(ref LinkedList<LilypondToken> tokens)
        {
            // \relative letter+amplitude { [clef + value] [time + value] [tempo + value] [ ... letters ] }

            if (tokens.First.Value.ValueToCompile != Keyword)
            {
                throw new Exception($"Expecting the start keyword {Keyword} for the stave");
            }
            tokens.RemoveFirst(); // compiled succesful

            // Compile the relative letter
            new CompilerRelativeNote().Compile(ref tokens);

            // Compile openbody tag
            if (tokens.First.Value.ValueToCompile != OpenBody)
            {
                throw new Exception();
            }
            tokens.RemoveFirst(); // compiled succesful

            // Compile body
            while (tokens.First.Value.ValueToCompile != CloseBody)
            {
                var statement = CompilerFactory.Instance.Create(tokens.First.Value.TokenKind.ToString());
                if (statement == null)
                {
                    // Non supported for compiler
                    tokens.RemoveFirst();
                    continue;
                }
                statement.Compile(ref tokens);
            }

            if (tokens.First.Value.ValueToCompile != CloseBody)
            {
                throw new Exception();
            }
            tokens.RemoveFirst(); // compiled succesful
        }
    }
}
