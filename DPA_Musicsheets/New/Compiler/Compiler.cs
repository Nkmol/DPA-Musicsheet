using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.Models;
using DPA_Musicsheets.New.Compiler.Nodes;

namespace DPA_Musicsheets.New.Compiler
{
    public class Compiler
    {
        public static IEnumerable<INode> Run(LinkedList<LilypondToken> tokens)
        {
            var factory = new CompilerFactory();
            var nodes = new List<INode>();
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
