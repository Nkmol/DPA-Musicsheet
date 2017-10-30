using DPA_Musicsheets.Managers;
using DPA_Musicsheets.Messages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using PSAMWPFControlLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Ioc;
using Models;
using Models.Domain;
using PSAMControlLibrary;
using Note = PSAMControlLibrary.Note;

namespace DPA_Musicsheets.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private string _fileName;
        public string FileName
        {
            get
            {
                return _fileName;
            }
            set
            {
                _fileName = value;
                RaisePropertyChanged(() => FileName);
            }
        }

        private string _currentState;
        public string CurrentState
        {
            get { return _currentState; }
            set { _currentState = value; RaisePropertyChanged(() => CurrentState); }
        }

        private FileHandler _fileHandler;

        public MainViewModel(FileHandler fileHandler)
        {
            _fileHandler = fileHandler;
            FileName = @"Files/Alle-eendjes-zwemmen-in-het-water.mid";

            MessengerInstance.Register<CurrentStateMessage>(this, (message) => CurrentState = message.State);
        }

        public ICommand OpenFileCommand => new RelayCommand(() =>
        {
            OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = "Midi or LilyPond files (*.mid *.ly)|*.mid;*.ly" };
            if (openFileDialog.ShowDialog() == true)
            {
                FileName = openFileDialog.FileName;
            }
        });
        public ICommand LoadCommand => new RelayCommand(() =>
        {
            //_fileHandler.OpenFile(FileName);
            var lilypond = _fileHandler.LoadFile(FileName);
            LilypondChange(lilypond);
        });

        public void LilypondChange(string newText)
        {
            SimpleIoc.Default.GetInstance<LilypondViewModel>().LilypondText = newText;

            var processedModels = _fileHandler.ProcessLillyPond(newText);

            // Generate staff view
            var symbols = CreateViewSymbols((Stave)processedModels[0]).ToList();
            SimpleIoc.Default.GetInstance<StaffsViewModel>().Staffs.Clear();
            foreach (var symbol in symbols)
            {
                SimpleIoc.Default.GetInstance<StaffsViewModel>().Staffs.Add(symbol);
            }

            // Init sequence
            var sequence = _fileHandler.GetSequenceFromWPFStaffs(symbols.ToList());
            SimpleIoc.Default.GetInstance<MidiPlayerViewModel>().Sequencer.Sequence = sequence;
        }
        
        public ICommand OnLostFocusCommand => new RelayCommand(() =>
        {
            Console.WriteLine("Maingrid Lost focus");
        });

        public ICommand OnKeyDownCommand => new RelayCommand<KeyEventArgs>((e) =>
        {
            Console.WriteLine($"Key down: {e.Key}");
        });

        public ICommand OnKeyUpCommand => new RelayCommand(() =>
        {
            Console.WriteLine("Key Up");
        });

        public ICommand OnWindowClosingCommand => new RelayCommand(() =>
        {
            ViewModelLocator.Cleanup();
        });

        // Create the PSAM controller view
        public IEnumerable<MusicalSymbol> CreateViewSymbols(Stave stave)
        {
            var symbols = new List<MusicalSymbol>();

            // clef
            Clef currentClef = null;
            if (stave.Clef == "treble")
                currentClef = new Clef(ClefType.GClef, 2);
            else if (stave.Clef == "bass")
                currentClef = new Clef(ClefType.FClef, 4);
            symbols.Add(currentClef);

            // time
            var times = stave.Time.Split('/');
            symbols.Add(new TimeSignature(TimeSignatureType.Numbers, UInt32.Parse(times[0]), UInt32.Parse(times[1])));

            // Notes
            foreach (var note in stave.Notes)
            {
                if (note is TrunkNote trunknote)
                {
                    //Create the actual note
                    var viewNote = new Note(trunknote.Letter.ToString().ToUpper(), (int)trunknote.ChromaticismType, trunknote.Pitch,
                        (MusicalSymbolDuration)trunknote.Length, NoteStemDirection.Up, NoteTieType.None,
                        new List<NoteBeamType>() { NoteBeamType.Single });
                    if (trunknote.HasPoint) viewNote.NumberOfDots += 1;

                    symbols.Add(viewNote);

                }
                else
                {
                    // Sepcial notes
                    switch (note.Special)
                    {
                        case SpecialType.Bar:
                            symbols.Add(new Barline());
                            break;
                        case SpecialType.Rest:
                            symbols.Add(new Rest((MusicalSymbolDuration)note.Length));
                            break;
                        default:
                            break;
                    }
                }
            }

            //symbols.Add(new Note("D", 0, 5,
            //    (MusicalSymbolDuration)8, NoteStemDirection.Up, NoteTieType.None,
            //    new List<NoteBeamType>() { NoteBeamType.Single }));
            //symbols.Add(new Note("F", 1, 5,
            //    (MusicalSymbolDuration)8, NoteStemDirection.Up, NoteTieType.None,
            //    new List<NoteBeamType>() { NoteBeamType.Single }));
            //symbols.Add(new Note("G", 0, 5,
            //    (MusicalSymbolDuration)2, NoteStemDirection.Up, NoteTieType.None,
            //    new List<NoteBeamType>() { NoteBeamType.Single }));
            return symbols;
        }
    }
}
