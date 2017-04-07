namespace GraphLabs.Graphs.Helpers
{
    /// <summary> Методы расширения для IEdge </summary>
    public static class EdgeExtensions
    {
        /// <summary> Проверяет, является ли данное ребро инцидентным заданной вершине </summary>
        /// <param name="edge"> Ребро </param>
        /// <param name="vertex"> Вершина </param>
        public static bool IsIncidentTo<TVertex>(this IEdge<TVertex> edge, TVertex vertex)
            where TVertex : IVertex
        {
            return ReferenceEquals(edge.Vertex1, vertex) || ReferenceEquals(edge.Vertex2, vertex);
        }

        /// <summary> Проверяет, является ли данное ребро взвешенным </summary>
        public static bool IsWeighted(this IEdge edge)
        {
            return edge.Weight.HasValue;
        }
    }
}
