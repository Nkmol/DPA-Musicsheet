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
        private readonly LinkedList<LilypondToken> _tokens;
        private readonly CompilerFactory _factory;

        public Compiler(LinkedList<LilypondToken> tokens)
        {
            _tokens = tokens;
            _factory = new CompilerFactory();
        }

        public void Run()
        {
            foreach (var lilypondToken in _tokens)
            {
                var statement = _factory.Create(lilypondToken.TokenKind.ToString());
                statement?.Compile(lilypondToken);
            }
        }
    }
}
