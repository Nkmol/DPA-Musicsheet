using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DPA_Musicsheets.Managers;
using DPA_Musicsheets.Messages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Win32;
using Models.Domain;

namespace DPA_Musicsheets.ViewModels
{
    public class LilypondViewModel : ViewModelBase
    {
        private readonly FileHandler _fileHandler;

        public int CaretIndex
        {
            get { return _caretIndex; }
            set
            {
                _caretIndex = value;
                RaisePropertyChanged(() => CaretIndex);
            }
        }

        private string _text;
        private string _previousText;
        private string _nextText;

        public string LilypondText
        {
            get => _text;
            set
            {
                if (!_waitingForRender && !_textChangedByLoad)
                    _previousText = _text;
                _text = value;

                RaisePropertyChanged(() => LilypondText);
                LilypondTextChanged?.Invoke(this, new LilypondEventArgs {LilypondText = value});
            }
        }

        public bool TextChanged(string e)
        {
            return e != _previousText;
        }

        public event EventHandler<LilypondEventArgs> LilypondTextChanged;
        public event EventHandler<SequenceSavedArgs> LilypondSaved;

        private readonly bool _textChangedByLoad = false;
        private DateTime _lastChange;
        private static readonly int MILLISECONDS_BEFORE_CHANGE_HANDLED = 1500;
        private bool _waitingForRender;
        private int _caretIndex;

        public LilypondViewModel(FileHandler fileHandler)
        {
            _fileHandler = fileHandler;

            //LilypondTextChanged += (src, e) =>
            //{
            //    _textChangedByLoad = true;
            //    LilypondText = _previousText = e.LilypondText;
            //    _textChangedByLoad = false;
            //};

            _text = "Your lilypond text will appear here.";
        }

        public void InsertAtCursor(string insert)
        {
            InsertAtPosition(insert, CaretIndex);
        }

        public void InsertAtPosition(string insert, int index)
        {
            LilypondText = _text.Insert(index, insert);
        }

        public ICommand TextChangedCommand => new RelayCommand<TextChangedEventArgs>(args =>
        {
            if (!_textChangedByLoad)
            {
                _waitingForRender = true;
                _lastChange = DateTime.Now;
                MessengerInstance.Send(new CurrentStateMessage {State = "Rendering..."});

                Task.Delay(MILLISECONDS_BEFORE_CHANGE_HANDLED).ContinueWith(task =>
                {
                    if ((DateTime.Now - _lastChange).TotalMilliseconds >= MILLISECONDS_BEFORE_CHANGE_HANDLED)
                    {
                        _waitingForRender = false;
                        UndoCommand.RaiseCanExecuteChanged();

                        Task.Factory.StartNew(() =>
                        {
                            SimpleIoc.Default.GetInstance<MainViewModel>().LilypondChange(LilypondText);
                        });
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext()); // Request from main thread.
            }
        });

        public RelayCommand UndoCommand => new RelayCommand(() =>
        {
            _nextText = LilypondText;
            LilypondText = _previousText;
            _previousText = null;
        }, () => _previousText != LilypondText);

        public RelayCommand RedoCommand => new RelayCommand(() =>
        {
            _previousText = LilypondText;
            LilypondText = _nextText;
            _nextText = null;
            RedoCommand.RaiseCanExecuteChanged();
        }, () => _nextText != LilypondText);

        public ICommand SaveAsCommand => new RelayCommand(() =>
        {
            var saveFileDialog = new SaveFileDialog {Filter = "Midi|*.mid|Lilypond|*.ly|PDF|*.pdf"};
            var result = true;
            if (saveFileDialog.ShowDialog() == true)
            {
                var extension = Path.GetExtension(saveFileDialog.FileName);
                if (extension.EndsWith(".mid"))
                {
                    var objs = _fileHandler.ProcessLillyPond(_text);
                    var symbols = SimpleIoc.Default.GetInstance<MainViewModel>().CreateViewSymbols((Stave) objs[0])
                        .ToList();

                    _fileHandler.SaveToMidi(saveFileDialog.FileName, symbols);
                }
                else if (extension.EndsWith(".ly"))
                {
                    _fileHandler.SaveToLilypond(saveFileDialog.FileName, _text);
                }
                else if (extension.EndsWith(".pdf"))
                {
                    _fileHandler.SaveToPDF(saveFileDialog.FileName, _text);
                }
                else
                {
                    MessageBox.Show($"Extension {extension} is not supported.");
                    result = false;
                }
            }
            else
            {
                result = false;
            }

            LilypondSaved?.Invoke(this, new SequenceSavedArgs {Result = result});
        });
    }
}