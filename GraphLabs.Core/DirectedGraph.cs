using System.Globalization;
using System.Linq;
using GraphLabs.Core.Helpers;

namespace GraphLabs.Core
{
    /// <summary> Ориентированный граф </summary>
    public sealed class DirectedGraph : Graph<Vertex, DirectedEdge>
    {
        /// <summary> Создаёт пустой граф на заданном числе вершин </summary>
        /// <remarks> Вершины в новом графе имеют имена 1, 2, 3, и т. д. </remarks>
        public static DirectedGraph CreateEmpty(int verticesCount)
        {
            var newGraph = new DirectedGraph();
            for (var i = 0; i < verticesCount; ++i)
                newGraph.AddVertex(new Vertex(i.ToString(CultureInfo.InvariantCulture)));
            return newGraph;
        }

        /// <summary> Возвращает ребро с началом в v1 и концом в v2 </summary>
        public override DirectedEdge this[Vertex v1, Vertex v2]
        {
            get
            {
                return EdgesList.SingleOrDefault(e => e.Vertex1.Equals(v1) && e.Vertex2.Equals(v2));
            }
        }

        /// <summary> Граф ориентированный? </summary>
        public override bool Directed
        {
            get { return true; } 
        }

        /// <summary> Допускать два и более ребра между двумя вершинами? </summary>
        public override bool AllowMultipleEdges
        {
            get { return false; }
        }

        /// <summary> Создаёт глубокую копию данного объекта </summary>
        public override object Clone()
        {
            var clone = new DirectedGraph();
            VerticesList.ForEach(v => clone.AddVertex(new Vertex(v.Name)));
            foreach (var edge in EdgesList)
            {
                var v1 = clone.Vertices.Single(v => edge.Vertex1.Equals(v));
                var v2 = clone.Vertices.Single(v => edge.Vertex2.Equals(v));
                clone.AddEdge(new DirectedEdge(v1, v2));
            }

            return clone;
        }
    }
}
