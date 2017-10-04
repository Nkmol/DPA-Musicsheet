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

    }
}
