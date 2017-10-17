using System.Collections.Generic;
using System.Linq;
using DPA_Musicsheets.New.Compiler.Nodes.Abstractions;
using Helpers.Datatypes;
using Models;

namespace DPA_Musicsheets.New.Builder
{
    public class MusicSheetCreator
    {
        public static List<IObject> CreateComponents(IEnumerable<BaseNode> nodes,
            List<IBuilder<IObject>> builders = null, IDirector lastDirector = null, bool firstCall = false)
        {
            if (builders == null) builders = new List<IBuilder<IObject>>();

            foreach (var node in nodes)
                if (node is NodeContainer nodeContainer)
                {
                    var director = new FactoryDirector().Create(node.Context.ToString());
                    var builder = new FactoryBuilder().Create(node.Context.ToString());

                    builders.Add(builder);
                    var components = CreateComponents(nodeContainer.Properties, builders, director);
                    if (components.Any())
                        if (builders.Count > 0)
                            lastDirector?.Direct(builders.Last(), components.Last(), node.Context);
                        else
                            return components;
                }
                else if (node is Node nodeValue)
                {
                    var builder = builders.Last();
                    lastDirector?.Direct(builder, node);
                }

            // - After values processed -

            // The last processed state should just build all remaining builders
            if (firstCall)
                return builders.Select(b => b.Build()).ToList();
            var component = builders.Last().Build();
            builders.RemoveAt(builders.Count - 1);

            return new List<IObject> {component};
        }
    }
}