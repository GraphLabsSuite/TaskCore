using System;
using GraphLabs.Core;
using GraphLabs.Core.Helpers;
using NUnit.Framework;

namespace GraphLabs.Tests.Tasks.Core
{
    /// <summary> Тесты сравнения рёбер и вершин </summary>
    /// <remarks> Тут тестируем не только сами компараторы, но и обычное сравнение при помощи operator== </remarks>
    [TestFixture]
    public class ComparingTests
    {
        /// <summary> Тестирование компаратора вершин </summary>
        [Test]
        public void VerticesComparerTests()
        {
            var vertex1 = new Vertex("A");
            var vertex2 = new Vertex("A");
            var vertex3 = new Vertex("B");
            Assert.Catch<Exception>(() => VerticesComparer.Comparer.Equals(null, vertex2));
            Assert.IsTrue(VerticesComparer.Comparer.Equals(vertex1, vertex2));
            Assert.IsFalse(VerticesComparer.Comparer.Equals(vertex1, vertex3));
            Assert.IsTrue(VerticesComparer.Comparer.Equals(vertex1, vertex2));
        }

        /// <summary> Тестирование компаратора рёбер </summary>
        [Test]
        public void EdgesComparerTests()
        {
            var v11 = new Vertex("A");
            var v21 = new Vertex("B");
            var v12 = new Vertex("A");
            var v22 = new Vertex("B");
            var dirEdge1 = new DirectedEdge(v11, v21);
            var dirEdge2 = new DirectedEdge(v12, v22);
            var dirEdge3 = new DirectedEdge(v11, v22);
            var dirEdge4 = new DirectedEdge(v21, v11);
            var dirEdge5 = new DirectedEdge(v11, v11);
            var undirEdge1 = new UndirectedEdge(v11, v21);
            var undirEdge2 = new UndirectedEdge(v12, v22);
            var undirEdge3 = new UndirectedEdge(v11, v22);
            var undirEdge4 = new UndirectedEdge(v21, v11);
            var undirEdge5 = new DirectedEdge(v11, v11);
            Assert.Catch(() => EdgesComparer.Comparer.Equals(dirEdge1, null));
            Assert.IsTrue(EdgesComparer.Comparer.Equals(dirEdge1, dirEdge2));
            Assert.IsTrue(EdgesComparer.Comparer.Equals(dirEdge1, dirEdge3));
            Assert.IsTrue(EdgesComparer.Comparer.Equals(undirEdge1, undirEdge2));
            Assert.IsTrue(EdgesComparer.Comparer.Equals(undirEdge1, undirEdge3));
            Assert.IsFalse(EdgesComparer.Comparer.Equals(dirEdge1, undirEdge1));
            Assert.IsFalse(EdgesComparer.Comparer.Equals(dirEdge1, dirEdge4));
            Assert.IsTrue(EdgesComparer.Comparer.Equals(undirEdge1, undirEdge4));
            Assert.IsFalse(EdgesComparer.Comparer.Equals(dirEdge1, dirEdge5));
            Assert.IsFalse(EdgesComparer.Comparer.Equals(undirEdge1, undirEdge5));
        }

        /// <summary> Тестирование простого сравнения вершин </summary>
        [Test]
        public void VerticesComparingTests()
        {
            var vertex1 = new Vertex("A");
            var vertex2 = new Vertex("B");
            Assert.AreNotEqual(vertex1, vertex2);
            Assert.AreEqual(vertex1, vertex1);
        }

        /// <summary> Тестирование простого сравнения рёбер </summary>
        [Test]
        public void EdgesComparingTests()
        {
            var vertex1 = new Vertex("A");
            var vertex2 = new Vertex("B");
            var dirEdge1 = new DirectedEdge(vertex1, vertex2);
            var dirEdge2 = new DirectedEdge(vertex1, vertex2);
            var undirEdge1 = new UndirectedEdge(vertex1, vertex2);
            var undirEdge2 = new UndirectedEdge(vertex1, vertex2);
            Assert.AreNotEqual(dirEdge1, dirEdge2);
            Assert.AreNotEqual(undirEdge1, undirEdge2);
            Assert.AreEqual(dirEdge2, dirEdge2);
            Assert.AreEqual(undirEdge2, undirEdge2);
        }
    }
}

