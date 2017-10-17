using System;
using System.Collections.Generic;
using DPA_Musicsheets.New.Builder.Generic;
using DPA_Musicsheets.New.Compiler;
using DPA_Musicsheets.New.Compiler.Nodes.Abstractions;
using Models;

namespace DPA_Musicsheets.New.Builder
{
    public class DirectorStave : DirectorNode<StaveBuilder>
    {
        public DirectorStave()
        {
            ValueDirectorMap = new Dictionary<CompilerType, Action<StaveBuilder, Node>>
            {
                {CompilerType.Clef, (b, node) => b.SetClef(node.Value)},
                {CompilerType.Time, (b, node) => b.SetTime(node.Value)},
                {CompilerType.Tempo, (b, node) => b.SetTempo(node.Value)}
            };

            ComponentDirectorMap = new Dictionary<CompilerType, Action<StaveBuilder, IObject>>
            {
                {CompilerType.RelativeNote, (builder, node) => builder.SetRelativeNote(node as Note)},
                {CompilerType.Note, (builder, node) => builder.AddNote(node as Note)}
            };
        }
    }
}