using GraphLabs.Graphs;
using NUnit.Framework;

namespace GraphLabs.Tests.Graphs

{
    /// <summary> Тестирование метода проверки изоморфизма. </summary>
    public class IsomorphismTests
    {
        /// <summary> ... </summary>
        [Test]
        public void IsomorphismTest()
        {
            var graph1 = new UndirectedGraph();
            var vertexA = new Vertex("A");
            var vertexB = new Vertex("B");
            var vertexC = new Vertex("C");
            var vertexD = new Vertex("D");
            var graph2 = new UndirectedGraph();
            var vertex1 = new Vertex("1");
            var vertex2 = new Vertex("2");
            var vertex3 = new Vertex("3");
            var vertex4 = new Vertex("4");

            graph1.AddEdge(new UndirectedEdge(vertexB, vertexC));
            graph2.AddEdge(new UndirectedEdge(vertex1, vertex3));
            graph1.AddEdge(new UndirectedEdge(vertexC, vertexD));
            graph2.AddEdge(new UndirectedEdge(vertex3, vertex4));

            graph2.AddVertex(vertex1);
            graph2.AddVertex(vertex2);
            graph2.AddVertex(vertex3);
            graph2.AddVertex(vertex4);

            graph1.AddVertex(vertexA);
            graph1.AddVertex(vertexB);
            graph1.AddVertex(vertexC);
            graph1.AddVertex(vertexD);

            Assert.IsTrue(GraphOperations.CheckIsomorphism(graph1, graph2));
        }


        /// <summary> Проверка изоморфизма между двумя регулярными графами. </summary>
        [Test]
        public void RegularIsomorphismTest()
        {
        // Graph1
            var graph1 = new UndirectedGraph();
            var vertexA = new Vertex("A");
            var vertexB = new Vertex("B");
            var vertexC = new Vertex("C");
            var vertexD = new Vertex("D");
            var vertexG = new Vertex("G");
            var vertexH = new Vertex("H");
            var vertexI = new Vertex("I");
            var vertexJ = new Vertex("J");

            graph1.AddEdge(new UndirectedEdge(vertexA, vertexG));
            graph1.AddEdge(new UndirectedEdge(vertexA, vertexH));
            graph1.AddEdge(new UndirectedEdge(vertexA, vertexI));
            graph1.AddEdge(new UndirectedEdge(vertexG, vertexB));
            graph1.AddEdge(new UndirectedEdge(vertexG, vertexC));
            graph1.AddEdge(new UndirectedEdge(vertexB, vertexH));
            graph1.AddEdge(new UndirectedEdge(vertexB, vertexJ));
            graph1.AddEdge(new UndirectedEdge(vertexH, vertexD));
            graph1.AddEdge(new UndirectedEdge(vertexC, vertexI));
            graph1.AddEdge(new UndirectedEdge(vertexC, vertexJ));
            graph1.AddEdge(new UndirectedEdge(vertexD, vertexI));
            graph1.AddEdge(new UndirectedEdge(vertexD, vertexJ));

            graph1.AddVertex(vertexA);
            graph1.AddVertex(vertexB);
            graph1.AddVertex(vertexC);
            graph1.AddVertex(vertexD);
            graph1.AddVertex(vertexG);
            graph1.AddVertex(vertexH);
            graph1.AddVertex(vertexI);
            graph1.AddVertex(vertexJ);

            // Graph2
            var graph2 = new UndirectedGraph();

            var vertex1 = new Vertex("1");
            var vertex2 = new Vertex("2");
            var vertex3 = new Vertex("3");
            var vertex4 = new Vertex("4");
            var vertex5 = new Vertex("5");
            var vertex6 = new Vertex("6");
            var vertex7 = new Vertex("7");
            var vertex8 = new Vertex("8");

            graph2.AddEdge(new UndirectedEdge(vertex1, vertex2));
            graph2.AddEdge(new UndirectedEdge(vertex1, vertex5));
            graph2.AddEdge(new UndirectedEdge(vertex1, vertex4));
            graph2.AddEdge(new UndirectedEdge(vertex4, vertex3));
            graph2.AddEdge(new UndirectedEdge(vertex4, vertex8));
            graph2.AddEdge(new UndirectedEdge(vertex3, vertex7));
            graph2.AddEdge(new UndirectedEdge(vertex3, vertex2));
            graph2.AddEdge(new UndirectedEdge(vertex2, vertex6));
            graph2.AddEdge(new UndirectedEdge(vertex5, vertex6));
            graph2.AddEdge(new UndirectedEdge(vertex5, vertex8));
            graph2.AddEdge(new UndirectedEdge(vertex7, vertex6));
            graph2.AddEdge(new UndirectedEdge(vertex7, vertex8));

            graph2.AddVertex(vertex1);
            graph2.AddVertex(vertex2);
            graph2.AddVertex(vertex3);
            graph2.AddVertex(vertex4);
            graph2.AddVertex(vertex5);
            graph2.AddVertex(vertex6);
            graph2.AddVertex(vertex7);
            graph2.AddVertex(vertex8);

            Assert.IsTrue(GraphOperations.CheckIsomorphism(graph1, graph2));
        }
    }
}
