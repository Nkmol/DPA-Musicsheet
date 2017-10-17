using Models;

namespace DPA_Musicsheets.New.Builder
{
    public class NoteBuilder : AbstractBuilder<TrunkNote>
    {
        public NoteBuilder SetLetter(char value)
        {
            ToBuild.Letter = value;
            return this;
        }

        public NoteBuilder SetLength(int value)
        {
            ToBuild.Length = value;
            return this;
        }

        public NoteBuilder SetChroma(Chromaticism value)
        {
            ToBuild.ChromaticismType = value;
            return this;
        }

        public NoteBuilder SetPitch(int value)
        {
            ToBuild.Pitch = value;
            return this;
        }

        public NoteBuilder SetDot()
        {
            ToBuild.hasPoint = true;
            return this;
        }
    }
}