using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helpers.FileReaders;
using Models;
using Models.Midi;
using Sanford.Multimedia.Midi;


namespace DPA_Musicsheets.New
{
    public class MidiReadStrategy : IFileReaderStrategy
    {
        public IEnumerable<string> ReadFile(string path)
        {
            foreach (var s in TransformSequence(path))
            {
                yield return s;
            }
        }

        private IEnumerable<string> TransformSequence(string path)
        {
            var facade = new FacadeSanfordMidi();
            yield return "\\relative c' {" ;
            yield return "\\clef treble" ;

            foreach (var s in facade.LoadMidi(path))
            {
                yield return s;
            }
        }
    }
}
