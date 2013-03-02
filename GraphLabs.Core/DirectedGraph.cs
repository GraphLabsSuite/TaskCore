using System.Globalization;
using System.Linq;
using GraphLabs.Tasks.Core.Helpers;

namespace GraphLabs.Tasks.Core
{
    /// <summary> Ориентированный граф </summary>
    public class DirectedGraph : Graph
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
        public override IEdge this[IVertex v1, IVertex v2]
        {
            get
            {
                var edgeToFind = new DirectedEdge(v1, v2);
                return EdgesList.FirstOrDefault(e => EdgesComparer.Comparer.Equals(e, edgeToFind));
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
                var v1 = clone.Vertices.First(v => VerticesComparer.Comparer.Equals(v, edge.Vertex1));
                var v2 = clone.Vertices.First(v => VerticesComparer.Comparer.Equals(v, edge.Vertex2));
                clone.AddEdge(new DirectedEdge(v1, v2));
            }

            return clone;
        }
    }
}
