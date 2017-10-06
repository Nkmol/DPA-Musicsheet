using System.Collections.Generic;

namespace DPA_Musicsheets.New.Compiler.Nodes
{
    public class NodeStave : INode
    {
        public IList<INode> Notes;
        public INode RelativeNote, Tempo, Time, Clef;

        public NodeStave()
        {
            Notes = new List<INode>();
        }
    }
}