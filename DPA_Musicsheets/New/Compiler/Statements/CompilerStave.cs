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
        public void Compile(LilypondToken currentTokens, ref LinkedList<LilypondToken> tokens)
        {
            if (tokens.First.Value.Value != Keyword)
            {
                throw new Exception();
            }
            tokens.RemoveFirst(); // compiled succesful

            // Compile the letter
            //CompilerFactory.Instance.Create(tokens.First.Value.TokenKind.ToString()).Compile(tokens.First.Value, ref tokens);
            tokens.RemoveFirst(); // TODO

            if (tokens.First.Value.Value != OpenBody)
            {
                throw new Exception();
            }
            tokens.RemoveFirst(); // compiled succesful

            while (tokens.First.Value.Value != CloseBody)
            {
                // Compile the letter
                CompilerFactory.Instance.Create(tokens.First.Value.TokenKind.ToString())?.Compile(tokens.First.Value, ref tokens);
                tokens.RemoveFirst(); // TODO
            }

            if (tokens.First.Value.Value != CloseBody)
            {
                throw new Exception();
            }
            tokens.RemoveFirst(); // compiled succesful

            // /realitve letter { body }
        }
    }
}
