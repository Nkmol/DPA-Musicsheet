using Models;
using Models.Domain;

namespace DPA_Musicsheets.New.Builder
{
    public class StaveBuilder : AbstractBuilder<Stave>
    {
        public StaveBuilder SetRelativeNote(Note value)
        {
            ToBuild.RelativeNote = value;
            return this;
        }

        public StaveBuilder SetTempo(string value)
        {
            ToBuild.Tempo = value;
            return this;
        }

        public StaveBuilder SetTime(string value)
        {
            ToBuild.Time = value;
            return this;
        }

        public StaveBuilder SetClef(string value)
        {
            ToBuild.Clef = value;
            return this;
        }

        public StaveBuilder AddNote(Note value)
        {
            ToBuild.Notes.Add(value);
            return this;
        }
    }
}