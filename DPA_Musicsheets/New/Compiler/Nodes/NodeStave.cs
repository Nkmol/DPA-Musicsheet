using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.New.Compiler.Nodes
{
    public class NodeStave : INode
    {
        public NodeStave()
        {
            Notes = new List<INode>();
        }

        public IList<INode> Notes;
        public INode RelativeNote, Tempo, Time, Clef;
    }
}
