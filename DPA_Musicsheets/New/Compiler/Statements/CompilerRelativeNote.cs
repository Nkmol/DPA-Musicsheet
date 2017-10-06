using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.Models;
using DPA_Musicsheets.New.Compiler.Nodes;

namespace DPA_Musicsheets.New.Compiler.Statements
{
    public class CompilerRelativeNote : ICompilerStatement
    {
        public INode Compile(LinkedList<LilypondToken> tokens)
        {
            NodeRelativeNote node = new NodeRelativeNote();

            var statements = new ICompilerStatement[] {new CompilerLetter(), new CompilerForceAmplitude()};
            foreach (var compilerStatement in statements)
            {
                var prop = compilerStatement.Compile(tokens);
                AddProperty((dynamic)prop, node);
            }

            if (tokens.First.Value.ValueToCompile != String.Empty)
            {
                throw new Exception($"Relative note does not support the property of {tokens.First.Value.Value}");
            }

            tokens.RemoveFirst(); // compiled succesful

            return node;
        }

        public void AddProperty(NodeLetter property, NodeRelativeNote note)
        {
            note.NodeLetter = property;
        }
        public void AddProperty(NodeAmplitude property, NodeRelativeNote note)
        {
            note.NodeAmplitude = property;
        }
    }
}
