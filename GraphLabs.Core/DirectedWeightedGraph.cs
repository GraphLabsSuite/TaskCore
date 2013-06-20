using System.Globalization;
using System.Linq;
using GraphLabs.Core.Helpers;

namespace GraphLabs.Core
{
    /// <summary> Взвешенный орграф </summary>
    public class DirectedWeightedGraph : DirectedGraph
    {
        /// <summary> Создаёт пустой граф на заданном числе вершин </summary>
        /// <remarks> Вершины в новом графе имеют имена 1, 2, 3, и т. д. </remarks>
        public static DirectedWeightedGraph CreateEmpty(int verticesCount)
        {
            var newGraph = new DirectedWeightedGraph();
            for (var i = 0; i < verticesCount; ++i)
                newGraph.AddVertex(new Vertex(i.ToString(CultureInfo.InvariantCulture)));
            return newGraph;
        }

        /// <summary> Возвращает ребро с началом в v1 и концом в v2 </summary>
        public override IEdge this[IVertex v1, IVertex v2]
        {
            get
            {
                return EdgesList.FirstOrDefault(e => e.Vertex1 == v1 && e.Vertex2 == v2);
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
            var clone = new DirectedWeightedGraph();
            VerticesList.ForEach(v => clone.AddVertex(new Vertex(v.Name)));
            foreach (var edge in EdgesList)
            {
                var v1 = clone.Vertices.First(v => VerticesComparer.Comparer.Equals(v, edge.Vertex1));
                var v2 = clone.Vertices.First(v => VerticesComparer.Comparer.Equals(v, edge.Vertex2));
                clone.AddEdge(new DirectedWeightedEdge(v1, v2, ((IWeightedEdge)edge).Weight));
            }

            return clone;
        }
    }
}
