namespace GraphLabs.Core
{
    // ReSharper disable ConditionIsAlwaysTrueOrFalse

    /// <summary> Сравниватель вершин </summary>
    public static class EqualityComparer
    {
        /// <summary> Сравнение по значению! </summary>
        public static bool VerticesEquals(IVertex x, IVertex y)
        {
            if (ReferenceEquals(x, y))
                return true;
            if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
                return false;

            return x.Name == y.Name;
        }

        /// <summary> Сравнение по значению! </summary>
        public static bool EdgesEquals(IEdge x, IEdge y)
        {
            if (ReferenceEquals(x, y))
                return true;
            if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
                return false;

            return x.Directed == y.Directed &&
                   (x.Vertex1.Equals(y.Vertex1) && x.Vertex2.Equals(y.Vertex2)
                    || !x.Directed && x.Vertex1.Equals(y.Vertex2) && x.Vertex2.Equals(y.Vertex1));
        }

    }
    // ReSharper restore ConditionIsAlwaysTrueOrFalse
}
