using DPA_Musicsheets.Managers;
using DPA_Musicsheets.Messages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using PSAMWPFControlLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DPA_Musicsheets.ViewModels.CoR;
using DPA_Musicsheets.ViewModels.State;
using GalaSoft.MvvmLight.Ioc;
using Models;
using Models.Domain;
using PSAMControlLibrary;
using Key = System.Windows.Input.Key;
using Note = PSAMControlLibrary.Note;

namespace DPA_Musicsheets.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private LilypondViewModel _lilypondVM { get; }
        private ApplicationState _state;

        public ApplicationState State
        {
            get => _state;
            set
            {
                value.Handle(this);
                _state = value;
            }
        }


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

        public MainViewModel(FileHandler fileHandler, LilypondViewModel lilypondVM)
        {
            _lilypondVM = lilypondVM;
            SetupKeyHandler();
            // Inital state
            State = new StateSaved();
            _fileHandler = fileHandler;
            FileName = @"Files/Alle-eendjes-zwemmen-in-het-water.mid";

            MessengerInstance.Register<CurrentStateMessage>(this, (message) => CurrentState = message.State);
        }

        #region KeyHandlers

        private Dictionary<Command, KeyHandler> KeyMapping = new Dictionary<Command, KeyHandler>();
        private HashSet<Key> CurrentKeysPushed = new HashSet<Key>();

        public void SetupKeyHandler()
        {
            KeyMapping.Add(new Command(() => _fileHandler.SaveToLilypond(FileName, _lilypondVM.LilypondText)), 
                KeyHandlerModifierKeys.Creator(CurrentKeysPushed, Key.LeftCtrl, Key.S));

            KeyMapping.Add(new Command(() => _fileHandler.SaveToPDF(FileName, _lilypondVM.LilypondText)),
                KeyHandlerModifierKeys.Creator(CurrentKeysPushed, Key.LeftCtrl, Key.S, Key.P));

            KeyMapping.Add(new Command(() => OpenFileCommand.Execute(null)),
                KeyHandlerModifierKeys.Creator(CurrentKeysPushed, Key.LeftCtrl, Key.O));

            KeyMapping.Add(new Command(() => _lilypondVM.InsertAtCursor("\\clef treble")),
                KeyHandlerModifierKeys.Creator(CurrentKeysPushed, Key.LeftAlt, Key.C));

            KeyMapping.Add(new Command(() => _lilypondVM.InsertAtCursor("\\tempo 4=120")),
                KeyHandlerModifierKeys.Creator(CurrentKeysPushed, Key.LeftAlt, Key.S));

            KeyMapping.Add(new Command(() => _lilypondVM.InsertAtCursor("\\time 6/8")),
                KeyHandlerModifierKeys.Creator(CurrentKeysPushed, Key.LeftAlt, Key.T, Key.D6));

            KeyMapping.Add(new Command(() => _lilypondVM.InsertAtCursor("\\time 3/4")),
                KeyHandlerModifierKeys.Creator(CurrentKeysPushed, Key.LeftAlt, Key.T, Key.D3));

            KeyMapping.Add(new Command(() => _lilypondVM.InsertAtCursor("\\time 4/4")),
                KeyHandlerModifierKeys.Creator(CurrentKeysPushed, Key.LeftAlt, Key.T, Key.D4));

            KeyMapping.Add(new Command(() => _lilypondVM.InsertAtCursor("\\time 4/4")),
                KeyHandlerModifierKeys.Creator(CurrentKeysPushed, Key.LeftAlt, Key.T));
        }

        private async Task DelayedWork()
        {
            await Task.Delay(50);
            foreach (var kv in KeyMapping)
            {
                if (kv.Value.HandleRequest())
                {
                    kv.Key.Execute();
                    break;
                }
            }
        }

        public ICommand OnKeyDownCommand => new RelayCommand<KeyEventArgs>(e =>
        {
            if (e.Key == Key.System) e.Handled = true;
            var key = e.Key == Key.System ? e.SystemKey : e.Key;
            CurrentKeysPushed.Add(key);

            Task ignoredAwaitableResult = this.DelayedWork();
        });

        public ICommand OnKeyUpCommand => new RelayCommand<KeyEventArgs>(e =>
        {
            var key = (e.Key == Key.System ? e.SystemKey : e.Key);
            CurrentKeysPushed.Remove(key);
        });
        #endregion KeyHandlers

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
            var lilypond = _fileHandler.LoadFile(FileName);
            LilypondChange(lilypond);
        });

        public void LilypondChange(string newText)
        {
            SimpleIoc.Default.GetInstance<LilypondViewModel>().LilypondText = newText;

            try
            {
                var processedModels = _fileHandler.ProcessLillyPond(newText);

                // Generate staff view
                var symbols = CreateViewSymbols((Stave) processedModels[0]).ToList();
                SimpleIoc.Default.GetInstance<StaffsViewModel>().Staffs.Clear();
                foreach (var symbol in symbols)
                {
                    SimpleIoc.Default.GetInstance<StaffsViewModel>().Staffs.Add(symbol);
                }

                // Init sequence
                var sequence = _fileHandler.GetSequenceFromWPFStaffs(symbols.ToList());
                SimpleIoc.Default.GetInstance<MidiPlayerViewModel>().Sequencer.Sequence = sequence;

                CurrentState = "Rendering completed";
            }
            catch (Exception e)
            {
                CurrentState = e.Message;
            }
        }

        public ICommand OnLostFocusCommand => new RelayCommand(() =>
        {
            Console.WriteLine("Maingrid Lost focus");
        });

        public ICommand OnWindowClosingCommand => new RelayCommand<CancelEventArgs>(e =>
        {
            if (State is StateUnsavedChanges)
            {
                MessageBoxResult result = MessageBox.Show("You have unsaved changes. Are you sure you want to quit?",
                    "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    ViewModelLocator.Cleanup();
                    Environment.Exit(0);
                }
                else
                {
                    e.Cancel = true;
                }
            }
            else
            {
                Environment.Exit(0);
            }
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
            return symbols;
        }
    }
}
