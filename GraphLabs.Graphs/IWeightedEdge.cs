using System;

namespace GraphLabs.Graphs
{
    /// <summary> Взвешенное ребро </summary>
    public interface IWeightedEdge : IEdge, IEquatable<IWeightedEdge>
    {
        /// <summary> Вес ребра </summary>
        int Weight { get; }
    }
}
