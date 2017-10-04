using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using DPA_Musicsheets.Models;

namespace DPA_Musicsheets.New.Compiler.Statements
{
    public class CompilerNote : ICompilerStatement
    {
        private readonly Dictionary<int, Func<ICompilerStatement[]>> _positionCharsMapping = new Dictionary<int, Func<ICompilerStatement[]>>();

        public void Compile(LinkedList<LilypondToken> tokens)
        {
            // - Setup mapping of possible characters at place -
            // Possible at place 1
            _positionCharsMapping.Add(0, () => new ICompilerStatement[] { new CompilerLetter() });
            // Possible at place 2
            _positionCharsMapping.Add(1, () => new ICompilerStatement[] { new CompilerForceAmplitude(), new CompilerNumber(), new CompilerChroma() });
            // Possible at place 3
            _positionCharsMapping.Add(2, () => new ICompilerStatement[] { new CompilerNumber(), new CompilerDot()});
            // Possible at place 4
            _positionCharsMapping.Add(3, () => new ICompilerStatement[] { new CompilerDot() });

            // Work the Note chararistics away
            // When a chararistic is gone, it means it has been succesfully compiled
            var i = 0;
            var firstToken = tokens.First.Value;
            while (!string.IsNullOrEmpty(firstToken.ValueToCompile))
            {
                _positionCharsMapping.TryGetValue(i, out var validFunction);
                if (validFunction == null || firstToken.ValueToCompile.Length > _positionCharsMapping.Count)
                {
                    throw new Exception(
                        $"The letter contains too many specifications. A letter supports a total of {_positionCharsMapping.Count} properties"); // TODO CustomException
                }

                Exception exception = null;
                foreach (var compilerStatement in validFunction())
                {
                    try
                    {
                        compilerStatement.Compile(tokens);

                        // reset once succeeded
                        exception = null;
                        break;
                    }
                    catch (Exception e)
                    {
                        // Do not throw the error, first check other options
                        exception = e;
                    }
                }

                // If none did succeed, throw latest error
                if (exception != null)
                    throw exception;

                i++;
            }

            tokens.RemoveFirst(); // Succesfully compiled
        }
    }
}