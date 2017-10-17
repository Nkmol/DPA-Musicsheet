using System;
using System.Collections.Generic;
using DPA_Musicsheets.Models;
using DPA_Musicsheets.New.Compiler.Nodes;
using DPA_Musicsheets.New.Compiler.Nodes.Abstractions;

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
                        {new CompilerForceAmplitude(), new CompileLength(), new CompilerChroma()}
                },
                {2, () => new ICompilerStatement[] {new CompileLength(), new CompilerDot()}},
                {3, () => new ICompilerStatement[] {new CompilerDot()}}
            };

        public BaseNode Compile(LinkedList<LilypondToken> tokens)
        {
            var note = new NodeContainer();

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
                        note.Properties.Add(prop);

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

            note.Context = CompilerType.Note;
            return note;
        }
    }
}