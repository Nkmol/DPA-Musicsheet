using System;
using System.Collections.Generic;
using DPA_Musicsheets.Models;
using DPA_Musicsheets.New.Compiler.Nodes;
using DPA_Musicsheets.New.Compiler.Nodes.Abstractions;

namespace DPA_Musicsheets.New.Compiler.Statements
{
    public class CompilerStave : ICompilerStatement
    {
        private const string Keyword = "\\relative";
        private const string OpenBody = "{";
        private const string CloseBody = "}";

        public BaseNode Compile(LinkedList<LilypondToken> tokens)
        {
            // \relative letter+amplitude { [clef + value] [time + value] [tempo + value] [ ... letters ] }
            var node = new NodeContainer();

            if (tokens.First.Value.ValueToCompile != Keyword)
                throw new Exception($"Expecting the start keyword {Keyword} for the stave");
            tokens.RemoveFirst(); // compiled succesful

            // Compile the relative letter
            var relNote = new CompilerRelativeNote().Compile(tokens);
            node.Properties.Add(relNote);

            // Compile openbody tag
            if (tokens.First.Value.ValueToCompile != OpenBody)
                throw new Exception();
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
                if (prop != null) node.Properties.Add(prop);
            }

            if (tokens.First.Value.ValueToCompile != CloseBody)
                throw new Exception();

            tokens.RemoveFirst(); // compiled succesful

            node.Context = CompilerType.Stave;
            return node;
        }
    }
}