using System;
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
            AddType(LilypondTokenKind.Bar.ToString(), typeof(CompilerBar));
        }

        public void AddType(string typenaming, Type type)
        {
            _factory.AddType(typenaming, type);
        }

        public ICompilerStatement Create(string type)
        {
            var statement = _factory.Create(type);

            if (statement == null)
            {
                //throw new Exception($"{type} is not a valid Compiler type");
            }

            return _factory.Create(type);
        }
    }
}