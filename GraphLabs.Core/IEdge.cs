namespace GraphLabs.Core
{
    /// <summary> Ребро или дуга графа </summary>
    public interface IEdge<TVertex> : IEdgeBase
        where TVertex : IVertex
    {
        /// <summary> Индекс вершины 1 (вершины-истока) </summary>
        new TVertex Vertex1 { get; }

        /// <summary> Индекс вершины 2 (вершины-стока) </summary>
        new TVertex Vertex2 { get; }
    }
}
