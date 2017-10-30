
using DPA_Musicsheets.Models;
using PSAMControlLibrary;
using PSAMWPFControlLibrary;
using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DPA_Musicsheets.New;
using DPA_Musicsheets.New.Builder;
using DPA_Musicsheets.New.Compiler;
using DPA_Musicsheets.New.Compiler.Nodes;
using DPA_Musicsheets.New.Compiler.Nodes.Abstractions;
using DPA_Musicsheets.New.Compiler.Statements;
using Helpers;
using Helpers.Datatypes;
using Models;
using Models.Domain;
using Note = PSAMControlLibrary.Note;

namespace DPA_Musicsheets.Managers
{
    public class FileHandler
    {
        
        //public List<MusicalSymbol> WPFStaffs { get; set; } = new List<MusicalSymbol>();

        public Sequence MidiSequence { get; set; }

        //public event EventHandler<WPFStaffsEventArgs> WPFStaffsChanged;
        //public event EventHandler<MidiSequenceEventArgs> MidiSequenceChanged;

        private int _beatNote = 4;    // De waarde van een beatnote.
        private int _bpm = 120;       // Aantal beatnotes per minute.
        private int _beatsPerBar;     // Aantal beatnotes per maat.

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns>Lilypond stingified music file</returns>
        // TODO: Make sure "yield" sequence is preserved at build stage (so beyond this). At the moment we simply append them all to one result
        public string LoadFile(string path)
        {
            var strategy = FileReaderStrategyFactory.Instance.Create(Path.GetExtension(path));
            FileReader fr = new FileReader(path, strategy);


            var result = "";
            foreach (var s in fr.ReadLine())
            {
                result += s;
            }

            return result;
        }

        public List<IObject> ProcessLillyPond(string content)
        {
            var split = content.ToLower().Split(' ').ToList(); ;
            LinkedList<LilypondToken> tokens = GetTokensFromLilypond(split);

            var nodes = Compiler.Run(tokens).ToList();
            // if tokens.count > 0 == not all tokens are processed

            return MusicSheetCreator.CreateComponents(nodes);

            //WPFStaffs.Clear();
            //string message = "";
            ////WPFStaffs.AddRange(GetStaffsFromTokens(tokens, out message));
            //WPFStaffs.AddRange(new FileHandlerNew().CreateViewSymbols(components[0] as Stave));
            //WPFStaffsChanged?.Invoke(this, new WPFStaffsEventArgs() { Symbols = WPFStaffs, Message = message });

            //MidiSequence = GetSequenceFromWPFStaffs();
            //MidiSequenceChanged?.Invoke(this, new MidiSequenceEventArgs() { MidiSequence = MidiSequence });
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

        #region Saving to files
        internal void SaveToMidi(string fileName, List<MusicalSymbol> symbols)
        {
            Sequence sequence = GetSequenceFromWPFStaffs(symbols);

            sequence.Save(fileName);
        }

        public Sequence GetSequenceFromWPFStaffs(List<MusicalSymbol> symbols)
        {
            List<string> notesOrderWithCrosses = new List<string>() { "c", "cis", "d", "dis", "e", "f", "fis", "g", "gis", "a", "ais", "b" };
            int absoluteTicks = 0;

            Sequence sequence = new Sequence();

            Track metaTrack = new Track();
            sequence.Add(metaTrack);

            // Calculate tempo
            int speed = (60000000 / _bpm);
            byte[] tempo = new byte[3];
            tempo[0] = (byte)((speed >> 16) & 0xff);
            tempo[1] = (byte)((speed >> 8) & 0xff);
            tempo[2] = (byte)(speed & 0xff);
            metaTrack.Insert(0 /* Insert at 0 ticks*/, new MetaMessage(MetaType.Tempo, tempo));

            Track notesTrack = new Track();
            sequence.Add(notesTrack);

            foreach (MusicalSymbol musicalSymbol in symbols)
            {
                switch (musicalSymbol.Type)
                {
                    case MusicalSymbolType.Note:
                        Note note = musicalSymbol as Note;

                        // Calculate duration
                        double absoluteLength = 1.0 / (double)note.Duration;
                        absoluteLength += (absoluteLength / 2.0) * note.NumberOfDots;

                        double relationToQuartNote = _beatNote / 4.0;
                        double percentageOfBeatNote = (1.0 / _beatNote) / absoluteLength;
                        double deltaTicks = (sequence.Division / relationToQuartNote) / percentageOfBeatNote;

                        // Calculate height
                        int noteHeight = notesOrderWithCrosses.IndexOf(note.Step.ToLower()) + ((note.Octave + 1) * 12);
                        noteHeight += note.Alter;
                        notesTrack.Insert(absoluteTicks, new ChannelMessage(ChannelCommand.NoteOn, 1, noteHeight, 90)); // Data2 = volume

                        absoluteTicks += (int)deltaTicks;
                        notesTrack.Insert(absoluteTicks, new ChannelMessage(ChannelCommand.NoteOn, 1, noteHeight, 0)); // Data2 = volume

                        break;
                    case MusicalSymbolType.TimeSignature:
                        byte[] timeSignature = new byte[4];
                        timeSignature[0] = (byte)_beatsPerBar;
                        timeSignature[1] = (byte)(Math.Log(_beatNote) / Math.Log(2));
                        metaTrack.Insert(absoluteTicks, new MetaMessage(MetaType.TimeSignature, timeSignature));
                        break;
                    default:
                        break;
                }
            }

            notesTrack.Insert(absoluteTicks, MetaMessage.EndOfTrackMessage);
            metaTrack.Insert(absoluteTicks, MetaMessage.EndOfTrackMessage);
            return sequence;
        }

        internal void SaveToPDF(string fileName)
        {
            string withoutExtension = Path.GetFileNameWithoutExtension(fileName);
            string tmpFileName = $"{fileName}-tmp.ly";
            SaveToLilypond(tmpFileName);

            string lilypondLocation = @"C:\Program Files (x86)\LilyPond\usr\bin\lilypond.exe";
            string sourceFolder = Path.GetDirectoryName(tmpFileName);
            string sourceFileName = Path.GetFileNameWithoutExtension(tmpFileName);
            string targetFolder = Path.GetDirectoryName(fileName);
            string targetFileName = Path.GetFileNameWithoutExtension(fileName);

            var process = new Process
            {
                StartInfo =
                {
                    WorkingDirectory = sourceFolder,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    Arguments = String.Format("--pdf \"{0}\\{1}.ly\"", sourceFolder, sourceFileName),
                    FileName = lilypondLocation
                }
            };

            process.Start();
            while (!process.HasExited) { /* Wait for exit */
                }
                if (sourceFolder != targetFolder || sourceFileName != targetFileName)
            {
                File.Move(sourceFolder + "\\" + sourceFileName + ".pdf", targetFolder + "\\" + targetFileName + ".pdf");
                File.Delete(tmpFileName);
            }
        }

        internal void SaveToLilypond(string fileName)
        {
            using (StreamWriter outputFile = new StreamWriter(fileName))
            {
                outputFile.Write("");
                outputFile.Close();
            }
        }
        #endregion Saving to files
    }
}
