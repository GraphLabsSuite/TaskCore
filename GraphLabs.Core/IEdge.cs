namespace GraphLabs.Tasks.Core
{
    /// <summary> Ребро или дуга графа </summary>
    public interface IEdge
    {
        /// <summary> Индекс вершины 1 (вершины-истока) </summary>
        IVertex Vertex1 { get; }

        /// <summary> Индекс вершины 2 (вершины-стока) </summary>
        IVertex Vertex2 { get; }

        /// <summary> Ребро ориентированное? (является дугой?) </summary>
        bool Directed { get; }
    }
}
