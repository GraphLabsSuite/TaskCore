using System.Linq;
using GraphLabs.Utils;

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

        /// <summary> Сравнение по значению! </summary>
        public static bool GraphEquals(IGraph g1, IGraph g2)
        {
            if (g1 == null || g2 == null) return false;
            if (g1 == g2) return true;
            if (g1.VerticesCount != g2.VerticesCount || g1.EdgesCount != g2.EdgesCount) return false;
            if (g1.Vertices.Any(v => g2.Vertices.SingleOrDefault(v.Equals) == null)) return false;
            var eq = true;
            g1.Vertices.ForEach(v1 =>
                g1.Vertices.ForEach(v2 =>
                    eq &= (g1[v1, v2] != null && g2[
                        g2.Vertices.SingleOrDefault(v1.Equals),
                        g2.Vertices.SingleOrDefault(v2.Equals)] != null) ^
                          (g1[v1, v2] == null && g2[
                              g2.Vertices.SingleOrDefault(v1.Equals),
                              g2.Vertices.SingleOrDefault(v2.Equals)] == null)
                    )
                );
            return eq;
        }

        // ReSharper restore ConditionIsAlwaysTrueOrFalse
    }
}
