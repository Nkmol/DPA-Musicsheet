using System;
using System.Collections.Generic;
using System.Linq;
using DPA_Musicsheets.Models;
using DPA_Musicsheets.New.Compiler.Nodes;
using DPA_Musicsheets.New.Compiler.Nodes.Abstractions;

namespace DPA_Musicsheets.New.Compiler.Statements
{
    public class CompilerBar : ICompilerStatement
    {
        private static readonly string[] BarTypes = {"|"};

        public BaseNode Compile(LinkedList<LilypondToken> tokens)
        {
            var node = new Node();

            var value = tokens.First.Value;
            if (value.PreviousToken.TokenKind != LilypondTokenKind.Note)
                throw new Exception($"A Bar is expected to be after a Note");

            if (!BarTypes.Contains(value.Value))
                throw new Exception($"{value.Value} is not a supported Bar type");

            tokens.RemoveFirst();

            node.Context = CompilerType.Bar;
            return node;
        }
    }
}