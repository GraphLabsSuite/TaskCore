using System;
using GraphLabs.Core;
using GraphLabs.Core.Helpers;
using NUnit.Framework;

namespace GraphLabs.Tests.Core
{
    /// <summary> Тесты сравнения рёбер и вершин </summary>
    [TestFixture]
    public class ComparingTests
    {
        /// <summary> Тестирование компаратора вершин </summary>
        [Test]
        public void TestVerticesComparing()
        {
            var vertex1 = new Vertex("A");
            var vertex2 = new Vertex("A");
            var vertex3 = new Vertex("B");
            Assert.IsFalse((Vertex)null == vertex1);
            Assert.IsTrue((Vertex)null == (Vertex)null);
            Assert.IsTrue(vertex1 == vertex2);
            Assert.IsFalse(vertex1 == vertex3);
            Assert.IsTrue(vertex2.Equals(vertex1));
            Assert.IsFalse(vertex3.Equals(vertex2));
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
            Assert.IsTrue((DirectedEdge)null == (DirectedEdge)null);
            Assert.IsTrue(dirEdge1.Equals(dirEdge2));
            Assert.IsTrue(dirEdge1.Equals(dirEdge3));
            Assert.IsTrue(undirEdge1.Equals(undirEdge2));
            Assert.IsTrue(undirEdge1.Equals(undirEdge3));
            Assert.IsFalse(dirEdge1.Equals(undirEdge1));
            Assert.IsFalse(dirEdge1.Equals(dirEdge4));
            Assert.IsTrue(undirEdge1.Equals(undirEdge4));
            Assert.IsFalse(dirEdge1.Equals(dirEdge5));
            Assert.IsFalse(undirEdge1.Equals(undirEdge5));
        }
    }
}

