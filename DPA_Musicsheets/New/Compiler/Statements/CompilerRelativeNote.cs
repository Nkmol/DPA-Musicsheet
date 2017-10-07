using System;
using System.Collections.Generic;
using DPA_Musicsheets.Models;
using DPA_Musicsheets.New.Compiler.Nodes;
using DPA_Musicsheets.New.Compiler.Nodes.Abstractions;

namespace DPA_Musicsheets.New.Compiler.Statements
{
    public class CompilerRelativeNote : ICompilerStatement
    {
        public INode Compile(LinkedList<LilypondToken> tokens)
        {
            var node = new NodeContainer();

            var statements = new ICompilerStatement[] {new CompilerLetter(), new CompilerForceAmplitude()};
            foreach (var compilerStatement in statements)
            {
                var prop = compilerStatement.Compile(tokens);
                node.Properties.Add(prop);
            }

            if (tokens.First.Value.ValueToCompile != string.Empty)
                throw new Exception($"Relative note does not support the property of {tokens.First.Value.Value}");

            tokens.RemoveFirst(); // compiled succesful

            node.Context = CompilerType.RelativeNote;
            return node;
        }
    }
}