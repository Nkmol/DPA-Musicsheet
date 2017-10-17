using System.Collections.Generic;
using DPA_Musicsheets.Models;
using DPA_Musicsheets.New.Compiler.Nodes;
using DPA_Musicsheets.New.Compiler.Nodes.Abstractions;

namespace DPA_Musicsheets.New.Compiler
{
    public class Compiler
    {
        public static IEnumerable<BaseNode> Run(LinkedList<LilypondToken> tokens)
        {
            var factory = new CompilerFactory();
            var nodes = new List<BaseNode>();
            while (tokens.Count > 0)
            {
                var statement = factory.Create(tokens.First.Value.TokenKind.ToString());
                var node = statement.Compile(tokens);

                nodes.Add(node);
            }

            return nodes;
        }
    }
}