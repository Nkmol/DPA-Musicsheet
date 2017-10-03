using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DPA_Musicsheets.Models;

namespace DPA_Musicsheets.New.Compiler.Statements
{
    public class CompilerNote : ICompilerStatement
    {
        private static readonly char[] ValidLetters = {'a', 'b', 'c', 'd', 'e', 'f', 'g'};
        private static readonly int[] ValidNumbers = {1, 2, 4, 8, 16};
        private static readonly string[] PrefixChroma = {"es", "is"};

        private static readonly char Dot = '.';
        private static readonly char[] ApastrofComma = {',', '\''};

        private readonly Dictionary<int, Func<string, int>> _positionCharsMapping =
            new Dictionary<int, Func<string, int>>();

        public void Compile(LilypondToken currentTokens)
        {
            // Setup mapping of possible characters at place
            // TODO: Shortcut validation => Stop when first validation is true
            // Possible at place 1
            _positionCharsMapping.Add(0, 
                s => IsValidLetter(s[0]));
            // Possible at place 2
            _positionCharsMapping.Add(1,
                s => IsValidForceAmplitude(s[0]) + IsValidNumber(s) + IsValidChroma(s));
            // Possible at place 3
            _positionCharsMapping.Add(2, 
                s => IsValidNumber(s) + IsValidDot(s[0]));
            // Possible at place 4
            _positionCharsMapping.Add(3, 
                s => IsValidDot(s[0]));

            // Work the Note chararistics away
            // When a chararistic is gone, it means it has been succesfully compiled
            var value = currentTokens.Value;
            var i = 0;
            while (!string.IsNullOrEmpty(value))
            {
                _positionCharsMapping.TryGetValue(i, out var validFunction);
                if (validFunction == null)
                    throw new Exception();

                var result = validFunction(value);
                if (result > 0)
                    value = value.Remove(0, result);
                else
                    throw new Exception();

                i++;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="letter"></param>
        /// <returns>Amount of parsed chars</returns>
        public int IsValidLetter(char letter)
        {
            return ValidLetters.Contains(letter) ? 1 : 0;
        }

        public int IsValidChroma(string chroma)
        {
            return chroma.Length >= 2 && PrefixChroma.Contains(chroma.Substring(0, 2)) ? 2 : 0;
        }

        public int IsValidNumber(string number)
        {
            var numberResult = Regex.Match(number, "^([0-9]+)").Value;
            return int.TryParse(numberResult, out var result) && ValidNumbers.Contains(result)
                ? result.ToString().Length
                : 0;
        }

        public int IsValidForceAmplitude(char force)
        {
            return ApastrofComma.Contains(force) ? 1 : 0;
        }

        public int IsValidDot(char dot)
        {
            return dot == Dot ? 1 : 0;
        }
    }
}