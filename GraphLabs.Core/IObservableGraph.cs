using System;

namespace GraphLabs.Core
{
    /// <summary> Граф </summary>
    public interface IObservableGraph : IGraph
    {
        /// <summary> Происходит при добавлении/удалении рёбер или вершин </summary>
        event EventHandler<GraphChangedEventArgs> GraphChanged;
    }
}
