namespace GraphLabs.Core.Helpers
{
    /// <summary> Методы расширения для IEdge </summary>
    public static class EdgeExtensions
    {
        /// <summary> Проверяет, является ли данное ребро инцидентным заданной вершине </summary>
        /// <param name="edge"> Ребро </param>
        /// <param name="vertex"> Вершина </param>
        public static bool IsIncidentTo(this IEdge edge, IVertex vertex)
        {
            return edge.Vertex1 == vertex || edge.Vertex2 == vertex;
        }
    }
}
