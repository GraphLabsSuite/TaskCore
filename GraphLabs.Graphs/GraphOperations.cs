using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using GraphLabs.Graphs.Helpers;
using GraphLabs.Utils;

namespace GraphLabs.Graphs
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

        private static bool DirectCompare<TVertex, TEdge>(IGraph<TVertex, TEdge> graph1, IGraph<TVertex, TEdge> graph2)
            where TVertex : IVertex
            where TEdge : IEdge<TVertex>
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

        private static void UpdateBijection<TVertex>(IEnumerable<TVertex> verticesList1, IEnumerable<TVertex> verticesList2)
            where TVertex : IVertex
        {
            _bijection = new Dictionary<string, string>();
            foreach (string[] i in verticesList1.Zip(verticesList2, (a, b) => new string[] { a.Name, b.Name }))
                _bijection.Add(i[1], i[0]);
        }

        /// <summary> Проверка двух графов на изоморфизм. </summary>
        public static bool CheckIsomorphism<TVertex, TEdge>(IGraph<TVertex, TEdge> graph1, IGraph<TVertex, TEdge> graph2)
            where TVertex : IVertex
            where TEdge : IEdge<TVertex>
        {
            if (graph1.VerticesCount != graph2.VerticesCount || graph1.EdgesCount != graph2.EdgesCount)
                return false;
            foreach (TVertex[] perm in Permute(graph1.Vertices.ToArray()))
            {
                UpdateBijection(perm, graph2.Vertices.ToArray());
                if (DirectCompare(graph1, graph2))
                    return true;
            }
            return false;
        }

        #endregion


        #region Операции над графами

        /// <summary> Объединение графов </summary>
        [Obsolete("Нужно переписать")]
        public static Graph<TVertex, TEdge> Union<TVertex, TEdge>(params Graph<TVertex, TEdge>[] graphs)
            where TVertex : IVertex
            where TEdge : IEdge<TVertex>
        {
            Contract.Requires(graphs != null && graphs.Any());
            throw new NotImplementedException();
            //var copies = graphs.Select(g => (Graph<TVertex, TEdge>)g.Clone()).ToArray();
            //for (var i = 1; i <= copies.Length; ++i)
            //{
            //    copies[i - 1].Vertices.ForEach(v => v.Rename(string.Format("{0}-{1}", i.ToString(CultureInfo.InvariantCulture), v.Name)));
            //}

            //var result = copies.First();
            //for (var i = 1; i < copies.Length; ++i)
            //{
            //    copies[i].Vertices.ForEach(result.AddVertex);
            //    copies[i].Edges.ForEach(result.AddEdge);
            //}

            //return result;
        }

        /// <summary> Пересечение двух графов. </summary>
        public static Graph<TVertex, TEdge> Intersection<TVertex, TEdge>(Graph<TVertex, TEdge> g1, Graph<TVertex, TEdge> g2)
            where TVertex : IVertex
            where TEdge : IEdge<TVertex>
        {
            var resultVertices = g1.Vertices.Where(g => g2.Vertices.Contains(g)).Select(e => (TVertex)e.Clone()).ToArray();
            var resultEdges = g1.Edges.Where(g => g2.Edges.Contains(g)).Select(e => (TEdge)e.Clone()).ToArray();

            var result = (Graph<TVertex, TEdge>)g1.Clone();
            resultEdges.ForEach(result.RemoveEdge);
            resultVertices.ForEach(result.RemoveVertex);

            return result;
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
