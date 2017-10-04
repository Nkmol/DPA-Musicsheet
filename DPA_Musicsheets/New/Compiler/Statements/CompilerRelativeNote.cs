﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.Models;

namespace DPA_Musicsheets.New.Compiler.Statements
{
    public class CompilerRelativeNote : ICompilerStatement
    {
        public void Compile(LinkedList<LilypondToken> tokens)
        {
            var statements = new ICompilerStatement[] {new CompilerLetter(), new CompilerForceAmplitude()};
            foreach (var compilerStatement in statements)
            {
                compilerStatement.Compile(tokens);
            }

            if (tokens.First.Value.ValueToCompile != String.Empty)
            {
                throw new Exception($"Relative note does not support the property of {tokens.First.Value.Value}");
            }

            tokens.RemoveFirst(); // compiled succesful
        }
    }
}
