using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Collections.Generic;
using GraphLabs.Core.Helpers;

namespace GraphLabs.Core
{
    /// <summary> Статичный класс, реализующий операции над графами. </summary>
    public static class GraphOperations
    {
        #region Вспомогательные методы для проверки изоморфизма
        private static Dictionary<string, string> _bijection;

        private static IEnumerable<T[]> Permute<T>(T[] xs, params T[] pre)
        {
            if (xs.Length == 0) yield return pre;
            for (int i = 0; i < xs.Length; i++)
            {
                var tmp_xs = xs.Take(i).Union(xs.Skip(i + 1)).ToArray();
                var tmpParams = pre.Union(new[] { xs[i] }).ToArray();
                foreach (T[] y in Permute(tmp_xs, tmpParams))
                {
                    yield return y;
                }
            }
        }

        private static bool CompareHelper(IVertex vertex1, IVertex vertex2)
        {
            return vertex1.Name == _bijection[vertex2.Name];
        }

        private static bool DirectCompare(IGraph graph1, IGraph graph2)
        {
            int equals = 0;
            int count = graph1.EdgesCount;
            for (int i = 0; i < count; i++)
                for (int j = 0; j < count; j++)
                    if (CompareHelper(graph1.Edges[i].Vertex1, graph2.Edges[j].Vertex1) &&
                        CompareHelper(graph1.Edges[i].Vertex2, graph2.Edges[j].Vertex2) ||
                       (CompareHelper(graph1.Edges[i].Vertex2, graph2.Edges[j].Vertex1) &&
                        CompareHelper(graph1.Edges[i].Vertex1, graph2.Edges[j].Vertex2)))
                    {
                        equals++;
                        break;
                    }
            return equals == count;
        }

        private static void UpdateBijection(IEnumerable<IVertex> verticesList1, IEnumerable<IVertex> verticesList2)
        {
            _bijection = new Dictionary<string, string>();
            foreach (string[] i in verticesList1.Zip(verticesList2, (a, b) => new string[] { a.Name, b.Name }))
                _bijection.Add(i[1], i[0]);
        }

        /// <summary> Проверка двух графов на изоморфизм. </summary>
        public static bool CheckIsomorphism(IGraph graph1, IGraph graph2)
        {
            if (graph1.VerticesCount != graph2.VerticesCount || graph1.EdgesCount != graph2.EdgesCount)
                return false;
            foreach (IVertex[] perm in Permute(graph1.Vertices.ToArray()))
            {
                UpdateBijection(perm, graph2.Vertices.ToArray());
                if (DirectCompare(graph1, graph2))
                    return true;
            }
            return false;
        }

        #endregion


        #region Операции над графами

        /// <summary> Объединение двух графов. </summary>
        public static Graph Union(Graph g1, Graph g2)
        {
            int g1length = g1.VerticesCount;
            int g2length = g2.VerticesCount;

            Graph g = g1;


            for (int i = 0; i <= g1length; i++)
            {
                int newVertexNum = -1;
                for (int j = 0; j <= g2length; j++)
                {
                    if (g.Vertices.ToArray()[i].Equals(g2.Vertices.ToArray()[j]))
                    {
                        newVertexNum = j;
                    };
                }
                if (newVertexNum != -1)
                {
                    g.AddVertex(g2.Vertices.ToArray()[newVertexNum]);
                }
            }

            for (int i = 0; i <= g2length; i++)
            {
                for (int j = 0; j <= g2length; j++)
                {
                    if (!g.Edges.ToArray()[i].Equals(g2.Edges.ToArray()[j]))
                    {
                        g.AddEdge(g2.Edges.ToArray()[j]);
                    };
                }
            }
            return g;
        }

        /// <summary> Пересечение двух графов. </summary>
        public static Graph Intersection(Graph g1, Graph g2)
        {
            int g1length = g1.VerticesCount;
            int g2length = g2.VerticesCount;

            Graph g = g1;
            foreach (IEdge e in  g.Edges.ToArray()){ 
                g.RemoveEdge(e);
            }

            for (int i = 0; i <= g1length; i++)
            {
                int newVertexNum = -1;
                for (int j = 0; j <= g2length; j++)
                {
                    if (g.Vertices.ToArray()[i].Equals(g2.Vertices.ToArray()[j]))
                    {
                        newVertexNum = j;
                    };
                }
                if (newVertexNum != -1)
                {
                    g.AddVertex(g2.Vertices.ToArray()[newVertexNum]);
                }
            }

            for (int i = 0; i <= g2length; i++)
            {
                for (int j = 0; j <= g2length; j++)
                {
                    if (g.Edges.ToArray()[i].Equals(g2.Edges.ToArray()[j]))
                    {
                        g.AddEdge(g2.Edges.ToArray()[j]);
                    };
                }
            }
            return g;
        }

