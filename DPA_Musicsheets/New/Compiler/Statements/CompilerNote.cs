using System;
using System.Collections.Generic;
using System.Linq;
using DPA_Musicsheets.Models;
using DPA_Musicsheets.New.Compiler.Nodes;
using DPA_Musicsheets.New.Compiler.Nodes.Abstractions;
using Models;

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
                        $"The letter contains too many specifications. A letter supports a total of {PositionCharsMapping.Count} properties"); 

                Exception exception = null;
                foreach (var compilerStatement in validFunction())
                {
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
                }

                // If none did succeed, throw latest error
                if (exception != null)
                    throw exception;

                i++;
            }

            // TODO
            // Force relative pitch when token is not present
            if (note.Properties.All(x => x.Context != CompilerType.ForceAmplitude))
            {
                note.Properties.Add(new Node() { Context = CompilerType.ForceAmplitude, Value = Compiler.Octave.ToString() });
            }

            // Contenxtual change the pitch so it is relatively correct
            var alterPitch = AlterPitch(firstToken.Value[0], Compiler.PreviousNote);
            if (alterPitch != 0)
            {
                var property = note.Properties.Select(x => (Node) x)
                    .FirstOrDefault(x => x.Context == CompilerType.ForceAmplitude);

                var result = int.TryParse(property?.Value
                    , out var pitch);

                if (result && property != null)
                {
                    Compiler.Octave = pitch + alterPitch;
                    var newVal = Compiler.Octave.ToString();
                    property.Value = newVal;
                }
            }
            Compiler.PreviousNote = firstToken.Value[0];

            tokens.RemoveFirst(); // Succesfully compiled

            note.Context = CompilerType.Note;
            return note;
        }

        public int AlterPitch(char currentNote, char previousNote)
        {
            var result = 0;

            int distanceWithPreviousNote =
                TrunkNote.GetNoteOrderIndex(currentNote) - TrunkNote.GetNoteOrderIndex(previousNote);
            if (distanceWithPreviousNote > 3) // Shorter path possible the other way around
            {
                distanceWithPreviousNote -= 7; // The number of notes in an octave
            }
            else if (distanceWithPreviousNote < -3)
            {
                distanceWithPreviousNote += 7; // The number of notes in an octave
            }

            if (distanceWithPreviousNote + TrunkNote.GetNoteOrderIndex(previousNote) >= 7)
            {
                result++;
            }
            else if (distanceWithPreviousNote + TrunkNote.GetNoteOrderIndex(previousNote) < 0)
            {
                result--;
            }

            return result;
        }
    }
}