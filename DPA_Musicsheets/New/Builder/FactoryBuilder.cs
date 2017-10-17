using DPA_Musicsheets.New.Compiler;
using Helpers.Datatypes;
using Helpers.Datatypes.Factories;
using Models;

namespace DPA_Musicsheets.New.Builder
{
    // Factory for the Model builders
    public class FactoryBuilder : Factory<IBuilder<IObject>>
    {
        public FactoryBuilder()
        {
            AddType(CompilerType.Stave.ToString(), typeof(StaveBuilder));
            AddType(CompilerType.Note.ToString(), typeof(NoteBuilder));
            AddType(CompilerType.RelativeNote.ToString(), typeof(NoteBuilder));
        }
    }
}