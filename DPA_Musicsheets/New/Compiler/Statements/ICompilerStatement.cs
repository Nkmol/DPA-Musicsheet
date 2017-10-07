using System.Collections.Generic;
using DPA_Musicsheets.Models;
using DPA_Musicsheets.New.Compiler.Nodes;
using DPA_Musicsheets.New.Compiler.Nodes.Abstractions;

namespace DPA_Musicsheets.New.Compiler.Statements
{
    public interface ICompilerStatement
    {
        INode Compile(LinkedList<LilypondToken> tokens);
    }
}