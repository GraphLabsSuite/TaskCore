using GraphLabs.Core;
using GraphLabs.Core.Helpers;
using NUnit.Framework;
using System;
using System.Linq;

namespace GraphLabs.Tests.Core
{
    /// <summary> Тривиальные тесты неориентированного графа </summary>
    class DirectedGraphTests
    {
        /// <summary> Тест конструктора по-умолчанию </summary>
        [Test]
        public void CreationTest()
        {
            var graph = new DirectedGraph();
            Assert.AreEqual(graph.VerticesCount, 0);
            Assert.AreEqual(graph.EdgesCount, 0);
            Assert.AreEqual(graph.Directed, true);
            Assert.AreEqual(graph.AllowMultipleEdges, false);
            Assert.AreEqual(graph.Vertices.Count, 0);
            Assert.AreEqual(graph.Edges.Count, 0);
        }

        /// <summary> Тест создания пустого графа с заданным числом вершин </summary>
        [Test]
        public void CreateEmptyTest()
        {
            var graph = DirectedGraph.CreateEmpty(5);
            Assert.AreEqual(graph.VerticesCount, 5);
            Assert.AreEqual(graph.EdgesCount, 0);
        }

        /// <summary> Тест добавления вершин </summary>
        [Test]
        public void AddVertexTest()
        {
            var graph = new DirectedGraph();
            var newVertex = new Vertex("test");
            Assert.DoesNotThrow(() => graph.AddVertex(newVertex));
            Assert.AreEqual(graph.VerticesCount, 1);
            Assert.AreEqual(graph.Vertices.Count, 1);
            Assert.AreEqual(graph.Vertices.First(), newVertex);
            Assert.Throws<InvalidOperationException>(() => graph.AddVertex(newVertex));
            Assert.Throws<ArgumentNullException>(() => graph.AddVertex(null));
        }

        /// <summary> Тест добавления ребра </summary>
        [Test]
        public void AddEdgeTest()
        {
            var graph = new DirectedGraph();
            var vertex1 = new Vertex("test-1");
            var vertex2 = new Vertex("test-2");
            graph.AddVertex(vertex1);
            graph.AddVertex(vertex2);
            var edge = new DirectedEdge(vertex1, vertex2);

            Assert.DoesNotThrow(() => graph.AddEdge(edge));
            Assert.AreEqual(graph.EdgesCount, 1);
            Assert.AreEqual(graph.Edges.First(), edge);
            Assert.IsNotNull(graph[vertex1, vertex2]);
            Assert.IsNull(graph[vertex2, vertex1]);
            Assert.Throws<InvalidOperationException>(() => graph.AddEdge(edge));
            Assert.Throws<ArgumentNullException>(() => graph.AddEdge(null));
        }

        /// <summary> Тест удаления ребра </summary>
        [Test]
        public void RemoveEdgeTest()
        {
            var graph = new DirectedGraph();
            var vertex1 = new Vertex("test-1");
            var vertex2 = new Vertex("test-2");
            graph.AddVertex(vertex1);
            graph.AddVertex(vertex2);
            var edge = new DirectedEdge(vertex1, vertex2);

            graph.AddEdge(edge);
            Assert.DoesNotThrow(() => graph.RemoveEdge(edge));
            Assert.AreEqual(graph.EdgesCount, 0);
            Assert.IsNull(graph[vertex1, vertex2]);
            Assert.IsNull(graph[vertex2, vertex1]);
            Assert.Throws<ArgumentException>(() => graph.RemoveEdge(edge));
            Assert.Throws<ArgumentNullException>(() => graph.RemoveEdge(null));
        }

        /// <summary> Тест удаления вершин из пустого графа </summary>
        [Test]
        public void RemoveVertexFromEmptyGraphTest()
        {
            var graph = new DirectedGraph();
            var newVertex = new Vertex("test");
            graph.AddVertex(newVertex);
            Assert.Throws<ArgumentException>(() => graph.RemoveVertex(new Vertex("test-2")));
            Assert.DoesNotThrow(() => graph.RemoveVertex(newVertex));
            Assert.AreEqual(graph.VerticesCount, 0);
            Assert.AreEqual(graph.Vertices.Count, 0);
            Assert.Throws<ArgumentException>(() => graph.RemoveVertex(newVertex));
            Assert.Throws<ArgumentNullException>(() => graph.RemoveVertex(null));
        }

        /// <summary> Тест удаления вершины из непустого графа </summary>
        [Test]
        public void RemoveVertexFromNonEmptyGraphTest()
        {
            var graph = new DirectedGraph();
            var newVertex1 = new Vertex("test-1");
            var newVertex2 = new Vertex("test-2");
            graph.AddVertex(newVertex1);
            graph.AddVertex(newVertex2);
            var edge = new DirectedEdge(newVertex1, newVertex2);
            graph.AddEdge(edge);

            Assert.DoesNotThrow(() => graph.RemoveVertex(newVertex1));
            Assert.AreEqual(graph.VerticesCount, 1);
            Assert.AreEqual(graph.EdgesCount, 0);
            Assert.IsTrue(graph.Vertices.Contains(newVertex2));
        }

        /// <summary> Тест клонирования </summary>
        [Test]
        public void CloneTest()
        {
            var graph = new DirectedGraph();
            var newVertex1 = new Vertex("test-1");
            var newVertex2 = new Vertex("test-2");
            var newVertex3 = new Vertex("test-3");
            graph.AddVertex(newVertex1);
            graph.AddVertex(newVertex2);
            graph.AddVertex(newVertex3);
            var newEdge1 = new DirectedEdge(newVertex1, newVertex2);
            var newEdge2 = new DirectedEdge(newVertex1, newVertex3);
            graph.AddEdge(newEdge1);
            graph.AddEdge(newEdge2);

            var clonedGraph = graph.Clone() as IGraph;
            Assert.IsTrue(clonedGraph is DirectedGraph);
            Assert.AreEqual(graph.VerticesCount, clonedGraph.VerticesCount);
            foreach (var vertex in graph.Vertices)
            {
                var clonedVertex = clonedGraph.Vertices.Single(v => VerticesComparer.Comparer.Equals(v, vertex));
                Assert.AreNotSame(clonedVertex, vertex);
            }
            Assert.AreEqual(graph.EdgesCount, clonedGraph.EdgesCount);
            foreach (var clonedEdge in clonedGraph.Edges)
            {
                Assert.IsTrue(clonedEdge is DirectedEdge);
                var edge = graph.Edges.Single(e => EdgesComparer.Comparer.Equals(e, clonedEdge));
                Assert.AreNotSame(edge, clonedEdge);
            }
        }
    }
}