        #endregion 


        #region Поиск компонент сильной связности

        /// <summary> Класс для нахождения компонент сильной связности </summary>
        //todo: добавить описание алгоритма на вики
        private class SccBuilder
        {
            /// <summary> Ищет компоненты связности для заданного графа </summary>
            public static ICollection<IGraph> FindComponents(IGraph graph)
            {
                return (new SccBuilder(graph)).BuildComponents();
            }

            private readonly int[,] _accessibilityMatrix;
            private readonly IGraph _graph;
            private readonly IList<IVertex> _vertices;

            private SccBuilder(IGraph graph)
            {
                _graph = graph;
                _vertices = _graph.Vertices;
                _accessibilityMatrix = new int[_graph.VerticesCount, _graph.VerticesCount];
            }

            private void BuildAccessibilityMatrix(int startIndex, int currentIndex)
            {
                var currentVertex = _vertices[currentIndex];

                for (var i = 0; i < _graph.VerticesCount; ++i)
                {
                    if (i == startIndex || 
                        _graph[currentVertex, _vertices[i]] == null ||
                        _accessibilityMatrix[startIndex, i] != 0)
                    {
                        continue;
                    }

                    _accessibilityMatrix[startIndex, i] = 1;
                    BuildAccessibilityMatrix(startIndex, i);
                }
            }

            
            //todo: кажется, тут местами можно немного проще сделать
            private ICollection<IGraph> BuildComponents()
            {
                for (var i = 0; i < _graph.VerticesCount; ++i)
                {
                    BuildAccessibilityMatrix(i, i);
                }

                var s = new int[_graph.VerticesCount, _graph.VerticesCount];
                
                
                for (var i = 0; i < _graph.VerticesCount; ++i)
                {
                    for (var j = 0; j < _graph.VerticesCount; ++j)
                    {
                        s[i, j] = _accessibilityMatrix[i, j] * _accessibilityMatrix[j, i];
                    }
                }
 
                var added = new bool[_graph.VerticesCount];
                for (var i = 0; i < added.Length; ++i)
                {
                    added[i] = false;
                }

                var components = new List<IGraph>();
                for (var i = 0; i < _graph.VerticesCount; ++i)
                {
                    if (added[i])
                        continue;
                    var scc = _graph.Directed
                                     ? (IGraph)new DirectedGraph()
                                     : (IGraph)new UndirectedGraph();

                    added[i] = true;
                    scc.AddVertex(_vertices[i]);
                    for (var j = 0; j < _graph.VerticesCount; ++j)
                    {
                        if (!added[j] && s[i, j] == 1)
                        {
                            added[j] = true;
                            scc.AddVertex(_vertices[j]);
                        }
                    }
                    components.Add(scc);
                }
                foreach (var edge in _graph.Edges)
                {
                    var whereToAdd =
                        components.Where(c => c.Vertices.Contains(edge.Vertex1) && c.Vertices.Contains(edge.Vertex2));
                    whereToAdd.ForEach(c => c.AddEdge(edge));
                }
                return components;
            }
        }

        /// <summary> Находит компоненты сильной связности для заданного графа </summary>
        /// <requires>graph != null</requires>
        /// <requires>!graph.AllowMultipleEdges</requires>
        /// <requires>graph.Directed</requires>
        public static ICollection<IGraph> FindStronglyConnectedComponents(IGraph graph)
        {
            Contract.Requires<ArgumentNullException>(graph != null);
            // надо понять, что даст алгоритм в случае с MultipleEdges, поэтому пока запретим их
            Contract.Requires<ArgumentException>(!graph.AllowMultipleEdges);
            // Аналогично с неориентированными графами
            Contract.Requires<ArgumentException>(graph.Directed);

            return SccBuilder.FindComponents(graph);
        }

        #endregion
    }
}
