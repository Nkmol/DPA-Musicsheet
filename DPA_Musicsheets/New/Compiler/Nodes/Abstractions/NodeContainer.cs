using System.Collections.Generic;

namespace DPA_Musicsheets.New.Compiler.Nodes.Abstractions
{
    public class NodeContainer : BaseNode
    {
        public IList<BaseNode> Properties;

        public NodeContainer()
        {
            Properties = new List<BaseNode>();
        }
    }
}
