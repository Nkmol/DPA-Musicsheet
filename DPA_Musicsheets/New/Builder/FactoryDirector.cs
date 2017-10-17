using System;
using DPA_Musicsheets.New.Builder.Generic;
using DPA_Musicsheets.New.Compiler;
using Helpers.Datatypes;
using Helpers.Datatypes.Factories;
using Models;

namespace DPA_Musicsheets.New.Builder
{
    public class FactoryDirector : Factory<DirectorNode<IBuilder<IObject>>>
    {
        public FactoryDirector()
        {
            AddType(CompilerType.Note.ToString(), typeof(DirectorNote));
            AddType(CompilerType.RelativeNote.ToString(), typeof(DirectorNote));
            AddType(CompilerType.Stave.ToString(), typeof(DirectorStave));
        }

        public new IDirector Create(string type)
        {
            if (Types.TryGetValue(type, out var t))
                return (IDirector) Activator.CreateInstance(t);

            return null;
        }
    }
}