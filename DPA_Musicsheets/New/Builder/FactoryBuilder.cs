using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datatypes;
using DPA_Musicsheets.New.Compiler;
using DPA_Musicsheets.New.Compiler.Nodes;
using DPA_Musicsheets.New.Compiler.Nodes.Abstractions;
using Helpers.Datatypes;
using Models;

namespace DPA_Musicsheets.New.Builder
{
    // Factory for the Model builders
    // Acts as interpreter of Nodes
    public class DirectorBuilders : Factory<IBuilder<object>>
    {

        private static readonly Dictionary<CompilerType, Action<StaveBuilder, Node>> StaveDirector = new Dictionary<CompilerType, Action<StaveBuilder, Node>>
        {
            {CompilerType.Clef, (b, node) => b.SetClef(node.Value) },
            {CompilerType.Time, (b, node) => b.SetTime(node.Value)},
            {CompilerType.Tempo, (b, node) => b.SetTempo(node.Value) },
        };

        private static readonly Dictionary<CompilerType, Action<NoteBuilder, Node>> NoteDirector = new Dictionary<CompilerType, Action<NoteBuilder, Node>>
        {
            {CompilerType.Letter, (b, node) => b.SetLetter(node.Value[0]) },
            {CompilerType.Length, (b, node) => b.SetLength(int.Parse(node.Value))},
            {CompilerType.Chroma, (b, node) => b.SetChroma(TrunkNote.TranslateChromaticism(node.Value)) },
            {CompilerType.Dot, (b, node) => b.SetDot() },
        };

        public DirectorBuilders()
        {
            AddType(CompilerType.Stave.ToString(), typeof(StaveBuilder));
            AddType(CompilerType.Note.ToString(), typeof(NoteBuilder));
            AddType(CompilerType.RelativeNote.ToString(), typeof(NoteBuilder));
        }

        public static bool Direct(StaveBuilder to, Node input)
        {
            StaveDirector.TryGetValue(input.Context, out var result);
            result?.Invoke(to, input);

            return result != null;
        }

        public static bool Direct(NoteBuilder to, Node input)
        {
            NoteDirector.TryGetValue(input.Context, out var result);
            result?.Invoke(to, input);

            return result != null;
        }

        public static bool DirectComponent(IBuilder<object> to, TrunkNote input, CompilerType context)
        {
            if (to is StaveBuilder)
            {
                var buildCasted = to as StaveBuilder;
                if (context == CompilerType.RelativeNote) buildCasted.SetRelativeNote(input);
                else if (context == CompilerType.Note) buildCasted.AddNote(input);
            }

            return true;
        }
    }
}
