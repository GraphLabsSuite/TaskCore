using System.Globalization;
using System.Linq;
using GraphLabs.Tasks.Core.Helpers;

namespace GraphLabs.Tasks.Core
{
    /// <summary> Обыкновенный неориентированный граф </summary>
    public class UndirectedGraph : Graph 
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


        /// <summary> Возвращает ребро между вершинамиv v1 и v2. Если ребра нет, то null. </summary>
        public override IEdge this[IVertex v1, IVertex v2]
        {
            get
            {
                var edgeToFind = new UndirectedEdge(v1, v2);
                return EdgesList.FirstOrDefault(e => EdgesComparer.Comparer.Equals(e, edgeToFind));
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
                var v1 = clone.Vertices.First(v => VerticesComparer.Comparer.Equals(v, edge.Vertex1));
                var v2 = clone.Vertices.First(v => VerticesComparer.Comparer.Equals(v, edge.Vertex2));
                clone.AddEdge(new UndirectedEdge(v1, v2));
            }

            return clone;
        }
    }
}
