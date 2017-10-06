using System;
using System.Collections.Generic;
using DPA_Musicsheets.New.Compiler.Nodes;
using Helpers;

namespace DPA_Musicsheets.New.Compiler.Statements
{
    public class DispatchPropertyByType<T> : DoubleDispatch<Action<T, INode>>
        where T : INode
    {
        public DispatchPropertyByType(Dictionary<Type, Action<T, INode>> values)
        {
            DispatchByType = values;
        }

        public void AddProperty(T note, INode value)
        {
            DispatchByType.TryGetValue(value.GetType(), out var action);
            action?.Invoke(note, value);
        }
    }
}