using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.New;
using Helpers;
using Sanford.Multimedia.Midi;
using DPA_Musicsheets.Models;
using System.Text.RegularExpressions;
using Models;
using Models.Domain;
using PSAMControlLibrary;
using Note = PSAMControlLibrary.Note;

namespace DPA_Musicsheets.Managers
{
    public class FileHandlerNew
    {
        private List<string> split;
        // TODO: Make sure "yield" sequence is preserved at build stage (so beyond this). At the moment we simply append them all to one result
        public string OpenFile(string path)
        {
            split = new List<string>();
            var strategy = FileReaderStrategyFactory.Instance.Create(Path.GetExtension(path));
            FileReader fr = new FileReader(path, strategy);

        
            var result = "";
            foreach (var s in fr.ReadLine())
            {
                result += s;
            }
            
            return result;
        }

        public LinkedList<LilypondToken> GetTokensFromLilypond(List<string> notes)
        {
            var tokens = new LinkedList<LilypondToken>();
            var tokenizer = new Tokenizer();
            int level = 0;

            foreach (string s in notes)
            {
                LilypondToken token = tokenizer.Tokenize(s);
                token.index = tokens.Count();

                if (token.Value == "{")
                {
                    level++;
                }

                if (token.Value == "}")
                {
                    level--;
                }

                token.level = level;
                
                if (tokens.Last != null)
                {
                    tokens.Last.Value.NextToken = token;
                    token.PreviousToken = tokens.Last.Value;
                }

                tokens.AddLast(token);
            }

            return tokens;
        }


        public IEnumerable<MusicalSymbol> CreateViewSymbols(Stave stave)
        {
            var symbols = new List<MusicalSymbol>();

            // time
            var times = stave.Time.Split('/');
            symbols.Add(new TimeSignature(TimeSignatureType.Numbers, UInt32.Parse(times[0]), UInt32.Parse(times[1])));

            // clef
            Clef currentClef = null;
            if (stave.Clef == "treble")
                currentClef = new Clef(ClefType.GClef, 2);
            else if (stave.Clef == "bass")
                currentClef = new Clef(ClefType.FClef, 4);
            symbols.Add(currentClef);

            // Notes
            foreach (var note in stave.Notes)
            {
                if (note is TrunkNote trunknote)
                {
                    var viewNote = new Note(trunknote.Letter.ToString().ToUpper(), trunknote.ChromaticismAlter, trunknote.Pitch,
                        (MusicalSymbolDuration) trunknote.Length, NoteStemDirection.Up, NoteTieType.None,
                        new List<NoteBeamType>() {NoteBeamType.Single});
                    if (trunknote.HasPoint) viewNote.NumberOfDots += 1;

                    symbols.Add(viewNote);
                }
                else
                {
                    // The only symbol that is a normal note, is a Rest
                    symbols.Add(new Rest((MusicalSymbolDuration)note.Length));
                }
            }

            return symbols;
        }
    }
}
