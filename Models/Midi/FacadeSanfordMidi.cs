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
        public FacadeSanfordMidi()
        {
        }

        public IEnumerable<string> LoadMidi(string path)
        {
            var sequence = new Sequence(path);

            //StringBuilder lilypondContent = new StringBuilder();
            //lilypondContent.AppendLine("\\relative c' {");
            //lilypondContent.AppendLine("\\clef treble");

            var division = sequence.Division;
            int previousMidiKey = 60; // Central C;
            int previousNoteAbsoluteTicks = 0;
            double percentageOfBarReached = 0;
            bool startedNoteIsClosed = true;
            int bmp = 0;
            int beatNote = 0;
            int beatsPerBar = 0;

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
                                    beatNote = timeSignatureBytes[0];
                                    beatsPerBar = (int)(1 / Math.Pow(timeSignatureBytes[1], -2));
                                    //lilypondContent.AppendLine($"\\time {_beatNote}/{_beatsPerBar}");
                                    yield return $"\\time {beatNote}/{beatsPerBar} ";
                                    break;
                                case MetaType.Tempo:
                                    byte[] tempoBytes = metaMessage.GetBytes();
                                    int tempo = (tempoBytes[0] & 0xff) << 16 | (tempoBytes[1] & 0xff) << 8 | (tempoBytes[2] & 0xff);
                                    bmp = 60000000 / tempo;
                                    //lilypondContent.AppendLine($"\\tempo 4={_bpm}");
                                    yield return $"\\tempo 4={bmp} ";

                                    break;
                                case MetaType.EndOfTrack:
                                    if (previousNoteAbsoluteTicks > 0)
                                    {
                                        // Finish the last notelength.
                                        //double percentageOfBar = 0;
                                        var percentageOfBar = Note.CalculatePercentageBar(previousNoteAbsoluteTicks, midiEvent.AbsoluteTicks, division, beatsPerBar);
                                        var note = Note.CalculateLength(beatNote, percentageOfBar);
                                        yield return note + " ";

                                        //lilypondContent.Append(GetNoteLength(previousNoteAbsoluteTicks, midiEvent.AbsoluteTicks, division, _beatNote, _beatsPerBar, out percentageOfBar));
                                        //lilypondContent.Append(" ");

                                        percentageOfBarReached += percentageOfBar;
                                        if (percentageOfBarReached >= 1)
                                        {
                                            //lilypondContent.AppendLine("|");
                                            yield return "| ";
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
                                    yield return Note.GetNoteName(previousMidiKey, channelMessage.Data1);

                                    previousMidiKey = channelMessage.Data1;
                                    startedNoteIsClosed = false;
                                }
                                else if (!startedNoteIsClosed)
                                {
                                    // Finish the previous note with the length.
                                    //double percentageOfBar;
                                    var percentageOfBar = Note.CalculatePercentageBar(previousNoteAbsoluteTicks, midiEvent.AbsoluteTicks, division, beatsPerBar);
                                    var note = Note.CalculateLength(beatNote, percentageOfBar);
                                    yield return note + " ";

                                    //lilypondContent.Append(GetNoteLength(previousNoteAbsoluteTicks, midiEvent.AbsoluteTicks, division, _beatNote, _beatsPerBar, out percentageOfBar));
                                    previousNoteAbsoluteTicks = midiEvent.AbsoluteTicks;
                                    //lilypondContent.Append(" ");

                                    percentageOfBarReached += percentageOfBar;
                                    if (percentageOfBarReached >= 1)
                                    {
                                        yield return "| ";
                                        //lilypondContent.AppendLine("|");
                                        percentageOfBarReached -= 1;
                                    }
                                    startedNoteIsClosed = true;
                                }
                                else
                                {
                                    yield return "r ";
                                    //lilypondContent.Append("r");
                                }
                            }
                            break;
                    }
                }
            }

            yield return "}";
            //lilypondContent.Append("}");
        }
    }
}
