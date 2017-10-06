using System;
using System.Collections.Generic;
using DPA_Musicsheets.Models;
using DPA_Musicsheets.New.Compiler.Nodes;

namespace DPA_Musicsheets.New.Compiler.Statements
{
    public class CompilerNote : ICompilerStatement
    {
        // Setup mapping of possible characters at place
        private static readonly Dictionary<int, Func<ICompilerStatement[]>> PositionCharsMapping =
            new Dictionary<int, Func<ICompilerStatement[]>>
            {
                {0, () => new ICompilerStatement[] {new CompilerLetter()}},
                {
                    1,
                    () => new ICompilerStatement[]
                        {new CompilerForceAmplitude(), new CompilerNumber(), new CompilerChroma()}
                },
                {2, () => new ICompilerStatement[] {new CompilerNumber(), new CompilerDot()}},
                {3, () => new ICompilerStatement[] {new CompilerDot()}}
            };

        private static readonly DispatchPropertyByType<NodeNote> DispatchType = new DispatchPropertyByType<NodeNote>(
            new Dictionary<Type, Action<NodeNote, INode>>
            {
                {typeof(NodeLetter), (node, v) => node.NodeLetter = v},
                {typeof(NodeNumber), (node, v) => node.NodeNumber = v},
                {typeof(NodeAmplitude), (node, v) => node.NodeAmplitude = v},
                {typeof(NodeChroma), (node, v) => node.NodeChroma = v},
                {typeof(NodeDot), (node, v) => node.NodeDot = v}
            });

        public INode Compile(LinkedList<LilypondToken> tokens)
        {
            var note = new NodeNote();

            // Work the Note chararistics away
            // When a chararistic is gone, it means it has been succesfully compiled
            var i = 0;
            var firstToken = tokens.First.Value;
            while (!string.IsNullOrEmpty(firstToken.ValueToCompile))
            {
                PositionCharsMapping.TryGetValue(i, out var validFunction);
                if (validFunction == null || firstToken.ValueToCompile.Length > PositionCharsMapping.Count)
                    throw new Exception(
                        $"The letter contains too many specifications. A letter supports a total of {PositionCharsMapping.Count} properties"); // TODO CustomException

                Exception exception = null;
                foreach (var compilerStatement in validFunction())
                    try
                    {
                        var prop = compilerStatement.Compile(tokens);
                        DispatchType.AddProperty(note, prop);

                        // discard previous errors if succeeded
                        exception = null;
                        break;
                    }
                    catch (Exception e)
                    {
                        // Do not throw the error, first check other options
                        exception = e;
                    }

                // If none did succeed, throw latest error
                if (exception != null)
                    throw exception;

                i++;
            }

            tokens.RemoveFirst(); // Succesfully compiled

            return note;
        }
    }
}