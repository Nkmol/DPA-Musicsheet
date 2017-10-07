using System.Runtime.InteropServices;
using Helpers.Datatypes;
using Models;

namespace DPA_Musicsheets.New.Builder
{
    public class NoteBuilder : IBuilder<TrunkNote>
    {
        private TrunkNote _noteToBuild;

        public NoteBuilder()
        {
            _noteToBuild = new TrunkNote();
        }

        public TrunkNote Build()
        {
            var stave = _noteToBuild;
            _noteToBuild = new TrunkNote();

            return stave;
        }

        public NoteBuilder SetLetter(char value)
        {
            _noteToBuild.Letter = value;
            return this;
        }

        public NoteBuilder SetLength(int value)
        {
            _noteToBuild.Length = value;
            return this;
        }

        public NoteBuilder SetChroma(Chromaticism value)
        {
            _noteToBuild.ChromaticismType = value;
            return this;
        }

        public NoteBuilder SetPitch(int value)
        {
            _noteToBuild.Pitch = value;
            return this;
        }

        public NoteBuilder SetDot()
        {
            _noteToBuild.hasPoint = true;
            return this;
        }
    }
}
