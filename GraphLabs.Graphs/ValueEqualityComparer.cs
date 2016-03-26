namespace GraphLabs.Graphs
{
    // ReSharper disable ConditionIsAlwaysTrueOrFalse

    /// <summary> Сравниватель вершин </summary>
    public static class ValueEqualityComparer
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

            var areEquals = x.Directed == y.Directed &&
                            (x.Vertex1.Equals(y.Vertex1) && x.Vertex2.Equals(y.Vertex2)
                             || !x.Directed && x.Vertex1.Equals(y.Vertex2) && x.Vertex2.Equals(y.Vertex1));

            if (!areEquals)
                return false;

            var weightedX = x as IWeightedEdge;
            var weightedY = y as IWeightedEdge;

            if (weightedY == null && weightedY == null)
                return true;

            if (weightedX != null && weightedY != null)
                return weightedX.Weight == weightedY.Weight;
            return false;
        }

        // ReSharper restore ConditionIsAlwaysTrueOrFalse
    }
}
