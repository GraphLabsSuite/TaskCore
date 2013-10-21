using System;

namespace GraphLabs.Core
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
    }
}
