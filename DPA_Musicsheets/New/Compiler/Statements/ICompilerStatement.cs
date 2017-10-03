using DPA_Musicsheets.Models;

namespace DPA_Musicsheets.New.Compiler.Statements
{
    public interface ICompilerStatement
    {
        void Compile(LilypondToken currentTokens);
    }
}
