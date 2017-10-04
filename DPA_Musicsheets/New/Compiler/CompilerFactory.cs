using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datatypes;
using DPA_Musicsheets.Models;
using DPA_Musicsheets.New.Compiler.Statements;
using Helpers.Datatypes;

namespace DPA_Musicsheets.New.Compiler
{
    public class CompilerFactory : Singleton<CompilerFactory>, IFactory<ICompilerStatement>
    {
        private readonly IFactory<ICompilerStatement> _factory = new Factory<ICompilerStatement>();

        public CompilerFactory()
        {
            AddType(LilypondTokenKind.Note.ToString(), typeof(CompilerNote));
            AddType(LilypondTokenKind.Staff.ToString(), typeof(CompilerStave));
            AddType(LilypondTokenKind.Clef.ToString(), typeof(CompilerClef));
            AddType(LilypondTokenKind.Time.ToString(), typeof(CompilerTime));
            AddType(LilypondTokenKind.Tempo.ToString(), typeof(CompilerTempo));
        }

        public void AddType(string typenaming, Type type)
        {
            _factory.AddType(typenaming, type);
        }

        public ICompilerStatement Create(string type)
        {
            return _factory.Create(type);
        }
    }
}
