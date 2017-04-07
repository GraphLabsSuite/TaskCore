using System;

namespace GraphLabs.Graphs
{
    /// <summary> Non-generic IEdge </summary>
    public interface IEdge : IEquatable<IEdge>, ICloneable
    {
        /// <summary> Индекс вершины 1 (вершины-истока) </summary>
        IVertex Vertex1 { get; }

        /// <summary> Индекс вершины 2 (вершины-стока) </summary>
        IVertex Vertex2 { get; }

        /// <summary> Ребро ориентированное? (является дугой?) </summary>
        bool Directed { get; }

        /// <summary> Ребро взвешенное? </summary>
        int? Weight { get; }
    }
}
