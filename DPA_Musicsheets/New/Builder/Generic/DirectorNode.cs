using System;
using System.Collections.Generic;
using DPA_Musicsheets.New.Compiler;
using DPA_Musicsheets.New.Compiler.Nodes.Abstractions;
using Helpers.Datatypes;
using Models;

namespace DPA_Musicsheets.New.Builder.Generic
{
    public abstract class DirectorNode<T> : IDirector
        where T : class
    {
        protected static Dictionary<CompilerType, Action<T, Node>> ValueDirectorMap =
            new Dictionary<CompilerType, Action<T, Node>>();

        protected static Dictionary<CompilerType, Action<T, IObject>> ComponentDirectorMap =
            new Dictionary<CompilerType, Action<T, IObject>>();

        // Used so I can Polymorphism on IDirector without having to specify generic types 
        // (as this would not work with Polymorphism, atleast no "in" generic types)
        public bool Direct(IBuilder<IObject> builder, INode node)
        {
            return Direct(builder as T, node as Node);
        }

        public bool Direct(IBuilder<IObject> builder, IObject node, CompilerType context)
        {
            return Direct((T) builder, node, context);
        }

        public virtual bool Direct(T to, Node input)
        {
            ValueDirectorMap.TryGetValue(input.Context, out var result);
            result?.Invoke(to, input);

            return result != null;
        }

        public virtual bool Direct(T to, IObject input, CompilerType context)
        {
            ComponentDirectorMap.TryGetValue(context, out var result);
            result?.Invoke(to, input);

            return result != null;
        }
    }
}