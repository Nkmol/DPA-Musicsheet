using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.New.Compiler.Nodes
{
    public class NodeNote : INode
    {
        public INode NodeLetter, NodeAmplitude, NodeNumber, NodeChroma, NodeDot;
    }
}
