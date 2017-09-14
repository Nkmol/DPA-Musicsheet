using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sanford.Multimedia.Midi;

namespace Models.Midi
{
    public class FacadeSanfordMidi : IFacadeMidi
    {
        // There are two types of division values: Pulses per quarter note and SMPTE
        public int Division { get; set; }
        public int BeatNote { get; set; }
        public int BeatsPerBar { get; set; }
        public int Bmp { get; set; }
        public double PercentageOfBarReached { get; set; }
        /// <summary>
        /// All the in-order notes in a representation of duration + dot notation. For example: "8.", "16"
        /// </summary>
        public IList<string> Notes { get; set; }

        public FacadeSanfordMidi()
        {
            Notes = new List<string>();
        }

        public void LoadMidi(string path)
        {
            var sequence = new Sequence(path);

            //StringBuilder lilypondContent = new StringBuilder();
            //lilypondContent.AppendLine("\\relative c' {");
            //lilypondContent.AppendLine("\\clef treble");

            Division = sequence.Division;
            int previousMidiKey = 60; // Central C;
            int previousNoteAbsoluteTicks = 0;
            PercentageOfBarReached = 0;
            bool startedNoteIsClosed = true;

            for (int i = 0; i < sequence.Count(); i++)
            {
                Track track = sequence[i];

                foreach (var midiEvent in track.Iterator())
                {
                    IMidiMessage midiMessage = midiEvent.MidiMessage;
                    switch (midiMessage.MessageType)
                    {
                        case MessageType.Meta:
                            var metaMessage = midiMessage as MetaMessage;
                            switch (metaMessage.MetaType)
                            {
                                case MetaType.TimeSignature:
                                    byte[] timeSignatureBytes = metaMessage.GetBytes();
                                    BeatNote = timeSignatureBytes[0];
                                    BeatsPerBar = (int)(1 / Math.Pow(timeSignatureBytes[1], -2));
                                    //lilypondContent.AppendLine($"\\time {_beatNote}/{_beatsPerBar}");
                                    break;
                                case MetaType.Tempo:
                                    byte[] tempoBytes = metaMessage.GetBytes();
                                    int tempo = (tempoBytes[0] & 0xff) << 16 | (tempoBytes[1] & 0xff) << 8 | (tempoBytes[2] & 0xff);
                                    Bmp = 60000000 / tempo;
                                    //lilypondContent.AppendLine($"\\tempo 4={_bpm}");
                                    break;
                                case MetaType.EndOfTrack:
                                    if (previousNoteAbsoluteTicks > 0)
                                    {
                                        // Finish the last notelength.
                                        //double percentageOfBar = 0;
                                        var note = GetNoteLength(previousNoteAbsoluteTicks, midiEvent.AbsoluteTicks, Division, BeatNote, BeatsPerBar, out var percentageOfBar);
                                        Notes.Add(note);
                                        //lilypondContent.Append(GetNoteLength(previousNoteAbsoluteTicks, midiEvent.AbsoluteTicks, division, _beatNote, _beatsPerBar, out percentageOfBar));
                                        //lilypondContent.Append(" ");

                                        PercentageOfBarReached += percentageOfBar;
                                        if (PercentageOfBarReached >= 1)
                                        {
                                            //lilypondContent.AppendLine("|");
                                            percentageOfBar = percentageOfBar - 1;
                                        }
                                    }
                                    break;
                                default: break;
                            }
                            break;
                        case MessageType.Channel:
                            var channelMessage = midiEvent.MidiMessage as ChannelMessage;
                            if (channelMessage.Command == ChannelCommand.NoteOn)
                            {
                                if (channelMessage.Data2 > 0) // Data2 = loudness
                                {
                                    // Append the new note.
                                    //lilypondContent.Append(GetNoteName(previousMidiKey, channelMessage.Data1));

                                    previousMidiKey = channelMessage.Data1;
                                    startedNoteIsClosed = false;
                                }
                                else if (!startedNoteIsClosed)
                                {
                                    // Finish the previous note with the length.
                                    //double percentageOfBar;
                                    var note = GetNoteLength(previousNoteAbsoluteTicks, midiEvent.AbsoluteTicks, Division, BeatNote, BeatsPerBar, out var percentageOfBar);
                                    Notes.Add(note);
                                    //lilypondContent.Append(GetNoteLength(previousNoteAbsoluteTicks, midiEvent.AbsoluteTicks, division, _beatNote, _beatsPerBar, out percentageOfBar));
                                    previousNoteAbsoluteTicks = midiEvent.AbsoluteTicks;
                                    //lilypondContent.Append(" ");

                                    PercentageOfBarReached += percentageOfBar;
                                    if (PercentageOfBarReached >= 1)
                                    {
                                        //lilypondContent.AppendLine("|");
                                        PercentageOfBarReached -= 1;
                                    }
                                    startedNoteIsClosed = true;
                                }
                                else
                                {
                                    //lilypondContent.Append("r");
                                }
                            }
                            break;
                    }
                }
            }

            //lilypondContent.Append("}");
        }

        private string GetNoteLength(int absoluteTicks, int nextNoteAbsoluteTicks, int division, int beatNote, int beatsPerBar, out double percentageOfBar)
        {
            int duration = 0;
            int dots = 0;

            double deltaTicks = nextNoteAbsoluteTicks - absoluteTicks;

            if (deltaTicks <= 0)
            {
                percentageOfBar = 0;
                return String.Empty;
            }

            double percentageOfBeatNote = deltaTicks / division;
            percentageOfBar = (1.0 / beatsPerBar) * percentageOfBeatNote;

            for (int noteLength = 32; noteLength >= 1; noteLength -= 1)
            {
                double absoluteNoteLength = (1.0 / noteLength);

                if (percentageOfBar <= absoluteNoteLength)
                {
                    if (noteLength < 2)
                        noteLength = 2;

                    int subtractDuration;

                    if (noteLength == 32)
                        subtractDuration = 32;
                    else if (noteLength >= 16)
                        subtractDuration = 16;
                    else if (noteLength >= 8)
                        subtractDuration = 8;
                    else if (noteLength >= 4)
                        subtractDuration = 4;
                    else
                        subtractDuration = 2;

                    if (noteLength >= 17)
                        duration = 32;
                    else if (noteLength >= 9)
                        duration = 16;
                    else if (noteLength >= 5)
                        duration = 8;
                    else if (noteLength >= 3)
                        duration = 4;
                    else
                        duration = 2;

                    double currentTime = 0;

                    while (currentTime < (noteLength - subtractDuration))
                    {
                        var addtime = 1 / ((subtractDuration / beatNote) * Math.Pow(2, dots));
                        if (addtime <= 0) break;
                        currentTime += addtime;
                        if (currentTime <= (noteLength - subtractDuration))
                        {
                            dots++;
                        }
                        if (dots >= 4) break;
                    }

                    break;
                }
            }

            return duration + new String('.', dots);
        }
    }
}
