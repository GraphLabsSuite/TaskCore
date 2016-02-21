using System;
using System.Globalization;
using System.Linq;
using GraphLabs.Graphs.Helpers;
using GraphLabs.Utils;

namespace GraphLabs.Graphs
{
    /// <summary> Обыкновенный неориентированный граф </summary>
    public class UndirectedGraph : Graph<Vertex, UndirectedEdge>
    {

        #region Полезности

        /// <summary> Создаёт пустой граф на заданном числе вершин </summary>
        /// <remarks> Вершины в новом графе имеют имена 1, 2, 3, и т. д. </remarks>
        public static UndirectedGraph CreateEmpty(int verticesCount)
        {
            var newGraph = new UndirectedGraph();
            for (var i = 0; i < verticesCount; ++i)
                newGraph.AddVertex(new Vertex(i.ToString(CultureInfo.InvariantCulture)));
            return newGraph;
        }

        #endregion // Полезности


        /// <summary> Проверяет эквивалентность неориентированных графов </summary>
        public override bool Equals(object o)
        {
            return Equals(this, o as UndirectedGraph);
        }

        /// <summary> Хеш-код </summary>
        public override int GetHashCode()
        {
            unchecked
            {
                return Vertices.GetHashCode() ^ 397 + Edges.GetHashCode() ^ 397;
            }
        }

        /// <summary> Проверяет эквивалентность неориентированных графов </summary>
        protected bool Equals(UndirectedGraph g)
        {
            if (g == null) return false;
            if (this == g) return true;
            if (VerticesCount != g.VerticesCount || EdgesCount != g.EdgesCount) return false;
            if (Vertices.Any(v => g.Vertices.SingleOrDefault(v.Equals) == null)) return false;
            var eq = true;
            Vertices.ForEach(v1 =>
                Vertices.ForEach(v2 =>
                    eq &= (this[v1, v2] != null && g[
                        g.Vertices.SingleOrDefault(v1.Equals),
                        g.Vertices.SingleOrDefault(v2.Equals)] != null) ^
                          (this[v1, v2] == null && g[
                              g.Vertices.SingleOrDefault(v1.Equals),
                              g.Vertices.SingleOrDefault(v2.Equals)] == null)
                    )
                );
            return eq;
        }

        /// <summary> Возвращает ребро между вершинамиv v1 и v2. Если ребра нет, то null. </summary>
        public override UndirectedEdge this[Vertex v1, Vertex v2]
        {
            get
            {
                var edgeToFind = new UndirectedEdge(v1, v2);
                return EdgesList.FirstOrDefault(e => e.Equals(edgeToFind));
            }
        }

        /// <summary> Граф ориентированный? </summary>
        public override bool Directed
        {
            get { return false; }
        }

        /// <summary> Допускать два и более ребра между двумя вершинами? </summary>
        public override bool AllowMultipleEdges
        {
            get { return false; }
        }

        /// <summary> Создаёт глубокую копию данного объекта </summary>
        public override object Clone()
        {
            var clone = new UndirectedGraph();
            VerticesList.ForEach(v => clone.AddVertex(new Vertex(v.Name)));
            foreach (var edge in EdgesList)
            {
                var v1 = clone.Vertices.First(edge.Vertex1.Equals);
                var v2 = clone.Vertices.First(edge.Vertex2.Equals);
                clone.AddEdge(new UndirectedEdge(v1, v2));
            }

            return clone;
        }
    }
}
