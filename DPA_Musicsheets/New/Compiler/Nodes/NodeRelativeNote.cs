using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.New.Compiler.Nodes
{
    public class NodeRelativeNote : INode
    {
        public INode NodeLetter, NodeAmplitude;
    }
}
