using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.Models;
using DPA_Musicsheets.New.Compiler.Nodes;

namespace DPA_Musicsheets.New.Compiler.Statements
{
    public class CompilerStave : ICompilerStatement
    {
        private const string Keyword = "\\relative";
        private const string OpenBody = "{";
        private const string CloseBody = "}";

        private static readonly DispatchPropertyByType<NodeStave> DispatchType = new DispatchPropertyByType<NodeStave>(
            new Dictionary<Type, Action<NodeStave, INode>>()
            {
                { typeof(NodeRelativeNote), (node, value) => node.RelativeNote = value },
                { typeof(NodeClef), (node, value) => node.Clef = value },
                { typeof(NodeTempo), (node, value) => node.Tempo = value },
                { typeof(NodeTime), (node, value) => node.Time = value },
                { typeof(NodeNote), (node, value) => node.Notes.Add(value) },
            });

        public INode Compile(LinkedList<LilypondToken> tokens)
        {
            // \relative letter+amplitude { [clef + value] [time + value] [tempo + value] [ ... letters ] }
            NodeStave node = new NodeStave();

            if (tokens.First.Value.ValueToCompile != Keyword)
            {
                throw new Exception($"Expecting the start keyword {Keyword} for the stave");
            }
            tokens.RemoveFirst(); // compiled succesful

            // Compile the relative letter
            var relNote = new CompilerRelativeNote().Compile(tokens);
            DispatchType.AddProperty(node, relNote);

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

                // TODO: quickfix to bypass invalid chars
                if (statement == null)
                {
                    tokens.RemoveFirst();
                    continue;
                }

                var prop = statement.Compile(tokens);
                if(prop != null) DispatchType.AddProperty(node, prop);
            }

            if (tokens.First.Value.ValueToCompile != CloseBody)
            {
                throw new Exception();
            }

            tokens.RemoveFirst(); // compiled succesful

            return node;
        }
    }
}
