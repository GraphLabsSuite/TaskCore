using System;
using System.Linq;
using GraphLabs.Graphs;
using NUnit.Framework;

namespace GraphLabs.Tests.Graphs
{
    /// <summary> Тестирование преобразования графа в строку. </summary>
    public class PrinterTests
    {
        /// <summary> ... </summary>
        [Test]
        public void PrinterTest()
        {
            // Граф из пяти вершин и шести ребер 
            var graph1 = new UndirectedGraph();
            var vertexA = new Vertex("A");
            var vertexB = new Vertex("B");
            var vertexC = new Vertex("C");
            var vertexD = new Vertex("D");
            var vertexE = new Vertex("E");

            graph1.AddEdge(new UndirectedEdge(vertexA, vertexB));
            graph1.AddEdge(new UndirectedEdge(vertexA, vertexC));
            graph1.AddEdge(new UndirectedEdge(vertexB, vertexC));
            graph1.AddEdge(new UndirectedEdge(vertexB, vertexD));
            graph1.AddEdge(new UndirectedEdge(vertexC, vertexE));
            graph1.AddEdge(new UndirectedEdge(vertexD, vertexE));

            graph1.AddVertex(vertexA);
            graph1.AddVertex(vertexB);
            graph1.AddVertex(vertexC);
            graph1.AddVertex(vertexD);
            graph1.AddVertex(vertexE);
            Assert.AreEqual(GraphPrinter.GraphToString(graph1), "({A; B; C; D; E}, {(A, B); (A, C); (B, C); (B, D); (C, E); (D, E)})");
            Assert.AreEqual(GraphPrinter.EdgesToString(graph1), "{(A, B); (A, C); (B, C); (B, D); (C, E); (D, E)}");
            Assert.AreEqual(GraphPrinter.VerticesToString(graph1), "{A; B; C; D; E}");

            // Граф без ребер
            var graph2 = new UndirectedGraph();

            var vertex1 = new Vertex("1");
            var vertex2 = new Vertex("2");
            var vertex3 = new Vertex("3");
            var vertex4 = new Vertex("4");
            var vertex5 = new Vertex("5");

            graph2.AddVertex(vertex1);
            graph2.AddVertex(vertex2);
            graph2.AddVertex(vertex3);
            graph2.AddVertex(vertex4);
            graph2.AddVertex(vertex5);

            Assert.AreEqual(GraphPrinter.GraphToString(graph1), "({1; 2; 3; 4; 5}, {})");
            Assert.AreEqual(GraphPrinter.EdgesToString(graph1), "{}");
            Assert.AreEqual(GraphPrinter.VerticesToString(graph1), "{1; 2; 3; 4; 5}");

            // Пустой граф
            var graph3 = new UndirectedGraph();
            Assert.AreEqual(GraphPrinter.GraphToString(graph1), "({}, {})");
            Assert.AreEqual(GraphPrinter.EdgesToString(graph1), "{}");
            Assert.AreEqual(GraphPrinter.VerticesToString(graph1), "{}");
        }
    }
}
