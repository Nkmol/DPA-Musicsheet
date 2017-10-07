using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class Note
    {
        public Double Length { get; set; }

        public bool hasPoint { get; set; }

        public void Play()
        {
            throw new System.NotImplementedException();
        }

        public static double CalculatePercentageBar(int previousCurrentTicks, int currentTicks, int division, int beatsPerBar)
        {
            double deltaTicks = currentTicks - previousCurrentTicks;

            if (deltaTicks <= 0)
            {
                return 0;
            }

            double amountOfBeats = deltaTicks / division;
            return (1.0 / beatsPerBar) * amountOfBeats;
        }

        /*
         * 1. choose a "minimal note" that is compatible with your division (e.g. 1/128) and use it as a sort of grid.
         * 2. Align each note to the closest grid line (i.e. to the closest integer multiple of the minimal node)
         * 3. Convert it to standard notation (e.g a quarter note, a dotted eight note, etc...).
         */
        public static string CalculateLength(int beatNote, double percentageOfBar)
        {
            int duration = 0;
            int dots = 0;

            for (int noteLength = 32; noteLength >= 1; noteLength -= 1)
            {
                var absoluteNoteLength = (1.0 / noteLength);

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

        public static string GetNoteName(int previousMidiKey, int midiKey)
        {
            int octave = (midiKey / 12) - 1;
            string name = "";

            switch (midiKey % 12)
            {
                case 0:
                    name = "c";
                    break;
                case 1:
                    name = "cis";
                    break;
                case 2:
                    name = "d";
                    break;
                case 3:
                    name = "dis";
                    break;
                case 4:
                    name = "e";
                    break;
                case 5:
                    name = "f";
                    break;
                case 6:
                    name = "fis";
                    break;
                case 7:
                    name = "g";
                    break;
                case 8:
                    name = "gis";
                    break;
                case 9:
                    name = "a";
                    break;
                case 10:
                    name = "ais";
                    break;
                case 11:
                    name = "b";
                    break;
            }

            int distance = midiKey - previousMidiKey;
            while (distance < -6)
            {
                name += ",";
                distance += 8;
            }

            while (distance > 6)
            {
                name += "'";
                distance -= 8;
            }

            return name;
        }
    }
}