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
        private LinkedList<LilypondToken> _tokens;
        private readonly CompilerFactory _factory;

        public Compiler(LinkedList<LilypondToken> tokens)
        {
            _tokens = tokens;
            _factory = new CompilerFactory();
        }

        public void Run()
        {
            while(_tokens.Count > 0)
            {
                var statement = _factory.Create(_tokens.First.Value.TokenKind.ToString());
                statement?.Compile(ref _tokens);
            }
        }
    }
}
