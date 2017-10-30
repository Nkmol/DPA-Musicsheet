using System;
using System.Collections.Generic;
using DPA_Musicsheets.New.Builder.Generic;
using DPA_Musicsheets.New.Compiler;
using DPA_Musicsheets.New.Compiler.Nodes.Abstractions;
using Models;

namespace DPA_Musicsheets.New.Builder
{
    public class DirectorNote : DirectorNode<NoteBuilder>
    {
        public DirectorNote()
        {
            ValueDirectorMap = new Dictionary<CompilerType, Action<NoteBuilder, Node>>
            {
                {CompilerType.Letter, (b, node) => b.SetLetter(node.Value[0])},
                {CompilerType.Length, (b, node) => b.SetLength(int.Parse(node.Value))},
                {CompilerType.Chroma, (b, node) => b.SetChroma(TrunkNote.TranslateChromaticism(node.Value))},
                {CompilerType.ForceAmplitude, (b, node) => b.SetPitch(int.Parse(node.Value))},
                {CompilerType.Dot, (b, node) => b.SetDot()}
            };
        }
    }
}