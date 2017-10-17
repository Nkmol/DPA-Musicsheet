using DPA_Musicsheets.New.Compiler;
using DPA_Musicsheets.New.Compiler.Nodes.Abstractions;
using Helpers.Datatypes;
using Models;

namespace DPA_Musicsheets.New.Builder
{
    public interface IDirector
    {
        // TODO find way to use generic types for these types
        bool Direct(IBuilder<IObject> builder, INode node);

        bool Direct(IBuilder<IObject> builder, IObject node, CompilerType context);
    }
}