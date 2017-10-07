using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helpers.Datatypes;
using Models;

namespace DPA_Musicsheets.New.Builder
{
    public class StaveBuilder : IBuilder<Stave>
    {
        private Stave _staveToBuild;

        public StaveBuilder()
        {
            _staveToBuild = new Stave();
        }

        public Stave Build()
        {
            var stave = _staveToBuild;
            _staveToBuild = new Stave();

            return stave;
        }

        public StaveBuilder SetRelativeNote(Note value)
        {
            _staveToBuild.RelativeNote = value;
            return this;
        }

        public StaveBuilder SetTempo(string value)
        {
            _staveToBuild.Tempo = value;
            return this;
        }

        public StaveBuilder SetTime(string value)
        {
            _staveToBuild.Time = value;
            return this;
        }

        public StaveBuilder SetClef(string value)
        {
            _staveToBuild.Clef = value;
            return this;
        }

        public StaveBuilder AddNote(Note value)
        {
            _staveToBuild.Notes.Add(value);
            return this;
        }
    }
}
